using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SureviewDesktopApplicationTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<InputData> inputDataList = new ObservableCollection<InputData>();
        private int serverNumber = 0;
        private int alarmNumber = 0;
        private bool validResult = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            window.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            window.WindowState = WindowState.Minimized;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(((TextBox)sender).Text == "Enter text here")
            {
                ((TextBox)sender).Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(((TextBox)sender).Text == "")
            {
                ((TextBox)sender).Text = "Enter text here";
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                CheckInput();
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            CheckInput();
        }

        private void CheckInput()
        {
            string input = contentInput.Text;
            validResult = false;
            serverNumber = 0;
            alarmNumber = 0;

            string pattern = @"\b(server|alarm)\s*(\d+)\b|\b(\d+)\b";
            MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

            foreach(Match match in matches)
            {
                string? keyword = match.Groups[1].Success ? match.Groups[1].Value.ToLower() : null;
                int number = int.Parse(match.Groups[2].Success ? match.Groups[2].Value : match.Groups[3].Value);
                if(keyword == "server" && serverNumber == 0)
                {
                    serverNumber = number;
                }
                else if(keyword == "alarm" && alarmNumber == 0)
                {
                    alarmNumber = number;
                }
                else if(serverNumber == 0)
                {
                    serverNumber = number;
                }
                else if(alarmNumber == 0)
                {
                    alarmNumber = number;
                }
            }

            if(serverNumber != 0 && alarmNumber != 0)
            {
                resultLabel.Content = ("Alarm id " + alarmNumber + " has been received from video server number " + serverNumber + ".");
                validResult = true;
            }
            else
            {
                resultLabel.Content = ("Invalid input. Please enter both server and alarm numbers.");
            }
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if(validResult)
            {
                var inputData = new InputData
                {
                    ServerNumber = serverNumber,
                    AlarmNumber = alarmNumber,
                    Timestamp = DateTime.Now
                };

                inputDataList.Add(inputData);
                UpdateInputTable();

                contentInput.Text = "Enter text here";
                resultLabel.Content = ("Result: ");

                validResult = false;
                serverNumber = 0;
                alarmNumber = 0;
            }
        }

        private void UpdateInputTable()
        {
            InputTable.ItemsSource = null;
            InputTable.ItemsSource = inputDataList;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if(button != null)
            {
                var item = button.DataContext as InputData;

                if(item != null)
                {
                    inputDataList.Remove(item);
                }
            }
        }

        public class InputData
        {
            public int ServerNumber { get; set; }
            public int AlarmNumber { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}

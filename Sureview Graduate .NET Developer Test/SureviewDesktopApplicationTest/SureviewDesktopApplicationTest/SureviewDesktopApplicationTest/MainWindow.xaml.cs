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

        public MainWindow()
        {
            InitializeComponent();
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
                string input = ((TextBox)sender).Text;
                Submit(input);
            }
        }

        private void Submit(string input)
        {
            Console.WriteLine("Please enter the relevant information.\nNote that if you only enter numbers then the input will be taken as SERVER NUMBER & ALARM NUMBER respectively.");

            int serverNumber = 0;
            int alarmNumber = 0;

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

                var inputData = new InputData
                {
                    ServerNumber = serverNumber,
                    AlarmNumber = alarmNumber,
                    Timestamp = DateTime.Now
                };

                inputDataList.Add(inputData);
                UpdateInputTable();
            }
            else
            {
                serverNumber = 0;
                alarmNumber = 0;

                resultLabel.Content =  ("Invalid input. Please enter both server and alarm numbers.");
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string input = contentInput.Text;
            int serverNumber = 0;
            int alarmNumber = 0;

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

                var inputData = new InputData
                {
                    ServerNumber = serverNumber,
                    AlarmNumber = alarmNumber,
                    Timestamp = DateTime.Now
                };

                inputDataList.Add(inputData);
                UpdateInputTable();
            }
            else
            {
                serverNumber = 0;
                alarmNumber = 0;

                resultLabel.Content = ("Invalid input. Please enter both server and alarm numbers.");
            }
        }

        private void UpdateInputTable()
        {
            InputTable.ItemsSource = null;
            InputTable.ItemsSource = inputDataList;
        }

        public class InputData
        {
            public int ServerNumber { get; set; }
            public int AlarmNumber { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}

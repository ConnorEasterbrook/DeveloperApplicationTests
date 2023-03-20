using System.Text.RegularExpressions;

namespace SureviewApplicationTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the relevant information.\nNote that if you only enter numbers then the input will be taken as SERVER NUMBER & ALARM NUMBER respectively.");

            int serverNumber = 0;
            int alarmNumber = 0;

            while(serverNumber == 0 || alarmNumber == 0)
            {
                string input = Console.ReadLine();
                string pattern = @"\b(server|alarm)\s*(\d+)\b|\b(\d+)\b";
                MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

                foreach(Match match in matches)
                {
                    string keyword = match.Groups[1].Success ? match.Groups[1].Value.ToLower() : null;
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
                    Console.WriteLine("Alarm id " + alarmNumber + " has been received from video server number " + serverNumber + ".");
                }
                else
                {
                    serverNumber = 0;
                    alarmNumber = 0;

                    Console.WriteLine("Invalid input. Please enter both server and alarm numbers.");
                }
            }
        }
    }
}
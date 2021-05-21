using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TinyBrowser
{
    public class Program
    {
        private static string _host = "acme.com";
        private static List<UrlData> _globalHistory   = new List<UrlData>();
        private static List<UrlData> _navigateHistory = new List<UrlData>();
        private const string RegPattern = "https://|http://";
        private static int _historyPointer = 0;
        private static string _url = "https://acme.com/";
        private const int Port = 80;

        static void Main(string[] arguments)
        {
            _globalHistory.Add(new UrlData(null, _host));
            _navigateHistory.Add(new UrlData(null, _host));
            
            Regex rx = new Regex($@"({RegPattern})", RegexOptions.IgnoreCase);

            while (true)
            {
                Console.ResetColor();
                string readWeb = WebsiteConnect.Connect(_url, _host, Port);
                
                List<UrlData> hyperLinks = StringExtensions.GetNameAndUrl(readWeb, "<a href=\"");
                hyperLinks.RemoveSpecifiedElement("<img");

                int hyperLinksCount = 0;
                foreach (UrlData hyperLink in hyperLinks)
                {
                    Console.WriteLine($"{hyperLinksCount}: {hyperLink.DisplayName.Prettify()} ({hyperLink.Path})");
                    hyperLinksCount++;
                }
            
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(hyperLinksCount > 0
                    ? $"\nNavigate the site ({_url}) by choosing an index between 0 and {hyperLinksCount - 1} or the letters (b)ack, (f)orward, (r)efresh, (h)istory"
                    : $"\n No hyperlinks found. Navigate the site ({_url}) by choosing the letters (b)ack, (f)orward, (r)efresh, (h)istory");

                while (true)
                {
                    string userInput = Console.ReadLine();
                    
                    try
                    {
                        if (userInput != null)
                        {
                            switch (userInput.ToLower())
                            {
                                case "b":
                                    _url = _navigateHistory[_navigateHistory.Count - ++_historyPointer - 1].Path;
                                    break;

                                case "f":
                                    _url = _navigateHistory[_navigateHistory.Count - --_historyPointer - 1].Path;
                                    break;

                                case "r":
                                    break;

                                case "h":
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine("History");
                                    for (int i = 0; i < _globalHistory.Count; i++) 
                                        Console.WriteLine($"{i}: {_globalHistory[i].DisplayName.Prettify()} ({_globalHistory[i].Host}{_globalHistory[i].Path})");

                                    Console.ReadKey();
                                    break;

                                default:
                                    int temp = Convert.ToInt32(userInput);
                                    if (temp >= hyperLinksCount || temp < 0)
                                        throw new Exception();

                                    if (rx.IsMatch(hyperLinks[temp].Path))
                                    {
                                        _host = new Uri(hyperLinks[temp].Path).Host;
                                        _url = new Uri(hyperLinks[temp].Path).PathAndQuery;
                                        
                                        Console.WriteLine($"{_host}  {_url}");
                                    }
                                    else 
                                        _url = hyperLinks[temp].Path;

                                    _globalHistory.Add(hyperLinks[temp]);
                                    
                                    _navigateHistory.RemoveRange(_navigateHistory.Count - _historyPointer, _historyPointer);
                                    _navigateHistory.Add(hyperLinks[temp]);
                                    
                                    _historyPointer = 0;
                                    break;
                            }
                        }
                        
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Only Numbers between 0 and {hyperLinksCount - 1} and the letters b, f, r, h is allowed");
                    }
                }
            }
        }
    }
}
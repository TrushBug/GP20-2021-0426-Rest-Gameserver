using System;
using System.Collections.Generic;

namespace TinyBrowser
{
    public class Program
    {
        private static string _host = "acme.com";
        private static List<UrlData> _globalHistory   = new List<UrlData>();
        private static List<UrlData> _navigateHistory = new List<UrlData>();
        private static int _historyPointer = 0;
        private static string _url;
        private const int Port = 80;

        static void Main(string[] arguments)
        {
            _globalHistory.Add(new UrlData(null, _host));
            _navigateHistory.Add(new UrlData(null, _host));
            
            while (true)
            {
                Console.ResetColor();
                string readWeb = WebsiteConnect.Connect(_host, _url, Port);
                
                List<UrlData> hyperLinks = StringExtensions.GetNameAndUrl(readWeb, "<a href=\"");
                hyperLinks.RemoveSpecifiedElement("<img");

                int hyperLinksCount = 0;
                foreach (UrlData hyperLink in hyperLinks)
                {
                    Console.WriteLine($"{hyperLinksCount}: {hyperLink.DisplayName.Prettify()} ({hyperLink.Path})");
                    hyperLinksCount++;
                }
            
                Console.ForegroundColor = ConsoleColor.Green;
                if (hyperLinksCount > 0) 
                    Console.WriteLine($"\nNavigate the site ({_host}/{_url}) by choosing an index between 0 and {hyperLinksCount - 1} or the letters (b)ack, (f)orward, (r)efresh, (h)istory");
                else
                    Console.WriteLine($"\n No hyperlinks found. Navigate the site ({_host}/{_url}) by choosing the letters (b)ack, (f)orward, (r)efresh, (h)istory");

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
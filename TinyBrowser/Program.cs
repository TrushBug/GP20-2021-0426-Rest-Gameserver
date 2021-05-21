using System;
using System.Collections.Generic;

namespace TinyBrowser
{
    public class Program
    {
        private static string _host = "acme.com";
        private static string _url = null;
        private const int Port = 80;

        static void Main(string[] arguments)
        {
            while (true)
            {
                string readWeb = WebsiteConnect.Connect(_host, _url, Port);

                List<UrlData> hyperLinks = StringExtensions.GetNameAndUrl(readWeb, "<a href=\"");

                hyperLinks.RemoveSpecifiedElement("<img");

                int hyperLinksCount = 0;
                foreach (UrlData hyperLink in hyperLinks) {
                    string prettify = $"{hyperLink.DisplayName.Substring(0, 6)}...{hyperLink.DisplayName.Substring(hyperLink.DisplayName.Length - 6)}";
                    Console.WriteLine($"{hyperLinksCount}: {hyperLink.DisplayName} ({hyperLink.Url})");
                    hyperLinksCount++;
                }
            
                Console.WriteLine($"\nNavigate the site ({_host}/{_url}) by choosing an index between 0 and {hyperLinksCount - 1}");

                try
                {
                    int userIndex = Convert.ToInt32(Console.ReadLine());
                    if (userIndex >= hyperLinksCount || userIndex < 0) 
                        throw new Exception();

                    _url = hyperLinks[userIndex].Url;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Only Numbers between 0 and {hyperLinksCount - 1} allowed");
                }
            }
        }
    }
}
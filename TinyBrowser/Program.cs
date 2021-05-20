using System;
using System.Collections.Generic;

namespace TinyBrowser
{
    public class Program {
        
        static void Main(string[] arguments)
        {
            string readWeb = WebsiteConnect.Connect("acme.com", 80);

            List<UrlData> hyperLinks = StringExtensions.GetNameAndUrl(readWeb, "<a href=\"");

            int hyperLinksCount = 0;
            foreach (UrlData hyperLink in hyperLinks)
            {
                string prettify = $"{hyperLink.DisplayName.Substring(0, 6)}...{hyperLink.DisplayName.Substring(hyperLink.DisplayName.Length - 6)}";

                if (hyperLink.DisplayName.Contains("<img")) continue;
                Console.WriteLine($"{hyperLinksCount}: {hyperLink.DisplayName} ({hyperLink.Url})");
                hyperLinksCount++;
            }

        }
    }
}
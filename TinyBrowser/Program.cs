using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser
{
    public class Program {
        
        static void Main(string[] arguments) {
            string host = "acme.com";
            string uri = "/";
            
            TcpClient tcpClient = new TcpClient(host, 80);
            NetworkStream stream = tcpClient.GetStream();
            StreamWriter streamWriter = new StreamWriter(stream, Encoding.ASCII);

            string request = $"GET {uri} HTTP/1.1\r\nHost: {host}\r\n\r\n";
            streamWriter.Write(request);
            streamWriter.Flush();

            StreamReader streamReader = new StreamReader(stream);
            string response = streamReader.ReadToEnd();

            UriBuilder uriBuilder = new UriBuilder(null, host) {Path = uri};
            
            Console.WriteLine($"Opened {uriBuilder}");
            Console.WriteLine("Title: " + response.FindTextBetween("<title>", "</title>").Substring("<title>".Length));

            List<int> allIndexesOf = response.AllIndexesOf("<a href=\"");
            
            for (int i = 0; i < allIndexesOf.Count; i++) {
                string url = response.FindTextBetween(allIndexesOf[i] + "<a href=\"".Length, "\"");
                string bread = response.FindTextBetween(allIndexesOf[i] + ("<a href=\"" + url + "\">").Length, "</a>");

                string prettify = $"{bread.Substring(0, 6)}...{bread.Substring(bread.Length - 6)}";
                Console.WriteLine($"{i}: {prettify} ({url})");
            }
        }
    }
}
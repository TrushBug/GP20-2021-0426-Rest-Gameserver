using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser
{
    public class Program {
        
        static void Main(string[] arguments) {
            string host = "acme.com";
            string uri = "/";
            // Here, acme.com is only used for DNS-Resolving and gives us an IP
            TcpClient tcpClient = new TcpClient(host, 80);
            NetworkStream stream = tcpClient.GetStream();
            StreamWriter streamWriter = new StreamWriter(stream, Encoding.ASCII);
            // Here, acme.com is passed to the Webserver at the IP, so it knows, which website to give us
            // In case, that one computer hosts multiple websites (think of Web-Hosts like wix.com)
        
            /*
         * GET / HTTP.1.1
         * Host: acme.com
         * Content-Length: 7
         *
         * abcdefg
         */
        
            // This is a valid HTTP/1.1-Request to send:
            string request = $"GET {uri} HTTP/1.1\r\nHost: {host}\r\n\r\n";
            streamWriter.Write(request); // add data to the buffer
            streamWriter.Flush(); // actually send the buffered data

            StreamReader streamReader = new StreamReader(stream);
            string response = streamReader.ReadToEnd();

            UriBuilder uriBuilder = new UriBuilder(null, host);
            uriBuilder.Path = uri;
            Console.WriteLine($"Opened {uriBuilder}");
            Console.WriteLine("Title: " + response.FindTextBetween("<title>", "</title>"));
            
            string test = "<a href=\"https";

            foreach (int index in response.AllIndexesOf(test))
            {
                Console.WriteLine(response.FindTextBetween(index + "<a href=\"".Length, "\""));
            }
            
            //var titleText = FindTextBetweenTags(response, "<title>", "</title>");
        }
    }
}
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser
{
    public static class WebsiteConnect
    {
        private const string Uri = "/";

        public static string Connect (string url, string host, int port)
        {
            string path = url[0] == '/' ? url : "/" + url;
            try
            {
                if (url.Contains("//"))
                {
                    path = new Uri(url).PathAndQuery;
                    //host = new Uri(url).Host;
                }
                    
            }
            catch (Exception e){/*Ignored*/}
            
            using TcpClient tcpClient = new TcpClient(host, port);
            using NetworkStream stream = tcpClient.GetStream();
            using StreamWriter streamWriter = new StreamWriter(stream, Encoding.ASCII);
            using StreamReader streamReader = new StreamReader(stream);
            

            string request = $"GET {path} HTTP/1.1\r\nHost: {host}\r\n\r\n";
            streamWriter.Write(request);
            streamWriter.Flush();
            
            string response = streamReader.ReadToEnd();
            
            UriBuilder uriBuilder = new UriBuilder(null, host) {Path = path};
    
            Console.WriteLine($"\nOpened {uriBuilder}");

            try
            {
                Console.WriteLine("Title: " + response.FindTextBetween("<title>", "</title>").Substring("<title>".Length));
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't find title of website");
            }
            

            return response;
        }
    }
}
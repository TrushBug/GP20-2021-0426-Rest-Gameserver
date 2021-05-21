using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser
{
    public static class WebsiteConnect
    {
        private const string Uri = "/";

        public static string Connect (string host, string url, int port)
        {

            using TcpClient tcpClient = new TcpClient(host, port);
            using NetworkStream stream = tcpClient.GetStream();
            using StreamWriter streamWriter = new StreamWriter(stream, Encoding.ASCII);
            using StreamReader streamReader = new StreamReader(stream);

            string request = $"GET {Uri + url} HTTP/1.1\r\nHost: {host}\r\n\r\n";
            streamWriter.Write(request);
            streamWriter.Flush();
            
            string response = streamReader.ReadToEnd();

            UriBuilder uriBuilder = new UriBuilder(null, host) {Path = Uri + url};
    
            Console.WriteLine($"Opened {uriBuilder}");
            Console.WriteLine("Title: " + response.FindTextBetween("<title>", "</title>").Substring("<title>".Length));

            return response;
        }
    }
}
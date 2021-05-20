using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser
{
    public static class WebsiteConnect
    {
        private const string Uri = "/";

        public static string Connect (string host, int port)
        {
            TcpClient tcpClient = new TcpClient(host, port);
            NetworkStream stream = tcpClient.GetStream();
            StreamWriter streamWriter = new StreamWriter(stream, Encoding.ASCII);

            string request = $"GET {Uri} HTTP/1.1\r\nHost: {host}\r\n\r\n";
            streamWriter.Write(request);
            streamWriter.Flush();

            StreamReader streamReader = new StreamReader(stream);
            string response = streamReader.ReadToEnd();

            UriBuilder uriBuilder = new UriBuilder(null, host) {Path = Uri};
            
            Console.WriteLine($"Opened {uriBuilder}");
            Console.WriteLine("Title: " + response.FindTextBetween("<title>", "</title>").Substring("<title>".Length));
            
            return response;
        }
    }
}
using Grpc.Net.Client;
using GrpcService1;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Http;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:12345/");
            listener.Start();
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            string json = "";
            var Server = new WebServer();
            while (true)
            {
                 Server.Server(client, listener, json);
            }
          
        }
    }
}

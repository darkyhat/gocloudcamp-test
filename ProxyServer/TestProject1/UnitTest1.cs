using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp2;
using System.Net;
using System;
using Grpc.Net.Client;
using GrpcService1;
using Newtonsoft.Json;

namespace TestProject1
{
    class servicename
    {
        public string Servicename { get; set; }
    }
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void StartListener1()
        {
            HttpListener listener = new HttpListener();
            listener.Start();
        }


        [TestMethod]
        public void StartGrpc()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
        }



        [TestMethod]
        public void GetPath()
        {
            Uri uri = new Uri("http://localhost:12345/update");
            Assert.AreEqual(Path(uri), "Was updated");
            uri = new Uri("http://localhost:12345/read");
            Assert.AreEqual(Path(uri), "Was read");
            uri = new Uri("http://localhost:12345/create");
            Assert.AreEqual(Path(uri), "Was updated created");
            uri = new Uri("http://localhost:12345/delete");
            Assert.AreEqual(Path(uri), "Was deleted");
        }

        [TestMethod]
        public void GreeterTest() //only while GrpcService is running
        {
            var serv = new servicename();
            serv.Servicename = "test";
            string json = "";
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);

            json = JsonConvert.SerializeObject(serv);

            var reply4 = client.SayHello(new HelloRequest { });
            var reply = client.SayHello(new HelloRequest { Name = "create", Message = json });
            Assert.AreEqual(reply.Message, "File was created");

            reply = client.SayHello(new HelloRequest { Name = "update", Message = json });
            Assert.AreEqual(reply.Message, "File was updated");

            reply = client.SayHello(new HelloRequest { Name = "delete", Message = json });
            Assert.AreEqual(reply.Message, "File was deleted");

        }



        public string Path(Uri uri)
        {
            switch (uri.AbsolutePath)
            {
                case "/update":
                    return "Was updated";

                case "/create":
                    return "Was updated created";

                case "/delete":
                    return "Was deleted";

                case "/read":
                    return "Was read";
            }
            return "No path";
        }
    }
}

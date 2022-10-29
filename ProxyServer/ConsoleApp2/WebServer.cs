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
    public class WebServer
    {

       public WebServer()
        {
            
        }

        public int Server(Greeter.GreeterClient client, HttpListener listener, string json)
        {
            json = "";
            HttpListenerContext ctx = listener.GetContext();
            HttpListenerRequest request = ctx.Request;
            HttpListenerResponse resp = ctx.Response;
            switch (request.Url.AbsolutePath)
            {

                //POST: http://localhost:12345/update
                //{ "Servicename":"kate", "key1":"0x7h45y", "key2":"0x3f67d" }
                case "/update":
                    if (request.HttpMethod != HttpMethod.Post.Method)
                        return response("Method Is Not Post", ctx);

                    json = new StreamReader(request.InputStream).ReadToEnd();
                    Console.WriteLine(json);
                    var reply = client.SayHello(new HelloRequest { Name = "update", Message = json });
                    return response(reply.Message, ctx);

                case "/create":
                    if (request.HttpMethod != HttpMethod.Post.Method)
                        return response("Method Is Not Post", ctx);

                    json = new StreamReader(request.InputStream).ReadToEnd();
                    reply = client.SayHello(new HelloRequest { Name = "create", Message = json });
                    return response(reply.Message, ctx);

                case "/delete":
                    if (request.HttpMethod != HttpMethod.Post.Method)
                        return response("Method Is Not Post", ctx);

                    json = new StreamReader(request.InputStream).ReadToEnd();
                    reply = client.SayHello(new HelloRequest { Name = "delete", Message = json });
                    return response(reply.Message, ctx);

                case "/read":
                    if (request.HttpMethod != HttpMethod.Get.Method)
                        return response("Method Is Not Get", ctx);

                    json = new StreamReader(request.InputStream).ReadToEnd();
                    reply = client.SayHello(new HelloRequest { Name = "read", Message = json });
                    return response(reply.Message, ctx);
            }
            return 1;
        }

        public int response(string message, HttpListenerContext ctx)
        {
            HttpListenerResponse response = ctx.Response;
            string responseString = message;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            return 1;
        }
       
    }
}

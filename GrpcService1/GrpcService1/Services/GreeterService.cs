using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using ProtoBuf;
using Newtonsoft.Json;



namespace GrpcService1
{
    class servicename
    {
        public string Servicename { get; set; }
    }
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            servicename trx = JsonConvert.DeserializeObject<servicename>(request.Message);
            switch (request.Name)
            {
                case "create":
                    if ((File.Exists($"{trx.Servicename}.bin")))
                    {
                        return Task.FromResult(new HelloReply { Message = "File already exists" });
                    }
                    var myFile = File.Create($"{trx.Servicename}.bin");
                    myFile.Close();
                    return Task.FromResult(new HelloReply{Message = "File was created"});

                case "read":
                    if (!(File.Exists($"{trx.Servicename}.bin")))
                    {
                        return Task.FromResult(new HelloReply{Message = "there is no such service" });
                    }
                    string file = File.ReadAllText($"{trx.Servicename}.bin");
                    return Task.FromResult(new HelloReply{Message = file});
                   
                case "update":
                    if (!(File.Exists($"{trx.Servicename}.bin")))
                    {
                        return Task.FromResult(new HelloReply{Message = "there is no such service" });
                    }
                    double vers = 1.00;
                    string version = "";
                    if (new FileInfo($"{trx.Servicename}.bin").Length != 0)
                    {
                        version = File.ReadLines($"{trx.Servicename}.bin").Last();
                    }
                    if (version.Contains(":"))
                    {
                        string[] parts = version.Split(':');
                        version = parts[1];
                        vers = Convert.ToDouble(version) + 0.01;
                    }
                    File.AppendAllText($"{trx.Servicename}.bin", request.Message + "\n");
                    File.AppendAllText($"{trx.Servicename}.bin", "Version:" + vers.ToString() + "\n");
                    return Task.FromResult(new HelloReply{Message = "File was updated"});
                
                case "delete":
                    if (!(File.Exists($"{trx.Servicename}.bin")))
                    {
                        return Task.FromResult(new HelloReply{Message = "there is no such service" });
                    }
                    string path = $"{trx.Servicename}.bin";
                    FileInfo filepath = new FileInfo(path);
                    if (IsFileLocked(filepath))
                    {
                        return Task.FromResult(new HelloReply{Message = "File is locked"});
                    }
                    else 
                    {
                        File.Delete($"{trx.Servicename}.bin");
                        return Task.FromResult(new HelloReply
                        {
                            Message = "File was deleted"
                        });
                    }
                    


            }
            
            return Task.FromResult(new HelloReply
            {
                Message = "Trying..." + request.Name
            });
        }
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }
    }

}

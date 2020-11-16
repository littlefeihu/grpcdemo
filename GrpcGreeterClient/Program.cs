using Grpc.Net.Client;
using GrpcExample;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcGreeterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {

            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
           // var client = new Greeter.GreeterClient(channel);
            var client1 = new GrpcExample.ExampleService_Pro.ExampleService_ProClient(channel);

          //  var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });

            var reply1 = await client1.UnaryCallAsync(new ExampleRequest {  PageSize=1 });

            Console.WriteLine("Greeting: " + reply1.Message);


            var response= client1.StreamingFromServer(new ExampleRequest());
            CancellationToken cancellationToken;
            while (await response.ResponseStream.MoveNext(cancellationToken))
            {
                var message = response.ResponseStream.Current.Message;
                Console.WriteLine("StreamingFromServer:"+message);
            }


            Console.WriteLine("Greeting: " + response.ResponseStream);

           // Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

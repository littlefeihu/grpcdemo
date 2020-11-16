using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcExample;

namespace GrpcGreeter.Services
{
    public class ExampleService : ExampleService_Pro.ExampleService_ProBase
    {
        //一元方法
        public override Task<ExampleResponse> UnaryCall(ExampleRequest request, ServerCallContext context)
        {
            var response = new ExampleResponse() { Message = "UnaryCall" };
            return Task.FromResult(response);
        }
        //服务器流式处理方法
        public override async Task StreamingFromServer(ExampleRequest request, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
        {
            for (var i = 0; i < 5; i++)
            {
                await responseStream.WriteAsync(new ExampleResponse { Message = i.ToString() });
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
        //客户端流式处理方法
        public override async Task<ExampleResponse> StreamingFromClient(IAsyncStreamReader<ExampleRequest> requestStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var message = requestStream.Current;
                // ...
            }
            return new ExampleResponse();
        }
        //双向流式处理方法
        public override async Task StreamingBothWays(IAsyncStreamReader<ExampleRequest> requestStream,
    IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
        {
            // Read requests in a background task.
            var readTask = Task.Run(async () =>
            {
                await foreach (var message in requestStream.ReadAllAsync())
                {
                    // Process request.
                }
            });

            // Send responses until the client signals that it is complete.
            while (!readTask.IsCompleted)
            {
                await responseStream.WriteAsync(new ExampleResponse());
                await Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
            }
        }
    }
}

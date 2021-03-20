using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace grpcServer
{
    public class ServerStreamService : ServerStream.ServerStreamBase
    {
        private readonly ILogger<ServerStreamService> _logger;
        public ServerStreamService(ILogger<ServerStreamService> logger)
        {
            _logger = logger;
        }

        public override async Task Add(ServerStremRequest request,IServerStreamWriter<ServerStremReply> replyStream, ServerCallContext context)
        {
            await Task.Run(async () =>
            {
                foreach(var req in request.NumberList)
                {
                    await replyStream.WriteAsync(new ServerStremReply { Result = req.Number1 + req.Number2});
                    await Task.Delay(500);
                }
            });
        }

        public override async Task Sub(ServerStremRequest request,IServerStreamWriter<ServerStremReply> replyStream, ServerCallContext context)
        {
            await Task.Run(async () =>
            {
                foreach(var req in request.NumberList)
                {
                    await replyStream.WriteAsync(new ServerStremReply { Result = req.Number1 - req.Number2});
                    await Task.Delay(500);
                }
            });
        }
    }
}

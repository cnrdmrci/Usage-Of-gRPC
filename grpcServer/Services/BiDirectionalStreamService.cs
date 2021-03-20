using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace grpcServer
{
    public class BiDirectionalStreamService : BiDirectionalStream.BiDirectionalStreamBase
    {
        private readonly ILogger<BiDirectionalStreamService> _logger;
        public BiDirectionalStreamService(ILogger<BiDirectionalStreamService> logger)
        {
            _logger = logger;
        }

        public override async Task Add(IAsyncStreamReader<BiDirectionalStreamRequest> requestStream,IServerStreamWriter<BiDirectionalStreamReply> replyStream, ServerCallContext context)
        {
            await Task.Run(async () =>
            {
                while (await requestStream.MoveNext())
                {
                    await replyStream.WriteAsync(new BiDirectionalStreamReply { Result = requestStream.Current.Number1 + requestStream.Current.Number2 });
                }
            });
        }
    }
}

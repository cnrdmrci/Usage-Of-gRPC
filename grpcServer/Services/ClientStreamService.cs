using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace grpcServer
{
    public class ClientStreamService : ClientStream.ClientStreamBase
    {
        private readonly ILogger<ClientStreamService> _logger;
        public ClientStreamService(ILogger<ClientStreamService> logger)
        {
            _logger = logger;
        }

        public override async Task<ClientStreamReply> Add(IAsyncStreamReader<ClientStreamRequest> requestStream,ServerCallContext context)
        {
            int result = 0;
            await Task.Run(async () =>
            {
                while (await requestStream.MoveNext())
                {
                    result += requestStream.Current.Number;
                }
            });
    
            return new ClientStreamReply { Result = result };
        }
    }
}

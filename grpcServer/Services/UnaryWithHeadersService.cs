using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace grpcServer
{
    public class UnaryWithHeadersService : UnaryWithHeaders.UnaryWithHeadersBase
    {
        private readonly ILogger<UnaryWithHeadersService> _logger;
        public UnaryWithHeadersService(ILogger<UnaryWithHeadersService> logger)
        {
            _logger = logger;
        }

        public override async Task<UnaryWithHeadersReply> Add(UnaryWithHeadersRequest request, ServerCallContext context)
        {
            var authorizationKey = context.RequestHeaders.Get("authorization")?.Value;
            return await Task.FromResult(new UnaryWithHeadersReply(){ Result = request.Number1 + request.Number2 });
        }
    }
}

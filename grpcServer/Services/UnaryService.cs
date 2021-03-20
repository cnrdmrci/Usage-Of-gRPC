using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace grpcServer
{
    public class UnaryService : Unary.UnaryBase
    {
        private readonly ILogger<UnaryService> _logger;
        public UnaryService(ILogger<UnaryService> logger)
        {
            _logger = logger;
        }

        public override async Task<UnaryReply> Add(UnaryRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new UnaryReply(){ Result = request.Number1 + request.Number2 });
        }

        public override async Task<UnaryReply> Sub(UnaryRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new UnaryReply(){ Result = request.Number1 - request.Number2 });
        }
    }
}

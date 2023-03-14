using MediatR;
using Microsoft.Extensions.Caching.Memory;
using NoCap.Request;

namespace NoCap.Handlers

{
    public class CheckCodeHandler : IRequestHandler<CheckCodeRequest, CheckCodeResult>
    {
        private readonly IMemoryCache _memoryCache;

        public CheckCodeHandler(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public async Task<CheckCodeResult> Handle(CheckCodeRequest request, CancellationToken cancellationToken)
        {
            int code = request.Code;

            if (_memoryCache.TryGetValue(request.Email, out code) )
            {
                return new CheckCodeResult { Success = true };
            }
            else
            {
                return new CheckCodeResult { Success = false };
            }
        }
    }
}
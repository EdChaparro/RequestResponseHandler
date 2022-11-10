using IntrepidProducts.RequestResponse.Requests;
using System;

namespace IntrepidProducts.RequestResponse.Responses
{
    public interface IResponse
    {
        IRequest OriginalRequest { get; }

        DateTime CompletedUtcTime { get; set; }

        ErrorInfo? ErrorInfo { get; set; }

        bool IsSuccessful { get; }
    }

    public abstract class AbstractResponse : IResponse
    {
        protected AbstractResponse(IRequest originalRequest, ErrorInfo? errorInfo = null)
        {
            OriginalRequest = originalRequest;
            ErrorInfo = errorInfo;
        }

        public IRequest OriginalRequest { get; }

        public DateTime CompletedUtcTime { get; set; }

        public ErrorInfo? ErrorInfo { get; set; }

        public bool IsSuccessful => ErrorInfo == null;
    }
}
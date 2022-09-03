using IntrepidProducts.RequestResponseHandler.Requests;

namespace IntrepidProducts.RequestResponseHandler.Responses
{
    public interface IResponse
    {
        IRequest OriginalRequest { get; }

        ErrorInfo? ErrorInfo { get; set; }

        bool IsSuccessful { get; }
    }

    public abstract class ResponseAbstract : IResponse
    {
        protected ResponseAbstract(IRequest originalRequest, ErrorInfo? errorInfo = null)
        {
            OriginalRequest = originalRequest;
            ErrorInfo = errorInfo;
        }

        public IRequest OriginalRequest { get; }

        public ErrorInfo? ErrorInfo { get; set; }

        public bool IsSuccessful => ErrorInfo == null;
    }
}
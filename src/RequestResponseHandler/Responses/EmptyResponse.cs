using IntrepidProducts.RequestResponseHandler.Requests;

namespace IntrepidProducts.RequestResponseHandler.Responses
{
    public class EmptyResponse : ResponseAbstract
    {
        public EmptyResponse(IRequest originalRequest, ErrorInfo? errorInfo = null) : base(originalRequest, errorInfo)
        {}
    }
}
using IntrepidProducts.RequestResponse.Requests;

namespace IntrepidProducts.RequestResponse.Responses
{
    public class EmptyResponse : ResponseAbstract
    {
        public EmptyResponse(IRequest originalRequest, ErrorInfo? errorInfo = null) : base(originalRequest, errorInfo)
        {}
    }
}
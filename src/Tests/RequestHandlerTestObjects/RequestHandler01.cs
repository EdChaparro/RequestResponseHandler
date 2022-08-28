using IntrepidProducts.RequestResponseHandler.Handlers;
using IntrepidProducts.RequestResponseHandler.Requests;
using IntrepidProducts.RequestResponseHandler.Responses;

namespace IntrepidProducts.RequestHandlerTestObjects
{
    public class Request01 : RequestAbstract
    {}

    public class RequestHandler01 : RequestHandlerAbstract<Request01, EmptyResponse>
    {
        protected override EmptyResponse DoHandle(Request01 request)
        {
            return new EmptyResponse(request);
        }
    }
}
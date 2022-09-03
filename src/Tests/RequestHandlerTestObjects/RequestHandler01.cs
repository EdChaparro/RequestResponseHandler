using IntrepidProducts.RequestResponseHandler.Handlers;
using IntrepidProducts.RequestResponseHandler.Requests;

namespace IntrepidProducts.RequestHandlerTestObjects
{
    public class Request01 : RequestAbstract
    {}

    public class RequestHandler01 : RequestHandlerAbstract<Request01, RequestHandlerTypeResponse>
    {
        protected override RequestHandlerTypeResponse DoHandle(Request01 request)
        {
            return new RequestHandlerTypeResponse(request) { RequestHandlerType = GetType() };
        }
    }
}
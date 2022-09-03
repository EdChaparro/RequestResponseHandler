using IntrepidProducts.RequestResponseHandler.Handlers;
using IntrepidProducts.RequestResponseHandler.Requests;

namespace IntrepidProducts.RequestHandlerTestObjects
{
    public class Request02 : RequestAbstract
    {}

    public class RequestHandler02 : RequestHandlerAbstract<Request02, RequestHandlerTypeResponse>
    {
        protected override RequestHandlerTypeResponse DoHandle(Request02 request)
        {
            return new RequestHandlerTypeResponse(request) { RequestHandlerType = GetType() };
        }
    }
}
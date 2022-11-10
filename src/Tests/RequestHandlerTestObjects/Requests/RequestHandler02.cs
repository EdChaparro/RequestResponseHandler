using IntrepidProducts.RequestHandlerTestObjects.Responses;
using IntrepidProducts.RequestResponse.Requests;
using IntrepidProducts.RequestResponseHandler.Handlers;

namespace IntrepidProducts.RequestHandlerTestObjects.Requests
{
    public class Request02 : AbstractRequest
    { }

    public class RequestHandler02 : AbstractRequestHandler<Request02, RequestHandlerTypeResponse>
    {
        protected override RequestHandlerTypeResponse DoHandle(Request02 request)
        {
            return new RequestHandlerTypeResponse(request) { RequestHandlerType = GetType() };
        }
    }
}
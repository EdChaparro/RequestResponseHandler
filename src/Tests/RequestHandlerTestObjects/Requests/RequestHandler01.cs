using IntrepidProducts.RequestHandlerTestObjects.Responses;
using IntrepidProducts.RequestResponse.Requests;
using IntrepidProducts.RequestResponseHandler.Handlers;

namespace IntrepidProducts.RequestHandlerTestObjects.Requests
{
    public class Request01 : AbstractRequest
    { }

    public class RequestHandler01 : AbstractRequestHandler<Request01, RequestHandlerTypeResponse>
    {
        protected override RequestHandlerTypeResponse DoHandle(Request01 request)
        {
            return new RequestHandlerTypeResponse(request) { RequestHandlerType = GetType() };
        }
    }
}
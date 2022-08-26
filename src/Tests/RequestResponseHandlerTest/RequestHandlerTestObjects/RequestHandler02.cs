using IntrepidProducts.RequestResponseHandler.Handlers;
using IntrepidProducts.RequestResponseHandler.Requests;
using IntrepidProducts.RequestResponseHandler.Responses;

namespace IntrepidProducts.RequestResponseHandlerTest.RequestHandlerTestObjects
{
    public class Request02 : RequestAbstract
    {}

    public class RequestHandler02 : RequestHandlerAbstract<Request02, EmptyResponse>
    {
        protected override EmptyResponse DoHandle(Request02 request)
        {
            return new EmptyResponse(request);
        }
    }
}
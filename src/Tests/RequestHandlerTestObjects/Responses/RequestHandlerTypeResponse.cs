using IntrepidProducts.RequestResponse.Requests;
using IntrepidProducts.RequestResponse.Responses;
using System;

namespace IntrepidProducts.RequestHandlerTestObjects.Responses
{
    public class RequestHandlerTypeResponse : ResponseAbstract
    {
        public RequestHandlerTypeResponse(IRequest originalRequest, ErrorInfo errorInfo = null) : base(originalRequest, errorInfo)
        { }

        public Type RequestHandlerType { get; set; }
    }
}
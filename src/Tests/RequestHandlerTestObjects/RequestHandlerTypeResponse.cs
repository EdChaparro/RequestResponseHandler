using System;
using IntrepidProducts.RequestResponseHandler.Requests;
using IntrepidProducts.RequestResponseHandler.Responses;

namespace IntrepidProducts.RequestHandlerTestObjects
{
    public class RequestHandlerTypeResponse : ResponseAbstract
    {
        public RequestHandlerTypeResponse(IRequest originalRequest, ErrorInfo errorInfo = null) : base(originalRequest, errorInfo)
        {}

        public Type RequestHandlerType { get; set; }
    }
}
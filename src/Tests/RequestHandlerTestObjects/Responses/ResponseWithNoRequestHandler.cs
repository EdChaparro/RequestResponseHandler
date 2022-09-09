using System;
using IntrepidProducts.RequestResponse.Requests;
using IntrepidProducts.RequestResponse.Responses;

namespace IntrepidProducts.RequestHandlerTestObjects.Responses
{
    public class ResponseWithNoRequestHandler : ResponseAbstract
    {
        public ResponseWithNoRequestHandler(IRequest originalRequest, ErrorInfo errorInfo = null) : base(originalRequest, errorInfo)
        {}

        public Type RequestHandlerType { get; set; }
    }
}
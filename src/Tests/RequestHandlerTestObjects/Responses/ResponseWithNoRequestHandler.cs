using IntrepidProducts.RequestResponse.Requests;
using IntrepidProducts.RequestResponse.Responses;
using System;

namespace IntrepidProducts.RequestHandlerTestObjects.Responses
{
    public class ResponseWithNoRequestHandler : AbstractResponse
    {
        public ResponseWithNoRequestHandler(IRequest originalRequest, ErrorInfo errorInfo = null) : base(originalRequest, errorInfo)
        { }

        public Type RequestHandlerType { get; set; }
    }
}
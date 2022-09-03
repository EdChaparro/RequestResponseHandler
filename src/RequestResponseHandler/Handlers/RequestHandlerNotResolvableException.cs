using System;

namespace IntrepidProducts.RequestResponseHandler.Handlers
{
    public class RequestHandlerNotResolvableException : Exception
    {
        public RequestHandlerNotResolvableException(Type requestType)
        {
            RequestType = requestType;
        }

        public Type RequestType { get; private set; }
    }
}
using System;

namespace IntrepidProducts.RequestResponseHandler.Handlers.Exceptions
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
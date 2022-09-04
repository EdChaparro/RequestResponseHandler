using System;

namespace IntrepidProducts.RequestResponseHandler.Handlers.Exceptions
{
    public class RequestHandlerNotRegisteredException : Exception
    {
        public RequestHandlerNotRegisteredException(Type requestType)
        {
            RequestType = requestType;
        }

        public Type RequestType { get; private set; }
    }
}
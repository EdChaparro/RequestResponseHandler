using System;

namespace IntrepidProducts.RequestResponseHandler.Handlers
{
    public class RequestHandlerNotRegistered : Exception
    {
        public RequestHandlerNotRegistered(Type requestType)
        {
            RequestType = requestType;
        }

        public Type RequestType { get; private set; }
    }
}
using System;

namespace IntrepidProducts.RequestResponseHandler.Handlers
{
    public class DuplicateRequestHandlerstException : Exception
    {
        public DuplicateRequestHandlerstException(Type requestHandler1Type, Type requestHandler2Type, Type requestType)
        {
            RequestHandler1Type = requestHandler1Type;
            RequestHandler2Type = requestHandler2Type;
            RequestType = requestType;
        }

        public Type RequestHandler1Type { get; private set; }
        public Type RequestHandler2Type { get; private set; }
        public Type RequestType { get; private set; }
    }
}
﻿using IntrepidProducts.RequestResponseHandler.Requests;
using IntrepidProducts.RequestResponseHandler.Responses;

namespace IntrepidProducts.RequestResponseHandler.Handlers
{
    public interface IRequestHandlerProcessor
    {
        ResponseBlock Process(RequestBlock requestBlock);
    }

    public class RequestHandlerProcess : IRequestHandlerProcessor
    {
        public RequestHandlerProcess(IRequestHandlerRegistry requestHandlerRegistry)
        {
            _requestHandlerRegistry = requestHandlerRegistry;
        }

        private readonly IRequestHandlerRegistry _requestHandlerRegistry;

        public ResponseBlock Process(RequestBlock requestBlock)
        {
            return new ResponseBlock(requestBlock); //TODO: Finish me
        }
    }
}
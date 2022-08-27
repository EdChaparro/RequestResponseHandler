using System;
using System.Collections.Generic;
using IntrepidProducts.RequestResponseHandler.Requests;
using IntrepidProducts.RequestResponseHandler.Responses;

namespace IntrepidProducts.RequestResponseHandler.Handlers
{
    public interface IRequestHandlerProcessor
    {
        ResponseBlock Process(RequestBlock requestBlock);
    }

    public abstract class RequestHandlerProcessorAbstract : IRequestHandlerProcessor
    {
        protected RequestHandlerProcessorAbstract(IRequestHandlerRegistry requestHandlerRegistry)
        {
            _requestHandlerRegistry = requestHandlerRegistry;
        }

        private readonly IRequestHandlerRegistry _requestHandlerRegistry;

        public ResponseBlock Process(RequestBlock requestBlock)
        {
            var responseBlock = new ResponseBlock(requestBlock);

            var requestHandlers = GetRequestHandlers(requestBlock.Requests);

            //TODO: Finish me

            return responseBlock;
        }

        private IEnumerable<IRequestHandler> GetRequestHandlers(IEnumerable<IRequest> requests)
        {
            var requestHandlers = new List<IRequestHandler>();

            foreach (var request in requests)
            {
                var requestHandlerType = _requestHandlerRegistry
                    .GetRequestHandlerTypeFor(request.GetType());

                if (requestHandlerType == null)
                {
                    throw new RequestHandlerNotRegistered(request.GetType());
                }

                var requestHandler = Resolve(requestHandlerType);

                if (requestHandler == null)
                {
                    throw new RequestHandlerNotResolvableException(request.GetType());
                }

                requestHandlers.Add(requestHandler);
            }

            return requestHandlers;
        }

        protected abstract IRequestHandler? Resolve(Type requestHandlerType);
    }
}
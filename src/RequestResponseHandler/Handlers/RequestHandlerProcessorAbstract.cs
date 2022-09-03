﻿using System;
using System.Collections.Generic;
using System.Linq;
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

            //TODO: Implement parallel execution

            var responses = ExecuteSerially(requestHandlers);

            responseBlock.Add(responses.ToArray());

            return responseBlock;
        }

        private IEnumerable<IResponse> ExecuteSerially
            (IEnumerable<(IRequest request, IRequestHandler requestHandler)> requestHandlers)
        {
            var responses = new List<IResponse>();

            foreach (var rh in requestHandlers)
            {
                var response = rh.requestHandler.Handle(rh.request);

                responses.Add(response);
            }

            return responses;
        }

        private IEnumerable<(IRequest request, IRequestHandler requestHandler)>
            GetRequestHandlers(IEnumerable<IRequest> requests)
        {
            var requestHandlers = new List<(IRequest request, IRequestHandler requestHandler)>();

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

                requestHandlers.Add((request, requestHandler));
            }

            return requestHandlers;
        }

        protected abstract IRequestHandler? Resolve(Type requestHandlerType);
    }
}
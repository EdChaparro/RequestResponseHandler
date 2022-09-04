using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            switch (requestBlock.ExecutionStrategy)
            {
                case ExecutionStrategy.Sequential:
                    responseBlock.Add(ExecuteSerially(requestHandlers).ToArray());
                    break;

                case ExecutionStrategy.Parallel:
                    responseBlock.Add(ExecuteInParallel(requestHandlers).Result.ToArray());
                    break;
                default:
                    throw new ArgumentException($"Unknown Execution Strategy, {requestBlock.ExecutionStrategy}");
            }

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

        private async Task<IEnumerable<IResponse>> ExecuteInParallel
            (IEnumerable<(IRequest request, IRequestHandler requestHandler)> requestHandlers)
        {
            var responses = new List<IResponse>();

            var tasks = new List<Task<IResponse>>();

            foreach (var rh in requestHandlers)
            {
                var request = rh.request;
                tasks.Add(rh.requestHandler.HandleAsync(request));
            }

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                var response = task.Result;
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
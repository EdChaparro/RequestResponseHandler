using System;
using IntrepidProducts.RequestResponseHandler.Requests;
using IntrepidProducts.RequestResponseHandler.Responses;

namespace IntrepidProducts.RequestResponseHandler.Handlers
{
    public interface IRequestHandler
    {
        IResponse Handle(IRequest request);
    }

    public interface IRequestHandler<in TRequest, out TResponse> : IRequestHandler
        where TRequest : class, IRequest
        where TResponse : class, IResponse
    {
        TResponse Handle(TRequest request);
    }

    public abstract class RequestHandlerAbstract<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse>
        where TRequest : class, IRequest
        where TResponse : class, IResponse
    {
        public IResponse Handle(IRequest request)
        {
            var concreteRequest = request as TRequest;

            if (concreteRequest == null)
            {
                throw new ArgumentException
                    ($"Invalid request passed to Handle function, {request}");
            }

            return Handle(concreteRequest);
        }

        public TResponse Handle(TRequest request)
        {
            try
            {
                BeforeHandle(request);
                var response = DoHandle(request);
                OnSuccessfulCompletion(request, response);
                return response;
            }
            catch (Exception e)
            {
                return OnFailure(request, e);
            }
        }

        protected virtual void BeforeHandle(TRequest request)
        {}

        protected abstract TResponse DoHandle(TRequest request);

        public virtual void OnSuccessfulCompletion(TRequest request, TResponse response)
        {}

        public virtual TResponse OnFailure(TRequest request, Exception? e)
        {
            var errorId = e == null ? "Unknown" : e.GetType().Name;
            var message = e == null ? "Unknown Error" : e.Message;

            var response = Activator.CreateInstance
                    (typeof(TResponse), request, new ErrorInfo(errorId, message))
                as TResponse;

            if (response == null)   //This should never happen
            {
                throw new InvalidOperationException
                    ($"Unable to instantiate Response for type {typeof(TResponse).FullName}");
            }

            return response;
        }
    }
}
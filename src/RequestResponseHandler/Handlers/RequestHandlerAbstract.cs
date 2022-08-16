using System;
using IntrepidProducts.RequestResponseHandler.Requests;
using IntrepidProducts.RequestResponseHandler.Responses;

namespace IntrepidProducts.RequestResponseHandler.Handlers
{
    public interface IRequestHandler<in TRequest, out TResponse>
        where TRequest : class, IRequest
        where TResponse : class, IResponse, new()
    {
        TResponse Handle(TRequest request);
    }

    public abstract class RequestHandlerAbstract<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : class, IRequest
        where TResponse : class, IResponse, new()
    {
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

            return new TResponse
            {
                ErrorInfo = new ErrorInfo(errorId, message)
            };
        }
    }
}
using IntrepidProducts.RequestResponse.Requests;
using IntrepidProducts.RequestResponse.Responses;
using System;
using System.Threading.Tasks;

namespace IntrepidProducts.RequestResponseHandler.Handlers
{
    public interface IRequestHandler
    {
        IResponse Handle(IRequest request);
        Task<IResponse> HandleAsync(IRequest request);
    }

    public interface IRequestHandler<in TRequest, TResponse> : IRequestHandler
        where TRequest : class, IRequest
        where TResponse : class, IResponse
    {
        TResponse Handle(TRequest request);
        Task<TResponse> HandleAsync(TRequest request);
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

        public async Task<IResponse> HandleAsync(IRequest request)
        {
            return await Task.Run(() => Handle(request));
        }

        public async Task<TResponse> HandleAsync(TRequest request)
        {
            return await Task.Run(() => Handle(request));
        }

        public TResponse Handle(TRequest request)
        {
            try
            {
                request.StartUtcTime = DateTime.UtcNow;

                BeforeHandle(request);
                var response = DoHandle(request);
                OnSuccessfulCompletion(request, response);

                response.CompletedUtcTime = DateTime.UtcNow;
                return response;
            }
            catch (Exception e)
            {
                return OnFailure(request, e);
            }
        }

        protected virtual void BeforeHandle(TRequest request)
        { }

        protected abstract TResponse DoHandle(TRequest request);

        public virtual void OnSuccessfulCompletion(TRequest request, TResponse response)
        { }

        public virtual TResponse OnFailure(TRequest request, Exception? e)
        {
            var response = Activator.CreateInstance(typeof(TResponse), request)
                as TResponse;

            if (response == null)   //This should never happen
            {
                throw new InvalidOperationException
                    ($"Unable to instantiate Response for type {typeof(TResponse).FullName}");
            }

            var errorId = e == null ? "Unknown" : e.GetType().Name;
            var message = e == null ? "Unknown Error" : e.Message;

            response.ErrorInfo = new ErrorInfo(errorId, message);

            return response;
        }
    }
}
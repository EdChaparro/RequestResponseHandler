using System;
using IntrepidProducts.IocContainer;

namespace IntrepidProducts.RequestResponseHandler.Handlers
{
    public class RequestHandlerProcessor : RequestHandlerProcessorAbstract
    {
        public RequestHandlerProcessor
            (IRequestHandlerRegistry requestHandlerRegistry, IIocContainer iocContainer)
            : base(requestHandlerRegistry)
        {
            _iocContainer = iocContainer;
        }

        private readonly IIocContainer _iocContainer;

        protected override IRequestHandler? Resolve(Type requestHandlerType)
        {
            throw new NotImplementedException();
        }
    }
}
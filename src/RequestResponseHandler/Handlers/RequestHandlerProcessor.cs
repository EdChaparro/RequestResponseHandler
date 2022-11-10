using IntrepidProducts.IocContainer;
using System;

namespace IntrepidProducts.RequestResponseHandler.Handlers
{
    public class RequestHandlerProcessor : AbstractRequestHandlerProcessor
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
            return _iocContainer.Resolve<IRequestHandler>(requestHandlerType.FullName);
        }
    }
}
using IntrepidProducts.IoC.MicrosoftStrategy;
using IntrepidProducts.IocContainer;
using IntrepidProducts.RequestResponseHandler;
using IntrepidProducts.RequestResponseHandler.Handlers;

namespace IntrepidProducts.RequestHandlerTestObjects
{
    public class Bootstrapper : BootstrapperAbstract
    {
        public Bootstrapper() : base(new MicrosoftStrategy())
        {
            _requestHandlerRegistry = new RequestHandlerRegistry();
        }

        private readonly IRequestHandlerRegistry _requestHandlerRegistry;

        protected override void ConfigIoC(IIocContainer iocContainer)
        {
            RegisterRequestHandlers();
            iocContainer.RegisterInstance<IRequestHandlerRegistry>(_requestHandlerRegistry);
            iocContainer.RegisterSingleton
                (typeof(IRequestHandlerProcessor), typeof(RequestHandlerProcessor));
            iocContainer.RegisterInstance(typeof(IIocContainer), IocContainer);
        }

        private void RegisterRequestHandlers()
        {
            var requestHandlerInterfaceType = typeof(IRequestHandler);

            _requestHandlerRegistry.RequestHandlerFoundEvent += (object sender, RequestHandlerFoundEvent e) =>
            {
                IocContainer.RegisterTransient
                    (e.RequestHandlerType.FullName, requestHandlerInterfaceType, e.RequestHandlerType);
            };

            _requestHandlerRegistry.Register(GetType().Assembly);   //Register all Request Handlers in this Assembly
        }
    }
}
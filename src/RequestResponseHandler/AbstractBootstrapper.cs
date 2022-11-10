using IntrepidProducts.IocContainer;

namespace IntrepidProducts.RequestResponseHandler
{
    public interface IBootStrapper
    {
        void Bootstrap();
        IIocContainer IocContainer { get; }
    }

    public abstract class AbstractBootstrapper : IBootStrapper
    {
        protected AbstractBootstrapper(IIocContainer iocContainer)
        {
            IocContainer = iocContainer;
        }
        public void Bootstrap()
        {
            Init();
            ConfigIoC(IocContainer);
        }
        protected virtual void Init()
        { }

        public IIocContainer IocContainer { get; }

        protected abstract void ConfigIoC(IIocContainer iocContainer);

    };
}
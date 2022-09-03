using IntrepidProducts.IocContainer;
using IntrepidProducts.RequestHandlerTestObjects;
using IntrepidProducts.RequestResponseHandler.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.RequestResponseHandlerTest
{
    // Testing concrete Bootstrapper exercises abstract code
    [TestClass]
    public class BootstrapperTest
    {
        [TestMethod]
        public void ShouldRegisterDependencies()
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Bootstrap();

            var iocContainer = bootstrapper.IocContainer;

            //Note: Comparing Type to RuntimeTypes
            Assert.AreEqual
                (typeof(RequestHandler01).FullName,
                    iocContainer.Resolve<IRequestHandler>(typeof(RequestHandler01).FullName).GetType().FullName);

            Assert.AreEqual
            (typeof(RequestHandler02).FullName,
                iocContainer.Resolve<IRequestHandler>(typeof(RequestHandler02).FullName).GetType().FullName);

            Assert.IsNotNull(iocContainer.Resolve<IRequestHandlerRegistry>());
            Assert.IsNotNull(iocContainer.Resolve<IRequestHandlerProcessor>());
            Assert.IsNotNull(iocContainer.Resolve<IIocContainer>());
        }
    }
}
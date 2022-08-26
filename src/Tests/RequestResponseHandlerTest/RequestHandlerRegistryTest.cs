using System;
using IntrepidProducts.RequestResponseHandler.Handlers;
using IntrepidProducts.RequestResponseHandlerTest.RequestHandlerTestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.RequestResponseHandlerTest
{
    [TestClass]
    public class RequestHandlerRegistryTest
    {
        #region Registration
        [TestMethod]
        public void ShouldRegisterRequestHandlerByType()
        {
            var registry = new RequestHandlerRegistry();

            Assert.AreEqual(1, registry.Register(typeof(RequestHandler01)));
            Assert.AreEqual(1, registry.RequestHandlerCount);
        }

        [TestMethod]
        public void ShouldRegisterMultipleRequestHandlers()
        {
            var registry = new RequestHandlerRegistry();

            Assert.AreEqual(1, registry.Register(typeof(RequestHandler01)));
            Assert.AreEqual(1, registry.Register(typeof(RequestHandler02)));
            Assert.AreEqual(2, registry.RequestHandlerCount);
        }

        [TestMethod]
        public void ShouldRegisterRequestHandlerByAssembly()
        {
            var registry = new RequestHandlerRegistry();

            Assert.AreEqual(2, registry.Register(GetType().Assembly));
            Assert.AreEqual(2, registry.RequestHandlerCount);
        }


        [TestMethod]
        public void ShouldIgnoreWhenNotRequestHandler()
        {
            var registry = new RequestHandlerRegistry();

            Assert.AreEqual(0, registry.Register(typeof(object)));
            Assert.AreEqual(0, registry.RequestHandlerCount);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateRequestHandlerstException))]
        public void ShouldThrowExceptionWhenDuplicateRegistration()
        {
            var registry = new RequestHandlerRegistry();

            registry.Register(typeof(RequestHandler01));
            registry.Register(typeof(RequestHandler01));
        }

        #endregion

        #region Find Request Handlers
        [TestMethod]
        public void ShouldReturnRequestHandlerTypeLinkedToRequest()
        {
            var registry = new RequestHandlerRegistry();

            Assert.AreEqual(1, registry.Register(typeof(RequestHandler01)));

            Assert.AreEqual(typeof(RequestHandler01),
                registry.GetRequestHandlerTypeFor(typeof(Request01)));
        }

        [TestMethod]
        public void ShouldReturnNullWhenRequestHandlerIsUnregistered()
        {
            var registry = new RequestHandlerRegistry();

            Assert.IsNull(registry.GetRequestHandlerTypeFor(typeof(Request01)));
        }
        #endregion
    }
}
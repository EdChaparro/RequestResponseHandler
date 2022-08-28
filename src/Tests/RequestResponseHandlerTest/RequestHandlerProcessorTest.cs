using System;
using System.Collections.Generic;
using IntrepidProducts.RequestHandlerTestObjects;
using IntrepidProducts.RequestResponseHandler.Handlers;
using IntrepidProducts.RequestResponseHandler.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IntrepidProducts.RequestResponseHandlerTest
{
    public class MockRequestHandlerProcessor : RequestHandlerProcessorAbstract
    {
        public MockRequestHandlerProcessor
            (IRequestHandlerRegistry requestHandlerRegistry,
                IRequestHandler? mockHandler)
            : base(requestHandlerRegistry)
        {
            MockRequestHandler = mockHandler;
        }

        public IList<Type> RequestHandlerToResolve { get; } = new List<Type>();

        protected override IRequestHandler? Resolve(Type requestHandlerType)
        {
            RequestHandlerToResolve.Add(requestHandlerType);
            return MockRequestHandler;
        }

        private IRequestHandler? MockRequestHandler { get; }
    }

    [TestClass]
    public class RequestHandlerProcessorTest
    {
        [TestMethod]
        public void ShouldTryToResolveRegisteredRequestHandlers()
        {
            var mockRegistry = new Mock<IRequestHandlerRegistry>();
            mockRegistry.Setup
                    (x => x.GetRequestHandlerTypeFor(It.IsAny<Type>()))
                .Returns(typeof(RequestHandler01));

            var mockProcessor = new MockRequestHandlerProcessor
                (mockRegistry.Object, new RequestHandler01());

            var rb = new RequestBlock();
            rb.Add(new Request01());

            mockProcessor.Process(rb);

            mockRegistry.Verify
            (x =>
                x.GetRequestHandlerTypeFor(It.IsAny<Type>()), Times.Once);
        }

        [TestMethod]
        public void ShouldTryToResolveAllRequestHandlersInRequestBlock()
        {
            var mockRegistry = new Mock<IRequestHandlerRegistry>();
            mockRegistry.Setup
                    (x => x.GetRequestHandlerTypeFor(It.IsAny<Type>()))
                .Returns(typeof(RequestHandler01));

            var mockProcessor = new MockRequestHandlerProcessor
                (mockRegistry.Object, new RequestHandler01());

            var rb = new RequestBlock();
            rb.Add(new Request01());
            rb.Add(new Request02());

            mockProcessor.Process(rb);

            mockRegistry.Verify
            (x =>
                x.GetRequestHandlerTypeFor(It.IsAny<Type>()), Times.Exactly(2));
        }

        [TestMethod]
        [ExpectedException(typeof(RequestHandlerNotResolvableException))]
        public void ShouldThrowExceptionWhenRequestHandleCannotBeResolved()
        {
            var mockRegistry = new Mock<IRequestHandlerRegistry>();
            mockRegistry.Setup
                    (x => x.GetRequestHandlerTypeFor(It.IsAny<Type>()))
                .Returns(typeof(RequestHandler01));

            var mockProcessor = new MockRequestHandlerProcessor
                (mockRegistry.Object, null);

            var rb = new RequestBlock();
            rb.Add(new Request01());

            mockProcessor.Process(rb);
        }

        [TestMethod]
        [ExpectedException(typeof(RequestHandlerNotRegistered))]
        public void ShouldThrowExceptionWhenRequestHandleNotRegistered()
        {
            var mockRegistry = new Mock<IRequestHandlerRegistry>();
            mockRegistry.Setup
                    (x => x.GetRequestHandlerTypeFor(It.IsAny<Type>()))
                .Returns<IRequestHandler>(null);

            var mockProcessor = new MockRequestHandlerProcessor(mockRegistry.Object, null);

            var rb = new RequestBlock();
            rb.Add(new Request01());

            mockProcessor.Process(rb);
        }
    }
}
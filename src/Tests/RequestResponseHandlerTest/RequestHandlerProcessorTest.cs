using System.Linq;
using IntrepidProducts.IocContainer;
using IntrepidProducts.RequestHandlerTestObjects;
using IntrepidProducts.RequestResponseHandler.Handlers;
using IntrepidProducts.RequestResponseHandler.Requests;
using IntrepidProducts.RequestResponseHandler.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IntrepidProducts.RequestResponseHandlerTest
{
    [TestClass]
    public class RequestHandlerProcessorTest
    {
        [TestMethod]
        public void ShouldExecuteRequestHandlerLinkedToRequestType()
        {
            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;
            var processor = iocContainer.Resolve<IRequestHandlerProcessor>();

            var rb = new RequestBlock();
            rb.Add(new Request01());

            var responseBlock = processor.Process(rb);
            Assert.IsNotNull(responseBlock);

            var responses = responseBlock.Responses.ToList();
            Assert.AreEqual(1, responses.Count);
            var response = responses[0] as RequestHandlerTypeResponse;
            Assert.IsNotNull(response);

            Assert.AreEqual(rb.Requests.First(), response.OriginalRequest);
            Assert.AreEqual(typeof(RequestHandler01), response.RequestHandlerType);
            Assert.IsTrue(response.IsSuccessful);
        }

        [TestMethod]
        public void ShouldExecuteMultipleRequestHandlers()
        {
            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;
            var processor = iocContainer.Resolve<IRequestHandlerProcessor>();

            var rb = new RequestBlock();
            rb.Add(new Request01());
            rb.Add(new Request02());

            var responseBlock = processor.Process(rb);
            Assert.IsNotNull(responseBlock);

            var responses = responseBlock.Responses.ToList();
            Assert.AreEqual(2, responses.Count);
            var response1 = responses[0] as RequestHandlerTypeResponse;
            var response2 = responses[1] as RequestHandlerTypeResponse;
            Assert.IsNotNull(response1);
            Assert.IsNotNull(response2);

            Assert.AreEqual(rb.Requests.ToList()[0], response1.OriginalRequest);
            Assert.AreEqual(rb.Requests.ToList()[1], response2.OriginalRequest);
            Assert.AreEqual(typeof(RequestHandler01), response1.RequestHandlerType);
            Assert.AreEqual(typeof(RequestHandler02), response2.RequestHandlerType);

            Assert.IsTrue(response1.IsSuccessful);
            Assert.IsTrue(response2.IsSuccessful);
        }

        [TestMethod]
        [ExpectedException(typeof(RequestHandlerNotResolvableException))]
        public void ShouldThrowExceptionWhenRequestHandleCannotBeResolved()
        {
            var mockIocContainer = new Mock<IIocContainer>();

            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;

            var registry = iocContainer.Resolve<IRequestHandlerRegistry>();

            var processor = new RequestHandlerProcessor(registry, mockIocContainer.Object);

            var rb = new RequestBlock();
            rb.Add(new Request01());

            processor.Process(rb);
        }

        [TestMethod]
        [ExpectedException(typeof(RequestHandlerNotRegistered))]
        public void ShouldThrowExceptionWhenRequestHandleNotRegistered()
        {
            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;
            var processor = iocContainer.Resolve<IRequestHandlerProcessor>();

            var rb = new RequestBlock();
            rb.Add(new RequestWithNoRequestHandler());

            processor.Process(rb);
        }

        [TestMethod]
        public void ShouldReportWhenRequestHandlerThrowsException()
        {
            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;
            var processor = iocContainer.Resolve<IRequestHandlerProcessor>();

            var rb = new RequestBlock();
            rb.Add(new NumericOperationRequest()
            {
                Number1 = 4,
                Number2 = 0,                                    //Divide by zero should...
                NumberOperation = (n1, n2) => n1 / n2   // throw exception
            });

            var responseBlock = processor.Process(rb);
            Assert.IsNotNull(responseBlock);

            var responses = responseBlock.Responses.ToList();
            Assert.AreEqual(1, responses.Count);
            var response = responses[0] as NumericOperationResponse;
            Assert.IsNotNull(response);

            Assert.IsNotNull(response.ErrorInfo);
            Assert.IsFalse(response.IsSuccessful);

            var errorInfo = response.ErrorInfo;

            Assert.AreEqual("DivideByZeroException", errorInfo.ErrorId);
            Assert.AreEqual("Attempted to divide by zero.", errorInfo.Message);
        }
    }
}
using System.Linq;
using IntrepidProducts.IocContainer;
using IntrepidProducts.RequestHandlerTestObjects;
using IntrepidProducts.RequestHandlerTestObjects.Requests;
using IntrepidProducts.RequestHandlerTestObjects.Responses;
using IntrepidProducts.RequestResponseHandler.Handlers;
using IntrepidProducts.RequestResponseHandler.Handlers.Exceptions;
using IntrepidProducts.RequestResponseHandler.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IntrepidProducts.RequestResponseHandlerTest
{
    [TestClass]
    public class RequestHandlerProcessorTest
    {
        #region ShouldExecuteRequestHandlerLinkedToRequestType
        [TestMethod]
        public void ShouldExecuteRequestHandlerLinkedToRequestType()
        {
            ExecuteRequestHandlerLinkedToRequestType(ExecutionStrategy.Sequential);
            ExecuteRequestHandlerLinkedToRequestType(ExecutionStrategy.Parallel);
        }

        private void ExecuteRequestHandlerLinkedToRequestType(ExecutionStrategy executionStrategy)
        {
            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;
            var processor = iocContainer.Resolve<IRequestHandlerProcessor>();

            var rb = new RequestBlock {ExecutionStrategy = executionStrategy};
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

            Assert.IsTrue(rb.Requests.First().StartUtcTime < response.CompletedUtcTime);
        }
        #endregion

        #region ShouldExecuteMultipleRequestHandlers
        [TestMethod]
        public void ShouldExecuteMultipleRequestHandlers()
        {
            ExecuteMultipleRequestHandlers(ExecutionStrategy.Sequential);
            ExecuteMultipleRequestHandlers(ExecutionStrategy.Parallel);
        }

        private void ExecuteMultipleRequestHandlers(ExecutionStrategy executionStrategy)
        {
            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;
            var processor = iocContainer.Resolve<IRequestHandlerProcessor>();

            var rb = new RequestBlock {ExecutionStrategy = executionStrategy};
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
        #endregion

        #region ShouldReportWhenRequestHandlerThrowsException
        [TestMethod]
        public void ShouldReportWhenRequestHandlerThrowsException()
        {
            ReportWhenRequestHandlerThrowsException(ExecutionStrategy.Sequential);
            ReportWhenRequestHandlerThrowsException(ExecutionStrategy.Parallel);
        }

        private void ReportWhenRequestHandlerThrowsException(ExecutionStrategy executionStrategy)
        {
            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;
            var processor = iocContainer.Resolve<IRequestHandlerProcessor>();

            var rb = new RequestBlock {ExecutionStrategy = executionStrategy};
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
        #endregion

        [TestMethod]
        public void ShouldExecuteRequestHandlersInParallel()
        {
            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;
            var processor = iocContainer.Resolve<IRequestHandlerProcessor>();

            var rb = new RequestBlock { ExecutionStrategy = ExecutionStrategy.Parallel };
            rb.Add(new CalculateFibonacciSequenceRequest { NumberOfElements = 5000000 });
            rb.Add(new Request01());
            rb.Add(new Request01());
            rb.Add(new Request01());
            rb.Add(new Request01());
            rb.Add(new Request01());

            var responseBlock = processor.Process(rb);
            Assert.IsNotNull(responseBlock);

            Assert.AreEqual(6, responseBlock.Responses.Count());

            var responses = responseBlock.Responses
                .OrderByDescending(x => x.CompletedUtcTime)
                .ToList();

            var lastResponseToFinish = responses[0];    //Should be more CPU intensive request handler
            Assert.AreEqual(typeof(CalculateFibonacciSequenceResponse), lastResponseToFinish.GetType());
        }

        [TestMethod]
        public void ShouldExecuteRequestHandlersSeriallyl()
        {
            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;
            var processor = iocContainer.Resolve<IRequestHandlerProcessor>();

            var rb = new RequestBlock { ExecutionStrategy = ExecutionStrategy.Sequential };
            rb.Add(new CalculateFibonacciSequenceRequest { NumberOfElements = 5000000 });
            rb.Add(new Request01());
            rb.Add(new Request01());
            rb.Add(new Request01());
            rb.Add(new Request01());
            rb.Add(new Request01());

            var responseBlock = processor.Process(rb);
            Assert.IsNotNull(responseBlock);

            Assert.AreEqual(6, responseBlock.Responses.Count());

            var responses = responseBlock.Responses
                .OrderBy(x => x.CompletedUtcTime)
                .ToList();

            var firstResponseToFinish = responses[0];    //More CPU intensive request handler finished first due to FIFO
            Assert.AreEqual(typeof(CalculateFibonacciSequenceResponse), firstResponseToFinish.GetType());
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
        [ExpectedException(typeof(RequestHandlerNotRegisteredException))]
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
    }
}
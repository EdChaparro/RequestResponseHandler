using IntrepidProducts.RequestHandlerTestObjects;
using IntrepidProducts.RequestHandlerTestObjects.Requests;
using IntrepidProducts.RequestHandlerTestObjects.Responses;
using IntrepidProducts.RequestResponse.Requests;
using IntrepidProducts.RequestResponse.Responses;
using IntrepidProducts.RequestResponseHandler.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace IntrepidProducts.RequestResponseHandlerTest.Responses
{
    [TestClass]
    public class ResponseBlockTest
    {
        [TestMethod]
        public void ShouldOfferResponseByRequestId()
        {
            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;
            var processor = iocContainer.Resolve<IRequestHandlerProcessor>();

            var request1 = new Request01();
            var request2 = new CalculateFibonacciSequenceRequest();
            var request3 = new NumericOperationRequest();

            var rb = new RequestBlock();
            rb.Add(request1);
            rb.Add(request2);
            rb.Add(request3);

            var responseBlock = processor.Process(rb);
            Assert.IsNotNull(responseBlock);

            var responses = responseBlock.Responses.ToList();
            Assert.AreEqual(3, responses.Count);

            var response1 = responseBlock.GetResponseByRequestId<RequestHandlerTypeResponse>(request1.Id);
            var response2 = responseBlock.GetResponseByRequestId<CalculateFibonacciSequenceResponse>(request2.Id);
            var response3 = responseBlock.GetResponseByRequestId<NumericOperationResponse>(request3.Id);

            Assert.IsNotNull(response1);
            Assert.IsNotNull(response2);
            Assert.IsNotNull(response3);
        }

        [TestMethod]
        public void ShouldReturnNullWhenResponseByRequestIdTypeIsWrong()
        {
            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;
            var processor = iocContainer.Resolve<IRequestHandlerProcessor>();

            var request1 = new Request01();
            var request2 = new CalculateFibonacciSequenceRequest();
            var request3 = new NumericOperationRequest();

            var rb = new RequestBlock();
            rb.Add(request1);
            rb.Add(request2);
            rb.Add(request3);

            var responseBlock = processor.Process(rb);
            Assert.IsNotNull(responseBlock);

            var responses = responseBlock.Responses.ToList();
            Assert.AreEqual(3, responses.Count);

            var response1 = responseBlock.GetResponseByRequestId<ResponseWithNoRequestHandler>(request1.Id);
            var response2 = responseBlock.GetResponseByRequestId<ResponseWithNoRequestHandler>(request2.Id);
            var response3 = responseBlock.GetResponseByRequestId<ResponseWithNoRequestHandler>(request3.Id);

            Assert.IsNull(response1);
            Assert.IsNull(response2);
            Assert.IsNull(response3);
        }

        [TestMethod]
        public void ShouldReturnNullWhenResponseByRequestIdIsWrong()
        {
            var bootStrapper = new Bootstrapper();
            bootStrapper.Bootstrap();
            var iocContainer = bootStrapper.IocContainer;
            var processor = iocContainer.Resolve<IRequestHandlerProcessor>();

            var request1 = new Request01();
            var request2 = new CalculateFibonacciSequenceRequest();
            var request3 = new NumericOperationRequest();

            var rb = new RequestBlock();
            rb.Add(request1);
            rb.Add(request2);
            rb.Add(request3);

            var responseBlock = processor.Process(rb);
            Assert.IsNotNull(responseBlock);

            var responses = responseBlock.Responses.ToList();
            Assert.AreEqual(3, responses.Count);

            var response1 = responseBlock.GetResponseByRequestId<RequestHandlerTypeResponse>(new Guid());
            var response2 = responseBlock.GetResponseByRequestId<CalculateFibonacciSequenceResponse>(new Guid());
            var response3 = responseBlock.GetResponseByRequestId<NumericOperationResponse>(new Guid());

            Assert.IsNull(response1);
            Assert.IsNull(response2);
            Assert.IsNull(response3);
        }

        [TestMethod]
        public void ShouldIndicateWhetherResponsesHaveErrors()
        {
            var response1 = new RequestHandlerTypeResponse(new Request01());
            var response2 = new RequestHandlerTypeResponse(new Request01());
            var responseWithError
                = new RequestHandlerTypeResponse(new Request01(),
                    new ErrorInfo("foo", "bar"));

            var rb = new ResponseBlock(new RequestBlock());

            rb.Add(response1);
            rb.Add(response2);
            Assert.IsTrue(rb.IsSuccessful);
            Assert.IsFalse(rb.HasErrors);

            rb.Add(responseWithError);
            Assert.IsFalse(rb.IsSuccessful);
            Assert.IsTrue(rb.HasErrors);
        }
    }
}
using IntrepidProducts.RequestHandlerTestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.RequestResponseHandlerTest
{
    [TestClass]
    public class RequestHandlerTest
    {
        [TestMethod]
        public void ShouldHandleExceptions()
        {
            var request = new NumericOperationRequest
            {
                NumberOperation = (n1, n2) => n1 / n2,
                Number1 = 10,
                Number2 = 0
            };

            var rh = new NumericOperationRequestHandler();

            var response = rh.Handle(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccessful);

            var errorInfo = response.ErrorInfo;
            Assert.IsNotNull(errorInfo);
            Assert.AreEqual("DivideByZeroException", errorInfo.ErrorId);
            Assert.AreEqual("Attempted to divide by zero.", errorInfo.Message);
        }
    }
}
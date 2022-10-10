using System;
using IntrepidProducts.RequestHandlerTestObjects;
using IntrepidProducts.RequestHandlerTestObjects.Requests;
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

        #region Validation

        [TestMethod]
        public void ShouldAbortOnValidationWhenRequested()
        {
            var request = new CalculateFibonacciSequenceRequest
                {NumberOfElements = -1 };  //Out of range

            var rh = new CalculateFibonacciSequenceRequestHandler
            {
                AbortOnValidationError = true
            };

            var response = rh.Handle(request);

            Assert.IsFalse(response.IsSuccessful);
            var errorInfo = response.ErrorInfo;
            Assert.IsNotNull(errorInfo);
            Assert.AreEqual("ArgumentException", errorInfo.ErrorId);
            Assert.AreEqual("The field NumberOfElements must be between 1 and 10000000.", errorInfo.Message);

            rh.AbortOnValidationError = false;
            var response2 = rh.Handle(request);

            Assert.IsTrue(response2.IsSuccessful);
        }
        #endregion
    }
}
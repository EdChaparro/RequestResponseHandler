using IntrepidProducts.RequestHandlerTestObjects.Requests;
using IntrepidProducts.RequestResponse.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntrepidProducts.RequestResponseHandlerTest.Responses
{
    [TestClass]
    public class OperationResponseTest
    {
        [TestMethod]
        public void ShouldDefaultMessageToOperationalEnum()
        {
            var response = new OperationResponse(new Request01());

            Assert.AreEqual(response.Result,
                OperationResult.OperationalError);

            Assert.AreEqual("OperationalError", response.Message);

            const string CUSTOM_MESSAGE = "Something went wrong";

            response.Message = CUSTOM_MESSAGE;
            Assert.AreEqual(CUSTOM_MESSAGE, response.Message);

            response.Message = null; //Will cause message to revert to default
            Assert.AreEqual("OperationalError", response.Message);
        }
    }
}
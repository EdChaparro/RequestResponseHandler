using IntrepidProducts.Common;
using IntrepidProducts.RequestResponse.Requests;

namespace IntrepidProducts.RequestResponse.Responses
{
    public interface IOperationResponse : IResponse
    {
        OperationResult Result { get; set; }
        string? Message { get; set; }
    }

    public enum OperationResult
    {
        Successful,
        NotFound,
        ValidationFail,
        OperationalError,
        SecurityError
    }

    public class OperationResponse : AbstractResponse, IOperationResponse
    {
        public OperationResponse(IRequest originalRequest, ErrorInfo? errorInfo = null)
            : base(originalRequest, errorInfo)
        {
            Result = OperationResult.OperationalError;
        }

        public OperationResult Result { get; set; }

        private string? _message;
        public string? Message
        {
            get
            {
                if (_message != null)
                {
                    return _message;
                }

                return EnumHelper.GetDescription(Result);
            }

            set => _message = value;
        }
    }
}
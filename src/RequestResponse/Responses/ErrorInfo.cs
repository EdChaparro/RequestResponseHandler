namespace IntrepidProducts.RequestResponse.Responses
{
    public class ErrorInfo
    {
        public ErrorInfo(string errorId, string message)
        {
            ErrorId = errorId;
            Message = message;
        }

        public string ErrorId { get; }
        public string Message { get; }
    }
}
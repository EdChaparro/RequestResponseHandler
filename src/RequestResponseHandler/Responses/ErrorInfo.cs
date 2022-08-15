namespace IntrepidProducts.RequestResponseHandler.Responses
{
    public class ErrorInfo
    {
        public ErrorInfo(string id, string message)
        {
            Id = id;
            Message = message;
        }

        public string Id { get; }
        public string Message { get; }
    }
}
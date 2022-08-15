using System.Collections.Generic;
using IntrepidProducts.RequestResponseHandler.Requests;

namespace IntrepidProducts.RequestResponseHandler.Responses
{
    public class ResponseBlock
    {
        public ResponseBlock(RequestBlock originalRequestBlock)
        {
            OriginalRequestBlock = originalRequestBlock;
        }
        public RequestBlock OriginalRequestBlock { get; }

        private readonly IList<IResponse> _responses = new List<IResponse>();
        public void Add(IResponse response)
        {
            _responses.Add(response);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<IResponse> Responses => _responses.ToList();

        public void Add(IResponse response)
        {
            _responses.Add(response);
        }
    }
}
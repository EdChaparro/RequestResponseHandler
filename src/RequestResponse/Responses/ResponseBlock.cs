using IntrepidProducts.RequestResponse.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntrepidProducts.RequestResponse.Responses
{
    public class ResponseBlock
    {
        public ResponseBlock(RequestBlock originalRequestBlock)
        {
            OriginalRequestBlock = originalRequestBlock;
        }

        public RequestBlock OriginalRequestBlock { get; }

        private readonly List<IResponse> _responses = new List<IResponse>();

        public IEnumerable<IResponse> Responses => _responses.ToList();

        public void Add(params IResponse[] responses)
        {
            _responses.AddRange(responses);
        }

        public T? GetResponseByRequestId<T>(Guid id) where T : class, IResponse
        {
            return Responses.FirstOrDefault(x => x.OriginalRequest.Id == id) as T;
        }

        public bool HasErrors => Responses.Any(x => !x.IsSuccessful);

        public bool IsSuccessful => Responses.All(x => x.IsSuccessful);
    }
}
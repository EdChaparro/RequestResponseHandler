using System;
using System.Collections.Generic;

namespace IntrepidProducts.RequestResponseHandler.Requests
{
    public enum ExecutionStrategy
    {
        Sequential = 0,
        Parallel = 1
    }

    public class RequestBlock
    {
        protected RequestBlock()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public ExecutionStrategy ExecutionStrategy { get; set; } = ExecutionStrategy.Sequential;

        private readonly IList<IRequest> _requests = new List<IRequest>();
        public void Add(IRequest request)
        {
            _requests.Add(request);
        }
    }
}
using System;

namespace IntrepidProducts.RequestResponseHandler.Requests
{
    public interface IRequest
    {
        public Guid Id { get; }
    }

    public abstract class RequestAbstract : IRequest
    {
        protected RequestAbstract()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
    }
}
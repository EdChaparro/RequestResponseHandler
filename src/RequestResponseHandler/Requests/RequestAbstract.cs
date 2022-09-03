using System;

namespace IntrepidProducts.RequestResponseHandler.Requests
{
    public interface IRequest
    {
        Guid Id { get; }
        DateTime StartUtcTime { get; set; }
    }

    public abstract class RequestAbstract : IRequest
    {
        protected RequestAbstract()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public DateTime StartUtcTime { get; set; }

        public override string ToString()
        {
            return $"{Id}, {GetType().Name}";
        }
    }
}
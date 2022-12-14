using System;

namespace IntrepidProducts.RequestResponse.Requests
{
    public interface IRequest
    {
        Guid Id { get; }
        DateTime StartUtcTime { get; set; }
    }

    public abstract class AbstractRequest : IRequest
    {
        protected AbstractRequest()
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
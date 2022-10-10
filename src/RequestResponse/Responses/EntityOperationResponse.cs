using System;
using IntrepidProducts.RequestResponse.Requests;

namespace IntrepidProducts.RequestResponse.Responses
{
    public class EntityOperationResponse : OperationResponse
    {
        public EntityOperationResponse(IRequest originalRequest, ErrorInfo? errorInfo = null)
            : base(originalRequest, errorInfo)
        { }

        public Guid EntityId { get; set; }
    }
}
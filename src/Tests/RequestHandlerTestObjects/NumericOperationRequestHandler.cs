using System;
using IntrepidProducts.RequestResponseHandler.Handlers;
using IntrepidProducts.RequestResponseHandler.Requests;
using IntrepidProducts.RequestResponseHandler.Responses;

namespace IntrepidProducts.RequestHandlerTestObjects
{
    public class NumericOperationRequest : RequestAbstract
    {
        public NumericOperationRequest(int nbr1, int  nbr2)
        {
            Number1 = nbr1;
            Number2 = nbr2;
            NumberOperation = (n1, n2) => n1 + n2;
        }

        public int Number1 { get; set; }
        public int Number2 { get; set; }

        public Func<int, int, long> NumberOperation { get; set; }
    }

    public class NumericOperationResponse : ResponseAbstract
    {
        public NumericOperationResponse(NumericOperationRequest request) : base(request)
        {}

        public long Answer { get; set; }
    }

    public class NumericOperationRequestHandler : RequestHandlerAbstract<NumericOperationRequest, NumericOperationResponse>
    {
        protected override NumericOperationResponse DoHandle(NumericOperationRequest request)
        {
            return new NumericOperationResponse(request)
            {
                Answer = request.NumberOperation(request.Number1, request.Number2)
            };
        }
    }
}
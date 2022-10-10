using IntrepidProducts.RequestResponse.Requests;
using IntrepidProducts.RequestResponse.Responses;
using IntrepidProducts.RequestResponseHandler.Handlers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;

namespace IntrepidProducts.RequestHandlerTestObjects.Requests
{
    public class CalculateFibonacciSequenceRequest : RequestAbstract
    {
        [Range(1,10000000)]
        public int NumberOfElements { get; set; } = 10;
    }

    public class CalculateFibonacciSequenceResponse : ResponseAbstract
    {
        public CalculateFibonacciSequenceResponse
            (CalculateFibonacciSequenceRequest request, ErrorInfo? errorInfo)
            : base(request, errorInfo)
        { }

        public IEnumerable<BigInteger> Answer { get; set; }
    }

    public class CalculateFibonacciSequenceRequestHandler
        : AbstractRequestHandler
            <CalculateFibonacciSequenceRequest, CalculateFibonacciSequenceResponse>
    {
        public bool AbortOnValidationError { get; set; }

        protected override CalculateFibonacciSequenceResponse DoHandle(CalculateFibonacciSequenceRequest request)
        {
            var results = IsValid(request, AbortOnValidationError);

            return new CalculateFibonacciSequenceResponse(request, null)
            {
                Answer = FibonacciSequence(request.NumberOfElements).ToList()
            };
        }

        private static IEnumerable<BigInteger> FibonacciSequence(int nbrOfElements = 10)
        {
            var previousNbr = 1;
            var currentNbr = 0; //Bootstrap sequence with zero

            for (int i = 0; i < nbrOfElements; i++)
            {
                yield return currentNbr;

                var nextNbr = previousNbr + currentNbr;
                previousNbr = currentNbr;
                currentNbr = nextNbr;
            }
        }
    }
}
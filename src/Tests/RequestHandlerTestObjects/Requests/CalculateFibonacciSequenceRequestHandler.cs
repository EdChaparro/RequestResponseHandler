using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using IntrepidProducts.RequestResponse.Requests;
using IntrepidProducts.RequestResponse.Responses;
using IntrepidProducts.RequestResponseHandler.Handlers;

namespace IntrepidProducts.RequestHandlerTestObjects.Requests
{
    public class CalculateFibonacciSequenceRequest : RequestAbstract
    {
        public int NumberOfElements { get; set; } = 10;
    }

    public class CalculateFibonacciSequenceResponse : ResponseAbstract
    {
        public CalculateFibonacciSequenceResponse(CalculateFibonacciSequenceRequest request) : base(request)
        { }

        public IEnumerable<BigInteger> Answer { get; set; }
    }

    public class CalculateFibonacciSequenceRequestHandler : RequestHandlerAbstract<CalculateFibonacciSequenceRequest, CalculateFibonacciSequenceResponse>
    {
        protected override CalculateFibonacciSequenceResponse DoHandle(CalculateFibonacciSequenceRequest request)
        {
            return new CalculateFibonacciSequenceResponse(request)
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
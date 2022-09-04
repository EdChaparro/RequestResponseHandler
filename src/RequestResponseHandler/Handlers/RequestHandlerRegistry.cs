using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IntrepidProducts.RequestResponseHandler.Handlers.Exceptions;

namespace IntrepidProducts.RequestResponseHandler.Handlers
{
    public class RequestHandlerFoundEvent : EventArgs
    {
        public RequestHandlerFoundEvent(Type requestType, Type requestHandlerType)
        {
            RequestType = requestType;
            RequestHandlerType = requestHandlerType;
        }

        public Type RequestType { get; }
        public Type RequestHandlerType { get; }
    }

    public interface IRequestHandlerRegistry
    {
        int Register(params Type[] requestHandlerTypes);
        int Register(params Assembly[] assemblies);
        Type? GetRequestHandlerTypeFor(Type requestType);

        event EventHandler<RequestHandlerFoundEvent> RequestHandlerFoundEvent;
    }

    public class RequestHandlerRegistry : IRequestHandlerRegistry
    {
        private readonly Dictionary<Type, Type> _requestHandlers = new Dictionary<Type, Type>();

        public int RequestHandlerCount => _requestHandlers.Count;

        public Type? GetRequestHandlerTypeFor(Type requestType)
        {
             _requestHandlers.TryGetValue(requestType, out var requestHandlerType);

            return requestHandlerType;
        }

        public event EventHandler<RequestHandlerFoundEvent>? RequestHandlerFoundEvent;

        private void OnRequestHandlerEventFound(RequestHandlerFoundEvent e)
        {
            RequestHandlerFoundEvent?.Invoke(this, e);
        }

        public int Register(params Type[] requestHandlerTypes)
        {
            var registerCount = 0;

            foreach (var requestHandlerType in requestHandlerTypes.Where(IsValid))
            {
                var requestType = GetRequestTypeFor(requestHandlerType);

                if (requestType == null)
                {
                    continue;
                }

                if (_requestHandlers.ContainsKey(requestType))
                {
                    throw new DuplicateRequestHandlerstException
                    (_requestHandlers[requestType],
                        requestHandlerType,
                        requestType);
                }

                _requestHandlers[requestType] = requestHandlerType;
                registerCount++;
                OnRequestHandlerEventFound(new RequestHandlerFoundEvent(requestType, requestHandlerType));
            }

            return registerCount;
        }

        public int Register(params Assembly[] assemblies)
        {
            var requestHandlerTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                requestHandlerTypes.AddRange(FindRequestHandlers(assembly));
            }

            int registerCount = 0;

            foreach (var requestHandlerType in requestHandlerTypes)
            {
                registerCount += Register(requestHandlerType);
            }

            return registerCount;
        }

        private static IEnumerable<Type> FindRequestHandlers(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(IsValid);
        }

        private static Type? GetRequestTypeFor(Type requestHandlerType)
        {
            var rhInterface = requestHandlerType
                .GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType &&
                                     x.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

            return rhInterface?.GetGenericArguments().First();
        }

        private static bool IsValid(Type requestHandlerType)
        {
            return (!requestHandlerType.IsAbstract) &&
                   (!requestHandlerType.IsGenericType) &&
                   (requestHandlerType.GetInterfaces()
                       .Any(x => x.IsGenericType &&
                                 x.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));
        }
    }
}
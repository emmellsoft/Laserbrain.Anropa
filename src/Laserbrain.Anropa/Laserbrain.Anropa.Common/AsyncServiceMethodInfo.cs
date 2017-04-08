using System;

namespace Laserbrain.Anropa
{
    public class AsyncServiceMethodInfo
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="methodName">The name of the async method.</param>
        /// <param name="returnType">The async return type of the method; i.e. the "T" from a "Task&lt;T&gt;".</param>
        public AsyncServiceMethodInfo(Type serviceType, string methodName, Type returnType)
        {
            ServiceType = serviceType;
            MethodName = methodName;
            ReturnType = returnType;
        }

        /// <summary>
        /// The type of the service.
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// The name of the async method.
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// The async return type of the method; i.e. the "T" from a "Task&lt;T&gt;".
        /// </summary>
        public Type ReturnType { get; }
    }
}
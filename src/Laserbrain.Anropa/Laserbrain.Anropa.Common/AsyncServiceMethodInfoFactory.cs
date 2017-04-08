using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Laserbrain.Anropa
{
    public static class AsyncServiceMethodInfoFactory
    {
        public static AsyncServiceMethodInfo CreateFromMethodInfo(MethodInfo methodInfo)
        {
            // Validate the return type of the method.
            if (!methodInfo.ReturnType.IsGenericType ||
                (methodInfo.ReturnType.GetGenericTypeDefinition() != typeof(Task<>)))
            {
                throw new Exception($"Invalid service method. The method {methodInfo.Name} on the service {methodInfo.DeclaringType} does not return a Task<T>.");
            }

            // Validate the number of arguments needed for the called method.
            int argumentCount = methodInfo.GetGenericArguments().Length;
            if (argumentCount > 1)
            {
                throw new InvalidOperationException($"Invalid service method. The method {methodInfo.Name} on the service {methodInfo.DeclaringType} has too many arguments ({argumentCount}). The framework only supports 0 or 1 argument.");
            }

            // Get the return type T (out of the actual return type of Task<T>).
            Type returnType = methodInfo.ReturnType.GetGenericArguments()[0];

            return new AsyncServiceMethodInfo(methodInfo.DeclaringType, methodInfo.Name, returnType);
        }
    }
}
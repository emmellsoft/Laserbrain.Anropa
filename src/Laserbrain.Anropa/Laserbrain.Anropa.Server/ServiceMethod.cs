using System.Reflection;

namespace Laserbrain.Anropa.Server
{
    public class ServiceMethod
    {
        public ServiceMethod(object instance, MethodInfo methodInfo, AsyncServiceMethodInfo asyncServiceMethodInfo)
        {
            Instance = instance;
            MethodInfo = methodInfo;
            AsyncServiceMethodInfo = asyncServiceMethodInfo;
        }

        public object Instance { get; }

        public MethodInfo MethodInfo { get; }

        public AsyncServiceMethodInfo AsyncServiceMethodInfo { get; }
    }
}
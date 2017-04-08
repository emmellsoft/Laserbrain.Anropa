using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Laserbrain.Anropa.Server
{
    public class ServiceMethodLocator : IServiceMethodLocator
    {
        private readonly IEnumerable<IService> _services;

        public ServiceMethodLocator(IEnumerable<IService> services)
        {
            _services = services;
        }

        public IEnumerable<ServiceMethod> GetServiceMethods()
        {
            var serviceTypes = new HashSet<Type>();
            foreach (IService service in _services)
            {
                Type serviceType = service.GetType();
                if (!serviceTypes.Add(serviceType))
                {
                    continue;
                }

                foreach (MethodInfo methodInfo in serviceType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (methodInfo.DeclaringType == typeof(object))
                    {
                        // Skip the methods declared for all objects.
                        continue;
                    }

                    AsyncServiceMethodInfo asyncServiceMethodInfo;
                    try
                    {
                        asyncServiceMethodInfo = AsyncServiceMethodInfoFactory.CreateFromMethodInfo(methodInfo);
                    }
                    catch
                    {
                        Debug.WriteLine($"Ignoring the method {methodInfo.Name} on {methodInfo.DeclaringType?.Name}.");
                        continue;
                    }

                    yield return new ServiceMethod(service, methodInfo, asyncServiceMethodInfo);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Laserbrain.Anropa.Client
{
    public class ServiceHelper
    {
        private readonly Dictionary<Type, object> _serviceProxies = new Dictionary<Type, object>();

        public ServiceHelper(IServerCaller serverCaller)
            : this(serverCaller, new[] { Assembly.GetExecutingAssembly() })
        {
        }

        public ServiceHelper(IServerCaller serverCaller, IEnumerable<Assembly> assemblies)
        {
            Type serviceInterfaceType = typeof(IService);

            Type[] serviceInterfaces = assemblies.SelectMany(x => x.GetTypes())
                .Where(type => serviceInterfaceType.IsAssignableFrom(type) && type.IsInterface && (type != serviceInterfaceType))
                .ToArray();

            var proxyGenerator = new ProxyGenerator();

            var serviceInterceptor = new ServiceInterceptor(serverCaller);

            foreach (Type serviceInterface in serviceInterfaces)
            {
                object serviceProxy = proxyGenerator.CreateInterfaceProxyWithoutTarget(serviceInterface, serviceInterceptor);
                _serviceProxies.Add(serviceInterface, serviceProxy);
            }
        }

        public T GetService<T>() where T : IService
        {
            object serviceProxy;
            if (!_serviceProxies.TryGetValue(typeof(T), out serviceProxy))
            {
                throw new ArgumentException($"The service {nameof(T)} could not be resolved!");
            }

            return (T)serviceProxy;
        }

        public bool TryGetService<T>(out T service) where T : IService
        {
            object serviceProxy;
            if (!_serviceProxies.TryGetValue(typeof(T), out serviceProxy))
            {
                service = default(T);
                return false;
            }

            service = (T)serviceProxy;
            return true;
        }
    }
}
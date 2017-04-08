using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Nancy.TinyIoc;

namespace Laserbrain.Anropa.Server.Nancy
{
    public static class TinyIoCServiceLocator
    {
        public static IEnumerable<IService> GetServices(IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .SelectMany(x => x.GetTypes())
                .Where(type => typeof(IService).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .Select(ResolveService);
        }

        private static IService ResolveService(Type serviceType)
        {
            try
            {
                return (IService)TinyIoCContainer.Current.Resolve(serviceType);
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Couldn\'t resolve the service {serviceType}:\r\n{exception}");
                Debugger.Break();
                throw;
            }
        }
    }
}
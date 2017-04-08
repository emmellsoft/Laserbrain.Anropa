using System.Collections.Generic;
using System.Reflection;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace Laserbrain.Anropa.Server.Nancy
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly IEnumerable<IService> _services;

        public Bootstrapper()
            : this(TinyIoCServiceLocator.GetServices(new[] { Assembly.GetEntryAssembly() }))
        {
        }

        public Bootstrapper(IEnumerable<IService> services)
        {
            _services = services;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            container.Register<IServiceMethodLocator>(new ServiceMethodLocator(_services));
        }
    }
}
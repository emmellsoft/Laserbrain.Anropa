using System.Collections.Generic;

namespace Laserbrain.Anropa.Server
{
    public interface IServiceMethodLocator
    {
        IEnumerable<ServiceMethod> GetServiceMethods();
    }
}
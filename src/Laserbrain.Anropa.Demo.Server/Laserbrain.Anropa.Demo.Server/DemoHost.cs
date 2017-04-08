using System;
using Laserbrain.Anropa.Server.Nancy;
using Nancy.Hosting.Self;

namespace Laserbrain.Anropa.Demo.Server
{
    public class DemoHost : IDisposable
    {
        private readonly NancyHost _nancyHost;

        public DemoHost(Uri uri)
        {
            var configuration = new HostConfiguration
            {
                UrlReservations =
                {
                    CreateAutomatically = true
                }
            };

            _nancyHost = new NancyHost(
                uri,
                new Bootstrapper(),
                configuration);

            _nancyHost.Start();
        }

        public void Dispose()
        {
            _nancyHost?.Dispose();
        }
    }
}
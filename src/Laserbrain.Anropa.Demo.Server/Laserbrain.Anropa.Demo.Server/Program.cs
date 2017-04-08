using System;

namespace Laserbrain.Anropa.Demo.Server
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using (new DemoHost(new Uri("http://localhost:54321")))
            {
                Console.WriteLine("Host started and ready to serve. Press a key to exit.");
                Console.ReadKey(true);
            }
        }
    }
}

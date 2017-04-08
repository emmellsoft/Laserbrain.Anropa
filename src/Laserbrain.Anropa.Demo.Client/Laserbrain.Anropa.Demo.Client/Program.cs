using System;
using System.IO;
using System.Threading.Tasks;
using Laserbrain.Anropa.Client;
using Laserbrain.Anropa.Demo.Common;

namespace Laserbrain.Anropa.Demo.Client
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Task.Factory.StartNew(CallServer).ConfigureAwait(false).GetAwaiter().GetResult().Wait();
        }

        private static async Task CallServer()
        {
            var serverCaller = new ServerCaller("http://localhost:54321");

            var serviceHelper = new ServiceHelper(serverCaller, new[] { typeof(ITestService).Assembly });

            ITestService testService = serviceHelper.GetService<ITestService>();

            while (true)
            {
                try
                {
                    using (var stream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }))
                    {
                        TestRequest testRequest = new TestRequest(
                            true,
                            42,
                            42424242424242,
                            DateTime.Now);

                        TestResponse testResponse = await testService.Test(
                            testRequest,
                            stream,
                            new byte[] { 11, 22, 33, 44, 55, 66, 77, 88, 99 },
                            "Hello, world!",
                            42424242,
                            4242.4242m,
                            4242.4242,
                            true,
                            null,
                            null).ConfigureAwait(false);


                        Console.WriteLine("Result:");
                        Console.WriteLine(testResponse);

                        Console.WriteLine();
                        Console.Write("Press any key to repeat -- except ESC that exists!");
                        if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                        {
                            return;
                        }

                        Console.WriteLine();
                        Console.WriteLine();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }
    }
}

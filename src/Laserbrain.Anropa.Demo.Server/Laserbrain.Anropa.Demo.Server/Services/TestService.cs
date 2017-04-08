using System.IO;
using System.Threading.Tasks;
using Laserbrain.Anropa.Demo.Common;

namespace Laserbrain.Anropa.Demo.Server.Services
{
    public class TestService : ITestService
    {
        public Task<PingResponse> Ping(PingRequest request)
        {
            return Task.FromResult(new PingResponse(request.Message));
        }

        public async Task<TestResponse> Test(TestRequest request, Stream stream, byte[] bytes, string text, long number, decimal money, double floating, bool perhaps, string maybe, int? possible)
        {
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, buffer.Length);

            return new TestResponse(string.Join(", ", buffer) + " - " + string.Join(", ", bytes) + "\r\n" +
                $"\"{text}\", {number}, {money}, {floating}, {perhaps}, \"{maybe}\", {possible?.ToString() ?? "null"}");
        }
    }
}
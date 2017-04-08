using System.IO;
using System.Threading.Tasks;

namespace Laserbrain.Anropa.Demo.Common
{
    public interface ITestService : IService
    {
        Task<PingResponse> Ping(PingRequest request);

        Task<TestResponse> Test(TestRequest request, Stream stream, byte[] bytes, string text, long number, decimal money, double floating, bool perhaps, string maybe, int? possible);
    }
}
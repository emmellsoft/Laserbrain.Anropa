using System.Threading.Tasks;

namespace Laserbrain.Anropa.Client
{
    public interface IServerCaller
    {
        Task<object> Call(AsyncServiceMethodInfo asyncServiceMethodInfo, CallParam[] callParams);
    }
}
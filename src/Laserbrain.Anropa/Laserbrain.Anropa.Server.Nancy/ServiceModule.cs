using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Nancy;
using Newtonsoft.Json;

namespace Laserbrain.Anropa.Server.Nancy
{
    public class ServiceModule : NancyModule
    {
        public ServiceModule(IServiceMethodLocator serviceMethodLocator)
        {
            foreach (ServiceMethod serviceMethod in serviceMethodLocator.GetServiceMethods())
            {
                ParameterInfo[] parameterInfos = serviceMethod.MethodInfo.GetParameters();

                ServerPath serverPath = ServerPaths.GetServerPath(serviceMethod.AsyncServiceMethodInfo);

                Post[serverPath.SubPath, serverPath.FullPath, true] = async (x, ct) =>
                {
                    Task responseTask = (Task)serviceMethod.MethodInfo.Invoke(
                        serviceMethod.Instance,
                        parameterInfos.Select(CreateParameter).ToArray());

                    object result = await GetTaskResult(responseTask);

                    Response response = JsonConvert.SerializeObject(result);

                    response.ContentType = "application/json";
                    response.StatusCode = HttpStatusCode.OK;

                    return response;
                };
            }
        }

        private object CreateParameter(ParameterInfo parameterInfo)
        {
            return RequestModelSupport.CreateParameterFromRequest(Context.Request, parameterInfo);
        }

        private static async Task<object> GetTaskResult(Task task)
        {
            await task;

            Type taskType = task.GetType();
            if (!taskType.IsGenericType)
            {
                return null;
            }

            return taskType.GetProperty(nameof(Task<object>.Result)).GetValue(task);
        }
    }
}
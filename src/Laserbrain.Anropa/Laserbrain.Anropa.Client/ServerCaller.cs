using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Laserbrain.Anropa.Client
{
    public class ServerCaller : IServerCaller
    {
        private readonly string _rootUri;

        public ServerCaller(string rootUri)
        {
            if (rootUri.EndsWith("/"))
            {
                rootUri = rootUri.Substring(rootUri.Length - 1);
            }

            _rootUri = rootUri;
        }
        

        public async Task<object> Call(AsyncServiceMethodInfo asyncServiceMethodInfo, CallParam[] callParams)
        {
            try
            {
                // Get the URI for the HTTP server call.
                ServerPath path = ServerPaths.GetServerPath(asyncServiceMethodInfo);

                using (var httpClient = new HttpClient())
                {
                    using (HttpContent content = HttpContentSupport.Create(callParams))
                    {
                        HttpResponseMessage response = await httpClient.PostAsync(_rootUri + path, content).ConfigureAwait(false);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new ServerCallException(asyncServiceMethodInfo, response.StatusCode);
                        }

                        using (Stream responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            if (responseStream == null)
                            {
                                throw new ServerCallException(asyncServiceMethodInfo, HttpStatusCode.NoContent);
                            }

                            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                            {
                                string responseJson = await reader.ReadToEndAsync().ConfigureAwait(false);
                                return JsonConvert.DeserializeObject(responseJson, asyncServiceMethodInfo.ReturnType);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new ServerCallException(asyncServiceMethodInfo, "Exception during server method call", exception);
            }
        }
    }
}
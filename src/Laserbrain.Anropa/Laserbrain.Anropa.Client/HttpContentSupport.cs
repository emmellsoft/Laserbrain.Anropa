using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Laserbrain.Anropa.Client
{
    public static class HttpContentSupport
    {
        public static HttpContent Create(CallParam[] callParams)
        {
            var formData = new MultipartFormDataContent();

            foreach (CallParam callParam in callParams)
            {
                AddContent(formData, callParam);
            }

            return formData;
        }

        private static void AddContent(MultipartFormDataContent formData, CallParam callParam)
        {
            if (callParam.Value == null)
            {
                formData.Add(new StringContent(string.Empty), callParam.Name);
            }
            else if (typeof(Stream).IsAssignableFrom(callParam.Type))
            {
                formData.Add(new StreamContent((Stream)callParam.Value), callParam.Name, callParam.Name);
            }
            else if (callParam.Type == typeof(byte[]))
            {
                //formData.Add(new ByteArrayContent((byte[])value), propertyInfo.Name); // <-- Couldn't pick it up on the server side... :-(

                var memoryStream = new MemoryStream((byte[])callParam.Value);
                formData.Add(new StreamContent(memoryStream), callParam.Name, callParam.Name);
            }
            else
            {
                string json = JsonConvert.SerializeObject(callParam.Value);
                formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), callParam.Name);
            }
        }
    }
}
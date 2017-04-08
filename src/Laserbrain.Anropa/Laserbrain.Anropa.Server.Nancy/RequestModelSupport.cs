using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Nancy;
using Newtonsoft.Json;

namespace Laserbrain.Anropa.Server.Nancy
{
    internal static class RequestModelSupport
    {
        public static object CreateParameterFromRequest(Request request, ParameterInfo parameterInfo)
        {
            if (parameterInfo.ParameterType == typeof(Stream))
            {
                HttpFile httpFile = request.Files.FirstOrDefault(x => MatchFileName(x, parameterInfo));

                return httpFile?.Value;
            }

            if (parameterInfo.ParameterType == typeof(byte[]))
            {
                HttpFile httpFile = request.Files.FirstOrDefault(x => MatchFileName(x, parameterInfo));
                if (httpFile == null)
                {
                    return null;
                }

                byte[] buffer = new byte[httpFile.Value.Length];
                httpFile.Value.Read(buffer, 0, buffer.Length);
                return buffer;
            }

            IDictionary<string, object> form = request.Form;

            object value;
            if (form.TryGetValue(parameterInfo.Name, out value))
            {
                if (value == null)
                {
                    return null;
                }

                //if (parameterInfo.ParameterType == typeof(byte[]))
                //{
                //    DynamicDictionaryValue v = (DynamicDictionaryValue) value;
                //    var xx = v.GetTypeCode();
                //    var t = value.GetType();
                //    var xxxx = request.Form["Bytes"];
                //    var bytes = (byte[]) value;
                //    return bytes;
                //}

                return JsonConvert.DeserializeObject(value.ToString(), parameterInfo.ParameterType);
            }

            return null;
        }

        private static bool MatchFileName(HttpFile httpFile, ParameterInfo parameterInfo)
        {
            return string.Equals(httpFile.Name, parameterInfo.Name, StringComparison.OrdinalIgnoreCase) ||
                   httpFile.Name.StartsWith(parameterInfo.Name + ";", StringComparison.OrdinalIgnoreCase);
        }
    }
}
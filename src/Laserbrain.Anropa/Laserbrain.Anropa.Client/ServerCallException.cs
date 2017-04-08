using System;
using System.Net;

namespace Laserbrain.Anropa.Client
{
    public class ServerCallException : Exception
    {
        public ServerCallException(AsyncServiceMethodInfo serviceMethodInfo, HttpStatusCode statusCode)
        {
            ServiceMethodInfo = serviceMethodInfo;
            StatusCode = statusCode;
        }

        public ServerCallException(AsyncServiceMethodInfo serviceMethodInfo, string message, Exception innerException)
            : base(message, innerException)
        {
            ServiceMethodInfo = serviceMethodInfo;
        }

        public AsyncServiceMethodInfo ServiceMethodInfo { get; }

        public HttpStatusCode? StatusCode { get; }
    }
}
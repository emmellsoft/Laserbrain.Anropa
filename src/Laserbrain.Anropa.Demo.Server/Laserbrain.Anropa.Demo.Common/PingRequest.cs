namespace Laserbrain.Anropa.Demo.Common
{
    public class PingRequest
    {
        public PingRequest(string message)
        {
            Message = message;
        }

        public string Message { get; }

        public override string ToString() => Message;
    }
}
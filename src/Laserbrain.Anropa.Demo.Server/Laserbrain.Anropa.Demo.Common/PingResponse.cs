namespace Laserbrain.Anropa.Demo.Common
{
    public class PingResponse
    {
        public PingResponse(string message)
        {
            Message = message;
        }

        public string Message { get; }

        public override string ToString() => Message;
    }
}
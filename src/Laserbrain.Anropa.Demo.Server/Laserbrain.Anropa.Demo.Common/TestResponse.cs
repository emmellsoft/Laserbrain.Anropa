namespace Laserbrain.Anropa.Demo.Common
{
    public class TestResponse
    {
        public TestResponse(string message)
        {
            Message = message;
        }

        public string Message { get; }

        public override string ToString() => Message;
    }
}
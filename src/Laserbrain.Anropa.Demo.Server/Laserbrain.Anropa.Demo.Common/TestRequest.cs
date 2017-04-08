using System;

namespace Laserbrain.Anropa.Demo.Common
{
    public class TestRequest
    {
        public TestRequest(bool bit, int integer, long longInteger, DateTime timestamp)
        {
            Bit = bit;
            Integer = integer;
            LongInteger = longInteger;
            Timestamp = timestamp;
        }

        public bool Bit { get; }

        public int Integer { get; }

        public long LongInteger { get; }

        public DateTime Timestamp { get; }
    }
}
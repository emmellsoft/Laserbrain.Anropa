using System;

namespace Laserbrain.Anropa.Client
{
    public class CallParam
    {
        public CallParam(string name, Type type, object value)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public string Name { get; }

        public Type Type { get; }

        public object Value { get; }
    }
}
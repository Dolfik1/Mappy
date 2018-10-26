using System;

namespace Mappy
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
        AllowMultiple = false)]
    public class IdAttribute : Attribute { }
}

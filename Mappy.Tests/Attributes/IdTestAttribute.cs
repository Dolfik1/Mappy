using System;

namespace Mappy.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class IdTestAttribute : Attribute { }
}

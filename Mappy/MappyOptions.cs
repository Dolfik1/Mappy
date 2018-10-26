using Mappy.Utils;
using System;

namespace Mappy
{
    public struct MappyOptions
    {
        public string Delimiter { get; }
        public Type IdAttributeType { get; }
        public StringComparison StringComparison { get; }
        public IMappyCache Cache { get; }

        public string PrimitiveCollectionSign { get; }

        public MappyOptions(
            string delimiter = "_",
            Type idAttributeType = null,
            StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase,
            IMappyCache cache = null,
            string primitiveCollectionSign = "$")
        {
            Delimiter = delimiter;
            IdAttributeType = idAttributeType ?? typeof(IdAttribute);
            StringComparison = stringComparison;

            Cache = cache ?? new MappyCache();
            PrimitiveCollectionSign = primitiveCollectionSign;
        }

        public static MappyOptions Default = new MappyOptions(
            delimiter: "_");

        public override int GetHashCode()
        {
            return HashCode.CombineHashCodes(
                Delimiter.GetHashCode(),
                IdAttributeType.GetHashCode(),
                StringComparison.GetHashCode());
        }
    }
}

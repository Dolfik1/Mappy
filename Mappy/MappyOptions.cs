using Mappy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Mappy.Converters;

namespace Mappy
{
    public struct MappyOptions
    {
        public string Delimiter { get; }
        public Type IdAttributeType { get; }
        public StringComparison StringComparison { get; }
        public IMappyCache Cache { get; }
        
        public string PrimitiveCollectionSign { get; }
        
        public IReadOnlyCollection<ITypeConverter> Converters { get; }

        public MappyOptions(
            string delimiter = "_",
            Type idAttributeType = null,
            StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase,
            IMappyCache cache = null,
            string primitiveCollectionSign = "$",
            IEnumerable<ITypeConverter> converters = null)
        {
            Delimiter = delimiter;
            IdAttributeType = idAttributeType ?? typeof(IdAttribute);
            StringComparison = stringComparison;

            Cache = cache ?? new MappyCache();
            PrimitiveCollectionSign = primitiveCollectionSign;

            if (converters == null)
            {
                converters = new List<ITypeConverter>
                {
                    new GuidConverter(),
                    new EnumConverter(),
                    new ValueTypeConverter(),
                    new BaseConverter()
                };
            }

            Converters = converters
                .OrderBy(x => x.Order)
                .ToList();
        }

        public static MappyOptions Default = new MappyOptions(delimiter: "_");

        public override int GetHashCode()
        {
            return HashCode.CombineHashCodes(
                Delimiter.GetHashCode(),
                IdAttributeType.GetHashCode(),
                StringComparison.GetHashCode());
        }
    }
}

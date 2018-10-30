using Mappy.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Mappy
{
    public class MappyOptions
    {
        public string Delimiter { get; }

        public Type IdAttributeType { get; }

        public StringComparison StringComparison { get; }
        public bool UseDefaultDictionaryComparer { get; }
        internal StringComparer StringComparer { get; }

        public IMappyCache Cache { get; }

        public string PrimitiveCollectionSign { get; }

        public IReadOnlyCollection<ITypeConverter> Converters { get; }


        public MappyOptions(
            string delimiter = "_",
            Type idAttributeType = null,
            StringComparison stringComparison = StringComparison.Ordinal,
            bool useDefaultDictionaryComparer = true,
            IMappyCache cache = null,
            string primitiveCollectionSign = "$",
            IEnumerable<ITypeConverter> converters = null)
        {
            Delimiter = delimiter;

            IdAttributeType = idAttributeType ?? typeof(IdAttribute);

            StringComparison = stringComparison;
            UseDefaultDictionaryComparer = useDefaultDictionaryComparer;
            StringComparer = GetStringComparer(StringComparison);

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

        private StringComparer GetStringComparer(StringComparison comparison)
        {
            var culture = CultureInfo.InvariantCulture;
            var ignoreCase = comparison == StringComparison.CurrentCultureIgnoreCase
                             || comparison == StringComparison.InvariantCultureIgnoreCase
                             || comparison == StringComparison.OrdinalIgnoreCase;

            switch (comparison)
            {
                case StringComparison.CurrentCultureIgnoreCase:
                case StringComparison.CurrentCulture:
                    culture = CultureInfo.CurrentCulture;
                    break;
                case StringComparison.OrdinalIgnoreCase:
                    return StringComparer.OrdinalIgnoreCase;
                case StringComparison.Ordinal:
                    return StringComparer.Ordinal;
            }

            return StringComparer.Create(culture, ignoreCase);
        }

        public static readonly MappyOptions Default = new MappyOptions(delimiter: "_");
    }
}

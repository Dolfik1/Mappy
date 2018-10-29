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

        private StringComparer GetStringComparer(StringComparison comparsion)
        {
            var culture = CultureInfo.InvariantCulture;
            var ignoreCase = false;

            if (comparsion == StringComparison.CurrentCultureIgnoreCase
                || comparsion == StringComparison.InvariantCultureIgnoreCase
                || comparsion == StringComparison.OrdinalIgnoreCase)
            {
                ignoreCase = true;
            }

            if (comparsion == StringComparison.CurrentCultureIgnoreCase
                || comparsion == StringComparison.CurrentCulture)
            {
                culture = CultureInfo.CurrentCulture;
            }

            if (comparsion == StringComparison.OrdinalIgnoreCase)
            {
                return StringComparer.OrdinalIgnoreCase;
            }
            else if (comparsion == StringComparison.Ordinal)
            {
                return StringComparer.Ordinal;
            }



            return StringComparer.Create(culture, ignoreCase);
        }

        public static MappyOptions Default = new MappyOptions(delimiter: "_");
    }
}

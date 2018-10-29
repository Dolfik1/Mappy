using System.Collections.Generic;
using System.Linq;
using Mappy.Converters;
using Items = System.Collections.Generic.IDictionary<string, object>;

namespace Mappy
{
    internal class MappingContext
    {
        private MappyOptions Options { get; }
        private IDictionary<string, ITypeConverter> TypeConverters { get; }
        
        internal MappingContext(MappyOptions options)
        {
            Options = options;
            TypeConverters = new Dictionary<string, ITypeConverter>();
        }
        
        internal T? ConvertNullable<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
            where T : struct
        {
            var pfx = prefix + name;
            
            if (!items.TryGetValue(prefix + name, out var value) || value == null)
                return default(T?);

            if (TypeConverters.TryGetValue(pfx, out var converter)) 
                return converter.Convert<T>(value);
            
            converter = Options.Converters.First(x => x.CanConvert<T>(value));
            TypeConverters.Add(pfx, converter);

            return converter.Convert<T>(value);
        }
        
        internal T? ConvertNullableComplex<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
            where T : struct
        {
            var pfx = string.IsNullOrEmpty(prefix)
                      && string.IsNullOrEmpty(name) ? ""
                : prefix + name + Options.Delimiter;

            if (!items
                .Any(x => x.Key.StartsWith(pfx, Options.StringComparison)))
            {
                return default(T?);
            }

            var mapper = Options.Cache
                .GetOrCreateTypeMap<T>(Options);

            if (!mapper.HasValues(pfx, items, Options))
            {
                return default(T?);
            }

            return mapper.Map(this, pfx, values);
        }

        internal T Convert<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            var pfx = prefix + name;
            if (!items.TryGetValue(pfx, out var value) || value == null)
                return default(T);

            if (TypeConverters.TryGetValue(pfx, out var converter)) 
                return converter.Convert<T>(value);
            
            converter = Options.Converters.First(x => x.CanConvert<T>(value));
            TypeConverters.Add(pfx, converter);

            return converter.Convert<T>(value);
        }

        internal T ConvertComplex<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            var pfx = string.IsNullOrEmpty(prefix)
                && string.IsNullOrEmpty(name) ? ""
                : prefix + name + Options.Delimiter;

            if (!items
                .Any(x => x.Key.StartsWith(pfx, Options.StringComparison)))
            {
                return default(T);
            }

            var mapper = Options.Cache
                .GetOrCreateTypeMap<T>(Options);


            if (!mapper.HasValues(pfx, items, Options))
            {
                return default(T);
            }

            return mapper.Map(this, pfx, values);
        }

        internal List<T> ConvertList<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            return ConvertEnumerable<T>(prefix, name, items, values)
                .ToList();
        }

        internal List<T> ConvertListComplex<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            return ConvertEnumerableComplex<T>(prefix, name, items, values)
                .ToList();
        }

        internal T[] ConvertArrayComplex<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            return ConvertEnumerableComplex<T>(prefix, name, items, values)
                .ToArray();
        }

        internal T[] ConvertArray<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            return ConvertEnumerable<T>(prefix, name, items, values)
                ?.ToArray();
        }

        internal IEnumerable<T> ConvertEnumerable<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            var pfx = string.IsNullOrEmpty(prefix)
                && string.IsNullOrEmpty(name) ? ""
                : prefix + name;

            if (items.TryGetValue(pfx, out var arrayValue))
            {
                return (IEnumerable<T>) arrayValue;
            }

            pfx += Options.Delimiter;

            return values
                .Select(x => Convert<T>(pfx, 
                    Options.PrimitiveCollectionSign, x, null));
        }

        private IEnumerable<T> ConvertEnumerableComplex<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            var pfx = string.IsNullOrEmpty(prefix)
                && string.IsNullOrEmpty(name) ? ""
                : prefix + name + Options.Delimiter;

            var mapper = Options.Cache
                .GetOrCreateTypeMap<T>(Options);

            return values
                .Where(x => mapper.HasValues(pfx, x, Options))
                .GroupBy(x => mapper.GetIdentifierHashCode(pfx, x))
                .Select(x => mapper.Map(this, pfx, x));
        }
    }
}
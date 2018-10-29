using System.Collections.Generic;
using System.Linq;
using Items = System.Collections.Generic.IDictionary<string, object>;

namespace Mappy.Utils
{
    internal static class ConvertUtility
    {
        internal static T? ConvertNullable<T>(
            MappyOptions options,
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
            where T : struct
        {
            if (!items.TryGetValue(prefix + name, out var value) || value == null)
                return default(T?);

            foreach (var converter in options.Converters)
            {
                if (converter.CanConvert<T>(value))
                {
                    return converter.Convert<T>(value);
                }
            }

            return default(T?);
        }
        internal static T? ConvertNullableComplex<T>(
            MappyOptions options,
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
            where T : struct
        {
            var pfx = string.IsNullOrEmpty(prefix)
                      && string.IsNullOrEmpty(name) ? ""
                : prefix + name + options.Delimiter;

            if (!items
                .Any(x => x.Key.StartsWith(pfx, options.StringComparison)))
            {
                return default(T?);
            }

            var mapper = options.Cache
                .GetOrCreateTypeMap<T>(options);


            if (!mapper.HasValues(pfx, items, options))
            {
                return default(T?);
            }

            return mapper.Map(pfx, values, options);
        }

        internal static T Convert<T>(
            MappyOptions options,
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            if (!items.TryGetValue(prefix + name, out var value) || value == null)
                return default(T);

            foreach (var converter in options.Converters)
            {
                if (converter.CanConvert<T>(value))
                {
                    return converter.Convert<T>(value);
                }
            }

            return default(T);
        }

        internal static T ConvertComplex<T>(
            MappyOptions options,
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            var pfx = string.IsNullOrEmpty(prefix)
                && string.IsNullOrEmpty(name) ? ""
                : prefix + name + options.Delimiter;

            if (!items
                .Any(x => x.Key.StartsWith(pfx, options.StringComparison)))
            {
                return default(T);
            }

            var mapper = options.Cache
                .GetOrCreateTypeMap<T>(options);


            if (!mapper.HasValues(pfx, items, options))
            {
                return default(T);
            }

            return mapper.Map(pfx, values, options);
        }

        internal static List<T> ConvertList<T>(
            MappyOptions options,
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            return ConvertEnumerable<T>(options, prefix, name, items, values)
                .ToList();
        }

        internal static List<T> ConvertListComplex<T>(
            MappyOptions options,
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            return ConvertEnumerableComplex<T>(options, prefix, name, items, values)
                .ToList();
        }

        internal static T[] ConvertArrayComplex<T>(
            MappyOptions options,
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            return ConvertEnumerableComplex<T>(options, prefix, name, items, values)
                .ToArray();
        }

        internal static T[] ConvertArray<T>(
            MappyOptions options,
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            return ConvertEnumerable<T>(options, prefix, name, items, values)
                ?.ToArray();
        }

        internal static IEnumerable<T> ConvertEnumerable<T>(
            MappyOptions options,
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
                return arrayValue == null
                    ? null
                    : (IEnumerable<T>)arrayValue;
            }

            pfx += options.Delimiter;

            return values
                .Select(x => Convert<T>(
                    options, pfx, options.PrimitiveCollectionSign,
                    x, null));
        }

        private static IEnumerable<T> ConvertEnumerableComplex<T>(
            MappyOptions options,
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values)
        {
            var pfx = string.IsNullOrEmpty(prefix)
                && string.IsNullOrEmpty(name) ? ""
                : prefix + name + options.Delimiter;

            var mapper = options.Cache
                .GetOrCreateTypeMap<T>(options);

            return values
                .Where(x => mapper.HasValues(pfx, x, options))
                .GroupBy(x => mapper.GetIdentifierHashCode(pfx, x))
                .Select(x => mapper.Map(pfx, x, options));
        }
    }
}

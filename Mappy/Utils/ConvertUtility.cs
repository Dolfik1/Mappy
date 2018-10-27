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
            bool isComplexType,
            Items items,
            IEnumerable<Items> values) 
            where T : struct
        {
            if (isComplexType)
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
                    .GetOrCreateTypeMap<T>();
                
                
                if (!mapper.HasValues(pfx, items, options))
                {
                    return default(T?);
                }

                return options.Cache
                    .GetOrCreateTypeMap<T?>()
                    .Map(pfx, values, options);
            }

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
        
        internal static T Convert<T>(
            MappyOptions options,
            string prefix,
            string name,
            bool isComplexType,
            Items items,
            IEnumerable<Items> values)
        {
            if (isComplexType)
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
                    .GetOrCreateTypeMap<T>();
                
                
                if (!mapper.HasValues(pfx, items, options))
                {
                    return default(T);
                }

                return options.Cache
                    .GetOrCreateTypeMap<T>()
                    .Map(pfx, values, options);
            }

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

        internal static List<T> ConvertList<T>(
            MappyOptions options,
            string prefix,
            string name,
            bool isComplexType,
            Items items,
            IEnumerable<Items> values)
        {
            var pfx = string.IsNullOrEmpty(prefix)
                && string.IsNullOrEmpty(name) ? ""
                : prefix + name + options.Delimiter;

            

            if (!isComplexType)
            {
                return values
                    .Where(x => x != null)
                    .Select(x => Convert<T>(options, pfx, options.PrimitiveCollectionSign, false, x, null))
                    .ToList();
            }
            
            var mapper = options.Cache
                .GetOrCreateTypeMap<T>();

            return values
                .Where(x => mapper.HasValues(pfx, x, options))
                .GroupBy(x => mapper.GetIdentifierHashCode(pfx, x))
                .Select(x => mapper.Map(pfx, x, options))
                .ToList();
        }
    }
}

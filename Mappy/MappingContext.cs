using Mappy.Comparers;
using Mappy.Converters;
using Mappy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Items = System.Collections.Generic.IDictionary<string, object>;

namespace Mappy
{
    internal class MappingContext
    {
        private MappyOptions Options { get; }
        private IDictionary<string, ITypeConverter> TypeConverters { get; }
        private IDictionary<int, string[]> ValuesFields { get; }

        internal MappingContext(MappyOptions options)
        {
            Options = options;
            TypeConverters = new Dictionary<string, ITypeConverter>(
                StringComparer.Ordinal);
            ValuesFields = new Dictionary<int, string[]>();
        }

        internal T? ConvertNullable<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values,
            T? defaultValue)
            where T : struct
        {
            var pfx = prefix + name;

            if (!items.TryGetValue(pfx, out var value) || value == null)
            {
                return defaultValue;
            }

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
            IEnumerable<Items> values,
            T? defaultValue)
            where T : struct
        {
            var pfx = string.IsNullOrEmpty(prefix)
                      && string.IsNullOrEmpty(name) ? ""
                : prefix + name + Options.Delimiter;

            var mapper = Options.Cache
                .GetOrCreateTypeMap<T>(Options);

            if (!mapper.HasValues(this, pfx, items, Options))
            {
                return defaultValue;
            }

            return mapper.Map(this, pfx, items, values);
        }

        internal T Convert<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values,
            T defaultValue)
        {
            var pfx = prefix + name;

            if (!items.TryGetValue(pfx, out var value) || value == null)
            {
                return defaultValue;
            }

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
            IEnumerable<Items> values,
            T defaultValue)
        {
            var pfx = string.IsNullOrEmpty(prefix)
                && string.IsNullOrEmpty(name) ? ""
                : prefix + name + Options.Delimiter;

            if (!items
                .Any(x => x.Key.StartsWith(pfx, Options.StringComparison)))
            {
                return defaultValue;
            }

            var mapper = Options.Cache
                .GetOrCreateTypeMap<T>(Options);

            if (!mapper.HasValues(this, pfx, items, Options))
            {
                // Slapper undocummented bug or feature?
                // see Should_Map_First_Not_Null_Items_In_Group test
                items = values.FirstOrDefault(x => mapper.HasValues(this, pfx, x, Options));

                if (items == null)
                {
                    return defaultValue;
                }
            }

            return mapper.Map(this, pfx, items, values);
        }

        internal List<T> ConvertList<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values,
            IEnumerable<T> defaultValue)
        {
            return ConvertEnumerable(prefix, name, items, values, defaultValue)
                ?.ToList();
        }

        internal List<T> ConvertListComplex<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values,
            IEnumerable<T> defaultValue)
        {
            return ConvertEnumerableComplex(prefix, name, items, values, defaultValue)
                ?.ToList();
        }

        internal T[] ConvertArrayComplex<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values,
            IEnumerable<T> defaultValue)
        {
            return ConvertEnumerableComplex(prefix, name, items, values, defaultValue)
                ?.ToArray();
        }

        internal T[] ConvertArray<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values,
            IEnumerable<T> defaultValue)
        {
            return ConvertEnumerable(prefix, name, items, values, defaultValue)
                ?.ToArray();
        }

        internal IEnumerable<T> ConvertEnumerable<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values,
            IEnumerable<T> defaultValue)
        {
            var pfx = string.IsNullOrEmpty(prefix)
                && string.IsNullOrEmpty(name) ? ""
                : prefix + name;

            if (items.TryGetValue(pfx, out var arrayValue))
            {
                return (IEnumerable<T>)arrayValue;
            }

            pfx += Options.Delimiter;
            var pfxWithSign = pfx + Options.PrimitiveCollectionSign;

            if (!items.ContainsKey(pfxWithSign))
            {
                return defaultValue;
            }

            return values
                .Where(x => x[pfxWithSign] != null)
                .Select(x => Convert<T>(pfx,
                    Options.PrimitiveCollectionSign, x, null, default(T)));
        }

        private IEnumerable<T> ConvertEnumerableComplex<T>(
            string prefix,
            string name,
            Items items,
            IEnumerable<Items> values,
            IEnumerable<T> defaultValue)
        {
            var pfx = string.IsNullOrEmpty(prefix)
                && string.IsNullOrEmpty(name) ? ""
                : prefix + name + Options.Delimiter;

            var mapper = Options.Cache
                .GetOrCreateTypeMap<T>(Options);

            // items can be null only when MapEnumerable called

            if (!string.IsNullOrEmpty(pfx) && !mapper.HasKeys(this, pfx, items, Options))
            {
                return defaultValue;
            }

            return values
                .Where(x => mapper.HasValues(this, pfx, x, Options))
                .GroupBy(x => x, new IdentifierComparer(pfx, mapper.IdentifierFieldsAndProps))
                .Select(x => mapper.Map(this, pfx, x.Key, x));
        }

        internal bool TryGetValueFields(int hashCode, out string[] valueFields)
        {
            return ValuesFields.TryGetValue(hashCode, out valueFields);
        }

        internal void AddValueFields(int hashCode, string[] valueFields)
        {
            ValuesFields.Add(hashCode, valueFields);
        }



        internal string[] GetExistsFieldsForType<T>(
            string prefix,
            Items items,
            MappyOptions options)
        {
            var hashCode = HashCode.CombineHashCodes(prefix.GetHashCode(), typeof(T).GetHashCode());
            if (!TryGetValueFields(hashCode, out var valueFields))
            {
                valueFields = items
                    .Where(x => x.Key.StartsWith(prefix, options.StringComparison))
                    .Select(x => x.Key)
                    .ToArray();

                AddValueFields(hashCode, valueFields);
            }

            return valueFields;
        }
    }
}
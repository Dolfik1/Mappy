using Mappy.Comparers;
using Mappy.Converters;
using Mappy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Abbrs = System.Collections.Generic.IDictionary<string, string>;
using Items = System.Collections.Generic.IDictionary<string, object>;

namespace Mappy
{
    internal class MappingContext
    {
        private MappyOptions Options { get; }
        private IDictionary<string, ITypeConverter> TypeConverters { get; }
        private IDictionary<int, string[]> ValuesFields { get; }
        private Abbrs Abbreviations { get; }

        internal MappingContext(MappyOptions options, Abbrs abbreviations)
        {
            Options = options;
            TypeConverters = new Dictionary<string, ITypeConverter>(
                StringComparer.Ordinal);
            ValuesFields = new Dictionary<int, string[]>();
            Abbreviations = abbreviations;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            {
                return converter.Convert<T>(value);
            }

            converter = Options.Converters.First(x => x.CanConvert<T>(value));
            TypeConverters.Add(pfx, converter);

            return converter.Convert<T>(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T? ConvertNullableComplex<T>(
            string prefix,
            string name,
            Items items,
            List<Items> values,
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T Convert<T>(
            string prefix,
            string name,
            Items items,
            List<Items> values,
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T ConvertComplex<T>(
            string prefix,
            string name,
            Items items,
            List<Items> values,
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


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T[] ConvertArrayComplex<T>(
            string prefix,
            string name,
            Items items,
            List<Items> values,
            IEnumerable<T> defaultValue)
        {
            return ConvertListComplex(prefix, name, items, values, defaultValue)
                ?.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T[] ConvertArray<T>(
            string prefix,
            string name,
            Items items,
            List<Items> values,
            IEnumerable<T> defaultValue)
        {
            return ConvertEnumerable(prefix, name, items, values, defaultValue)
                ?.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal List<T> ConvertList<T>(
            string prefix,
            string name,
            Items items,
            List<Items> values,
            IEnumerable<T> defaultValue)
        {
            return ConvertEnumerable(prefix, name, items, values, defaultValue)
                ?.AsList();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal IEnumerable<T> ConvertEnumerable<T>(
            string prefix,
            string name,
            Items items,
            List<Items> values,
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
                return defaultValue?.AsList();
            }

            var result = new List<T>(values.Count);
            for (var i = 0; i < values.Count; i++)
            {
                if (values[i][pfxWithSign] == null) continue;
                result.Add(Convert(pfx,
                    Options.PrimitiveCollectionSign, values[i], null, default(T)));
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal List<T> ConvertListComplex<T>(
            string prefix,
            string name,
            Items items,
            List<Items> values,
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
                return defaultValue?.AsList();
            }

            var group = new Dictionary<int, List<Items>>();
            var comparer = new IdentifierComparer(pfx, mapper.IdentifierFieldsAndProps);
            for (var i = 0; i < values.Count; i++)
            {
                if (!mapper.HasValues(this, pfx, values[i], Options)) continue;
                var hc = comparer.GetHashCode(values[i]);

                if (group.ContainsKey(hc))
                {
                    group[hc].Add(values[i]);
                }
                else
                {
                    group.Add(hc, new List<Items> { values[i] });
                }
            }

            var result = new List<T>(group.Count);
            foreach (var x in group)
            {
                // x.Key replaced with x.Last() for compatibility with Slapper (see Should_Use_Last_Item_In_Array test)
                result.Add(mapper.Map(this, pfx, x.Value[x.Value.Count - 1], x.Value));
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool TryGetValueFields(int hashCode, out string[] valueFields)
        {
            return ValuesFields.TryGetValue(hashCode, out valueFields);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void AddValueFields(int hashCode, string[] valueFields)
        {
            ValuesFields.Add(hashCode, valueFields);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
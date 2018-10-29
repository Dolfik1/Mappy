using Mappy.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Items = System.Collections.Generic.IDictionary<string, object>;

namespace Mappy
{
    public class TypeMap<T> : TypeMap
    {
        internal Func<string, IEnumerable<Items>, Items, MappyOptions, T> MapExpression { get; }

        internal TypeMap(Type idAttribute)
            : base(typeof(T), idAttribute)
        {
            var type = typeof(T);

            var prefix = Expression.Parameter(typeof(string));
            var values = Expression.Parameter(typeof(IEnumerable<Items>));
            var first = Expression.Parameter(typeof(Items));
            var options = Expression.Parameter(typeof(MappyOptions));

            var newValue = Expression.New(type);

            var bindings = new List<MemberBinding>();

            MemberBinding Process(
                string propOrFieldName,
                Type propOrFieldType,
                MemberInfo mi)
            {
                var isEnumerable = propOrFieldType.Namespace != "System"
                    && typeof(IEnumerable).IsAssignableFrom(propOrFieldType);

                var isArray = propOrFieldType.IsArray;

                var underlyingType = propOrFieldType;

                if (isEnumerable || isArray)
                {
                    underlyingType =
                        isArray
                        ? propOrFieldType.GetElementType()
                        : propOrFieldType.GetGenericArguments()[0];
                }

                var isComplex = underlyingType.Namespace != "System"
                                && !underlyingType.IsPrimitive
                                && !underlyingType.IsValueType;

                var nullableType = Nullable.GetUnderlyingType(propOrFieldType);

                MethodInfo convertMethod;
                const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static;
                string methodName;

                if (isArray)
                {
                    methodName = isComplex
                        ? nameof(ConvertUtility.ConvertArrayComplex)
                        : nameof(ConvertUtility.ConvertArray);
                }
                else if (isEnumerable)
                {
                    methodName = isComplex
                        ? nameof(ConvertUtility.ConvertListComplex)
                        : nameof(ConvertUtility.ConvertList);
                }
                else if (nullableType != null)
                {
                    methodName = isComplex
                        ? nameof(ConvertUtility.ConvertNullableComplex)
                        : nameof(ConvertUtility.ConvertNullable);
                    underlyingType = nullableType;
                }
                else
                {
                    methodName = isComplex
                        ? nameof(ConvertUtility.ConvertComplex)
                        : nameof(ConvertUtility.Convert);
                }
                convertMethod = typeof(ConvertUtility).GetMethod(methodName, flags);

                var convertCall = Expression.Call(
                    convertMethod.MakeGenericMethod(underlyingType),
                    options,
                    prefix,
                    Expression.Constant(propOrFieldName),
                    first,
                    values);

                return Expression.Bind(mi, convertCall);
            }

            foreach (var property in GetProperties(type))
            {
                bindings.Add(Process(property.Name, property.PropertyType, property));
            }

            foreach (var field in GetFields(type))
            {
                bindings.Add(Process(field.Name, field.FieldType, field));
            }

            var member = Expression.MemberInit(newValue, bindings);


            MapExpression = Expression.Lambda<Func<string, IEnumerable<Items>, Items, MappyOptions, T>>(
                member, prefix, values, first, options).Compile();
        }

        internal T Map(
            string prefix,
            IEnumerable<Items> values,
            MappyOptions options)
        {
            return MapExpression(prefix, values, values.First(), options);
        }

        internal T MapSingle(Items values, MappyOptions options)
        {
            return Map("", new List<Items> { values }, options);
        }

        internal IEnumerable<T> MapEnumerable(
            IEnumerable<Items> values,
            MappyOptions options)
        {
            return ConvertUtility.ConvertListComplex<T>(
                options,
                "",
                "",
                null,
                values);
        }


        internal int GetIdentifierHashCode(
            string prefix,
            Items items)
        {
            if (IdentifierFieldsAndProps.Count == 0)
            {
                return Guid.NewGuid().GetHashCode();
            }

            var ids = IdentifierFieldsAndProps;

            IEnumerable<int> HashCodes()
            {
                foreach (var id in ids)
                {
                    if (!items.TryGetValue(prefix + id, out var value))
                    {
                        continue;
                    }
                    yield return value?.GetHashCode() ?? 0;
                }
            }

            return HashCode.CombineHashCodes(HashCodes());
        }

        internal bool HasValues(
            string prefix,
            Items items,
            MappyOptions options)
        {
            return items
                .Any(x =>
                    x.Key.StartsWith(prefix, options.StringComparison)
                    && x.Value != null);
        }
    }
}

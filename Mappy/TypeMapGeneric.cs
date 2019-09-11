using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Items = System.Collections.Generic.IDictionary<string, object>;

namespace Mappy
{
    public class TypeMap<T> : TypeMap
    {
        internal Func<MappingContext, string, IEnumerable<Items>, Items, T> MapExpression { get; }

        internal TypeMap(Type idAttribute)
            : base(typeof(T), idAttribute)
        {
            var type = typeof(T);

            var context = Expression.Parameter(typeof(MappingContext));
            var prefix = Expression.Parameter(typeof(string));
            var values = Expression.Parameter(typeof(IEnumerable<Items>));
            var first = Expression.Parameter(typeof(Items));

            var newValue = Expression.New(type);

            var defaultObject = Activator.CreateInstance<T>();

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

                if (underlyingType == null)
                {
                    throw new Exception("Can't detect type.");
                }

                var isComplex = underlyingType.Namespace != "System"
                                && !underlyingType.IsPrimitive
                                && !underlyingType.IsValueType;

                var nullableType = Nullable.GetUnderlyingType(propOrFieldType);

                string methodName;

                if (isArray)
                {
                    methodName = isComplex
                        ? nameof(MappingContext.ConvertArrayComplex)
                        : nameof(MappingContext.ConvertArray);
                }
                else if (isEnumerable)
                {
                    methodName = isComplex
                        ? nameof(MappingContext.ConvertListComplex)
                        : nameof(MappingContext.ConvertList);
                }
                else if (nullableType != null)
                {
                    methodName = isComplex
                        ? nameof(MappingContext.ConvertNullableComplex)
                        : nameof(MappingContext.ConvertNullable);
                    underlyingType = nullableType;
                }
                else
                {
                    methodName = isComplex
                        ? nameof(MappingContext.ConvertComplex)
                        : nameof(MappingContext.Convert);
                }

                var convertMethod = typeof(MappingContext).GetMethod(
                    methodName, BindingFlags.Instance | BindingFlags.NonPublic);

                if (convertMethod == null)
                {
                    throw new Exception($"Convert method with name \"{methodName}\" does not found.");
                }

                var defaultValue = Expression.PropertyOrField(
                    Expression.Constant(defaultObject),
                    propOrFieldName);

                var convertCall = Expression.Call(
                    context,
                    convertMethod.MakeGenericMethod(underlyingType),
                    prefix,
                    Expression.Constant(propOrFieldName),
                    first,
                    values,
                    defaultValue);

                return Expression.Bind(mi, convertCall);
            }

            var bindings = GetProperties(type)
                .Select(property => Process(property.Name, property.PropertyType, property))
                .ToList();

            bindings.AddRange(GetFields(type)
                .Select(field => Process(field.Name, field.FieldType, field)));

            var member = Expression.MemberInit(newValue, bindings);

            MapExpression = Expression.Lambda<Func<MappingContext, string, IEnumerable<Items>, Items, T>>(
                member, context, prefix, values, first).Compile();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T Map(
            MappingContext context,
            string prefix,
            Items first,
            IEnumerable<Items> values)
        {
            return MapExpression(context, prefix, values, first);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal IEnumerable<T> MapEnumerable(
            IEnumerable<Items> values,
            Items items,
            MappingContext context)
        {
            return context.ConvertEnumerableComplex(
                "",
                "",
                items,
                values,
                default(List<T>));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool HasValues(
            MappingContext context,
            string prefix,
            Items items,
            MappyOptions options)
        {
            var valueFields = context.GetExistsFieldsForType<T>(prefix, items, options);

            if (items.Count == 0)
            {
                return false;
            }

            foreach (var field in valueFields)
            {
                if (items[field] != null)
                {
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool HasKeys(
            MappingContext context,
            string prefix,
            Items items,
            MappyOptions options)
        {
            var valueFields = context.GetExistsFieldsForType<T>(prefix, items, options);
            return items.Count != 0 && valueFields.Length != 0;
        }
    }
}

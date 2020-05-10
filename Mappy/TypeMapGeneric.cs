using FastExpressionCompiler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using static System.Linq.Expressions.Expression;
using Items = System.Collections.Generic.IDictionary<string, object>;

namespace Mappy
{
    public class TypeMap<T> : TypeMap
    {
        internal Func<MappingContext, string, List<Items>, Items, T> MapExpression { get; }

        internal TypeMap(Type idAttribute)
            : base(typeof(T), idAttribute)
        {
            var type = typeof(T);

            var context = Parameter(typeof(MappingContext));
            var prefix = Parameter(typeof(string));
            var values = Parameter(typeof(List<Items>));
            var first = Parameter(typeof(Items));

            var newValue = New(type);

            var defaultObject = Activator.CreateInstance<T>();

            MemberBinding Process(
                string propOrFieldName,
                Type propOrFieldType,
                MemberInfo mi)
            {
                var isEnumerable = propOrFieldType.Namespace != "System"
                    && typeof(IEnumerable).IsAssignableFrom(propOrFieldType);

                var isArray = propOrFieldType.IsArray;
                var isGeneric = propOrFieldType.IsGenericType;
                
                var underlyingType = propOrFieldType;

                if (isGeneric || isArray)
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

                Expression defaultValue;
                if (mi is PropertyInfo pi)
                {
                    defaultValue = Constant(
                        pi.GetValue(defaultObject), propOrFieldType);
                }
                else if (mi is FieldInfo fi)
                {
                    defaultValue = Constant(
                        fi.GetValue(defaultObject), propOrFieldType);
                }
                else
                {
                    defaultValue = PropertyOrField(
                        Constant(defaultObject),
                        propOrFieldName);
                }

                var convertMethodGeneric =
                    isArray || isEnumerable || nullableType != null
                        ? convertMethod.MakeGenericMethod(underlyingType)
                        : convertMethod.MakeGenericMethod(propOrFieldType);

                var convertCall = Call(
                    context,
                    convertMethodGeneric,
                    prefix,
                    Constant(propOrFieldName),
                    first,
                    values,
                    defaultValue);

                return Bind(mi, convertCall);
            }

            var bindings = GetProperties(type)
                .Select(property => Process(property.Name, property.PropertyType, property))
                .ToList();

            bindings.AddRange(GetFields(type)
                .Select(field => Process(field.Name, field.FieldType, field)));

            var member = MemberInit(newValue, bindings.ToArray());

            MapExpression = Lambda<Func<MappingContext, string, List<Items>, Items, T>>(
                member, context, prefix, values, first).CompileFast();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T Map(
            MappingContext context,
            string prefix,
            Items first,
            List<Items> values)
        {
            return MapExpression(context, prefix, values, first);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal List<T> MapList(
            List<Items> values,
            Items items,
            MappingContext context)
        {
            return context.ConvertListComplex(
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

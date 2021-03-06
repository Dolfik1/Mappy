using FastExpressionCompiler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

            var props = GetProperties(type);
            var fields = GetFields(type);

            var context = Parameter(typeof(MappingContext));
            var prefix = Parameter(typeof(string));
            var values = Parameter(typeof(List<Items>));
            var first = Parameter(typeof(Items));

            var constructorParams = ConstructorToUse?.GetParameters();
            var defaultObject = 
                (constructorParams?.Length ?? 0) == 0 
                ? Activator.CreateInstance<T>() : default;

            (MethodCallExpression, MemberInfo) Process(
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
                    if (propOrFieldType.GetGenericTypeDefinition() == typeof(SortedSet<>))
                    {
                        methodName = isComplex
                            ? nameof(MappingContext.ConvertSortedSetComplex)
                            : nameof(MappingContext.ConvertSortedSet);
                    }
                    else
                    {
                        methodName = isComplex
                            ? nameof(MappingContext.ConvertListComplex)
                            : nameof(MappingContext.ConvertList);
                    }
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
                if (defaultObject != null)
                {
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
                }
                else
                {
                    defaultValue = Default(propOrFieldType);
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

                // return Bind(mi, convertCall);
                return (convertCall, mi);
            }

            var converts =
                props
                .Select(property => Process(property.Name, property.PropertyType, property))
                .ToList();

            converts.AddRange(fields
                .Select(field => Process(field.Name, field.FieldType, field)));

            var constructorArguments =
                constructorParams?.Select(x =>
                {
                    var converter = 
                        converts.First(c =>
                            c.Item2.Name.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
                    converts.Remove(converter);
                    return converter.Item1;
                });

            NewExpression newValue;
            if (IsValueType || ConstructorToUse == null || constructorParams == null)
            {
                newValue = New(type);
            }
            else
            {
                newValue = New(ConstructorToUse, constructorArguments);
            }

            Expression expr;
            if (converts.Count > 0)
            {
                var bindings = converts
                    .Select(t => (MemberBinding)Bind(t.Item2, t.Item1));
            
                expr = MemberInit(newValue, bindings.ToArray());
            }
            else
            {
                expr = newValue;
            }

            MapExpression = Lambda<Func<MappingContext, string, List<Items>, Items, T>>(
                expr, context, prefix, values, first).CompileFast();
            
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

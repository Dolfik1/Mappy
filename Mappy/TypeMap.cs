using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Mappy
{
    public class TypeMap
    {
        internal string[] FieldsAndProps { get; }
        internal string[] IdentifierFieldsAndProps { get; }

        internal ConstructorInfo ConstructorToUse { get; }
        internal bool IsValueType { get; }
        
        internal TypeMap(Type type, Type idAttribute)
        {
            bool IsIdentifier(string fieldOrPropName, MemberInfo mi)
            {
                const StringComparison sc = StringComparison.InvariantCultureIgnoreCase;
                return
                    fieldOrPropName.Equals("Id", sc)
                    || fieldOrPropName.Equals("Id" + type.Name, sc)
                    || fieldOrPropName.Equals(type.Name + "Id", sc)
                    || mi.GetCustomAttribute(idAttribute, true) != null;
            }

            IsValueType = type.IsValueType;

            if (!IsValueType)
            {
                // determine constructor to use
                var ctors = type.GetConstructors();

                ConstructorInfo defaultConstructor = null;
                ConstructorInfo maxParamsConstructor = null;

                var allPropsAndFields = type
                    .GetProperties()
                    .Select(x => (MemberInfo) x)
                    .Concat(type.GetFields())
                    .ToArray();

                var idx = -1;
                foreach (var ctor in ctors
                    .OrderByDescending(x => x.GetParameters().Length))
                {
                    idx++;

                    var allParams = ctor.GetParameters();
                    if (allParams.Length == 0)
                    {
                        defaultConstructor = ctor;
                        break;
                    }

                    var countUsedParams = 0;
                    var canUse = true;

                    foreach (var param in allParams)
                    {
                        if (allPropsAndFields
                            .Any(x => x.Name
                                .Equals(param.Name, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            countUsedParams++;
                        }
                        else
                        {
                            canUse = false;
                        }
                    }

                    if (!canUse) continue;

                    if (maxParamsConstructor == null)
                    {
                        maxParamsConstructor = ctor;
                    }
                    else if (maxParamsConstructor.GetParameters().Length < countUsedParams)
                    {
                        maxParamsConstructor = ctor;
                    }
                }

                if (maxParamsConstructor == null)
                {
                    ConstructorToUse = defaultConstructor;
                }
                else if (maxParamsConstructor.GetParameters().Length == allPropsAndFields.Length
                         || defaultConstructor == null)
                {
                    ConstructorToUse = maxParamsConstructor;
                }
                else
                {
                    throw new ArgumentException("Could not determine constructor to use.");
                }
            }

            FieldsAndProps = GetFields(type)
                .Select(x => x.Name)
                .Concat(GetProperties(type)
                    .Select(x => x.Name))
                .ToArray();

            IdentifierFieldsAndProps =
                GetFields(type)
                    .Where(x => IsIdentifier(x.Name, x))
                    .Select(x => x.Name)
                    .Concat(GetProperties(type)
                        .Where(x => IsIdentifier(x.Name, x))
                        .Select(x => x.Name))
                    .ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetFields();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal IEnumerable<PropertyInfo> GetProperties(Type type, bool all = false)
        {
            var param = ConstructorToUse?.GetParameters(); 
            if (param != null && param.Length > 0)
            {
                return type
                    .GetProperties()
                    .Where(x => x.CanWrite 
                                | param.Any(p => 
                                    p.Name.Equals(
                                        x.Name,
                                        StringComparison.InvariantCultureIgnoreCase)));
            }

            return type
                .GetProperties()
                .Where(x => x.CanWrite);
        }
    }
}

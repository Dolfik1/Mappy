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
        internal IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type
                .GetProperties()
                .Where(x => x.CanWrite);
        }
    }
}

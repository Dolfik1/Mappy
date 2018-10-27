using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Mappy
{
    public class TypeMap
    {
        internal HashSet<string> FieldsAndProps { get; }
        internal HashSet<string> IdentifierFieldsAndProps { get; }

        internal TypeMap(Type type)
        {
            IdentifierFieldsAndProps = new HashSet<string>();
            FieldsAndProps = new HashSet<string>();

            bool IsIdentifier(string fieldOrPropName, Type fieldOrPropType, MemberInfo mi)
            {
                const StringComparison sc = StringComparison.InvariantCultureIgnoreCase;
                return
                    fieldOrPropName.Equals("Id", sc)
                    || fieldOrPropName.Equals("Id" + type.Name, sc)
                    || fieldOrPropName.Equals(type.Name + "Id", sc)
                    || mi.GetCustomAttribute<IdAttribute>(true) != null;
            }

            foreach (var field in type.GetFields())
            {
                FieldsAndProps.Add(field.Name);
                if (IsIdentifier(field.Name, field.FieldType, field))
                {
                    IdentifierFieldsAndProps.Add(field.Name);
                }
            }

            foreach (var prop in type.GetProperties())
            {
                FieldsAndProps.Add(prop.Name);
                if (IsIdentifier(prop.Name, prop.PropertyType, prop))
                {
                    IdentifierFieldsAndProps.Add(prop.Name);
                }
            }
        }
    }
}

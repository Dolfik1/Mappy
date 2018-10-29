﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mappy
{
    public class TypeMap
    {
        internal HashSet<string> FieldsAndProps { get; }
        internal HashSet<string> IdentifierFieldsAndProps { get; }

        internal TypeMap(Type type, Type idAttribute)
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
                    || mi.GetCustomAttribute(idAttribute, true) != null;
            }

            foreach (var field in GetFields(type))
            {
                FieldsAndProps.Add(field.Name);
                if (IsIdentifier(field.Name, field.FieldType, field))
                {
                    IdentifierFieldsAndProps.Add(field.Name);
                }
            }

            foreach (var prop in GetProperties(type))
            {
                FieldsAndProps.Add(prop.Name);
                if (IsIdentifier(prop.Name, prop.PropertyType, prop))
                {
                    IdentifierFieldsAndProps.Add(prop.Name);
                }
            }
        }

        internal IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetFields();
        }

        internal IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type
                .GetProperties()
                .Where(x => x.CanWrite);
        }

        internal static int CalculateHashCode(Type type, Type idAttribute)
        {
            return Utils.HashCode.CombineHashCodes(
                   type.GetHashCode(),
                   idAttribute.GetHashCode());
        }
    }
}

using Mappy.Utils;
using System;
using System.Collections.Generic;

namespace Mappy.Comparers
{
    internal struct IdentifierComparer : IEqualityComparer<IDictionary<string, object>>
    {
        private string[] _identifierFieldsAndProps { get; }

        public IdentifierComparer(string prefix, string[] identifierFieldsAndProps)
        {
            _identifierFieldsAndProps = new string[identifierFieldsAndProps.Length];
            for (var i = 0; i < identifierFieldsAndProps.Length; i++)
            {
                _identifierFieldsAndProps[i] = prefix + identifierFieldsAndProps[i];
            }
        }

        public bool Equals(IDictionary<string, object> x, IDictionary<string, object> y)
        {
            if (_identifierFieldsAndProps.Length == 0)
            {
                return false;
            }

            foreach (var id in _identifierFieldsAndProps)
            {
                x.TryGetValue(id, out var valueX);
                y.TryGetValue(id, out var valueY);

                if (!Equals(valueX, valueY))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(IDictionary<string, object> obj)
        {
            if (_identifierFieldsAndProps.Length == 0)
            {
                return Guid.NewGuid().GetHashCode();
            }

            var ids = _identifierFieldsAndProps;
            if (ids.Length > 8)
            {
                IEnumerable<int> HashCodes()
                {
                    foreach (var id in ids)
                    {
                        if (!obj.TryGetValue(id, out var value))
                        {
                            continue;
                        }

                        yield return value?.GetHashCode() ?? 0;
                    }
                }

                return HashCode.CombineHashCodes(HashCodes());
            }

            var hashCode = 0;
            foreach (var id in ids)
            {
                if (!obj.TryGetValue(id, out var value))
                {
                    continue;
                }

                var hc = value?.GetHashCode() ?? 0;

                if (hashCode == 0)
                {
                    hashCode = hc;
                }
                else
                {
                    HashCode.CombineHashCodes(hc, hashCode);
                }
            }

            return hashCode;
        }
    }
}

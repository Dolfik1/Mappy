using Mappy.Utils;
using System.Collections.Generic;
using ComparerType = System.ValueTuple<System.Type, System.Type>;

namespace Mappy.Comparers
{
    internal struct TypeMapCacheComparer : IEqualityComparer<ComparerType>
    {
        public bool Equals(ComparerType x, ComparerType y)
        {
            if (x.Item1 != y.Item1
                || x.Item2 != y.Item2)
            {
                return false;
            }

            return true;
        }

        public int GetHashCode(ComparerType obj)
        {
            return HashCode.CombineHashCodes(obj.Item1.GetHashCode(), obj.Item2.GetHashCode());
        }
    }
}

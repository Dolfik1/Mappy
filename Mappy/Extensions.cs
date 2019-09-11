using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Mappy
{
    public static class Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> AsList<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is List<T> list) return list;
            return enumerable.ToList();
        }
    }
}

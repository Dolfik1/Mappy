using System.Collections.Generic;
using System.Linq;

namespace Mappy.Utils
{
    internal static class HashCode
    {

        internal static int CombineHashCodes(params int[] hashCodes)
        {
            return CombineHashCodes(hashCodes.AsEnumerable());
        }

        internal static int CombineHashCodes(IEnumerable<int> hashCodes)
        {
            var hash1 = (5381 << 16) + 5381;
            var hash2 = hash1;

            var i = 0;
            foreach (var hashCode in hashCodes)
            {
                if (i % 2 == 0)
                    hash1 = ((hash1 << 5) + hash1 + (hash1 >> 27)) ^ hashCode;
                else
                    hash2 = ((hash2 << 5) + hash2 + (hash2 >> 27)) ^ hashCode;

                ++i;
            }

            return hash1 + (hash2 * 1566083941);
        }
    }
}

using Mappy.Utils;
using System.Collections.Concurrent;

namespace Mappy
{
    public class MappyCache : IMappyCache
    {
        private readonly ConcurrentDictionary<int, TypeMap> _typeMaps
            = new ConcurrentDictionary<int, TypeMap>();

        public TypeMap<T> GetOrCreateTypeMap<T>(MappyOptions options)
        {
            var type = typeof(T);
            var hash = HashCode.CombineHashCodes(
                type.GetHashCode(),
                options.GetHashCode());

            return _typeMaps.GetOrAdd(hash, _ => new TypeMap<T>(options))
                as TypeMap<T>;
        }
    }
}

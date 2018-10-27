using System.Collections.Concurrent;

namespace Mappy
{
    public class MappyCache : IMappyCache
    {
        private readonly ConcurrentDictionary<int, TypeMap> _typeMaps
            = new ConcurrentDictionary<int, TypeMap>();

        public TypeMap<T> GetOrCreateTypeMap<T>()
        {
            var hash = typeof(T).GetHashCode();

            return _typeMaps.GetOrAdd(hash, _ => new TypeMap<T>())
                as TypeMap<T>;
        }
    }
}

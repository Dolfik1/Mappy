using System.Collections.Concurrent;

namespace Mappy
{
    public class MappyCache : IMappyCache
    {
        private readonly ConcurrentDictionary<int, TypeMap> _typeMaps
            = new ConcurrentDictionary<int, TypeMap>();

        public TypeMap<T> GetOrCreateTypeMap<T>(MappyOptions options)
        {
            var hash = TypeMap.CalculateHashCode(typeof(T), options.IdAttributeType);
            return _typeMaps.GetOrAdd(hash, _ => new TypeMap<T>(options.IdAttributeType))
                as TypeMap<T>;
        }
    }
}

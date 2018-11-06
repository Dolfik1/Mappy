using Mappy.Comparers;
using System;
using System.Collections.Concurrent;

namespace Mappy
{
    public class MappyCache : IMappyCache
    {
        private readonly ConcurrentDictionary<(Type, Type), TypeMap> _typeMaps
            = new ConcurrentDictionary<(Type, Type), TypeMap>(
                new TypeMapCacheComparer());

        public TypeMap<T> GetOrCreateTypeMap<T>(MappyOptions options)
        {
            return _typeMaps.GetOrAdd(
                (typeof(T), options.IdAttributeType),
                _ => new TypeMap<T>(options.IdAttributeType))
                as TypeMap<T>;
        }
    }
}

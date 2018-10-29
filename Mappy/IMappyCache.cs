namespace Mappy
{
    public interface IMappyCache
    {
        TypeMap<T> GetOrCreateTypeMap<T>(MappyOptions options);
    }
}

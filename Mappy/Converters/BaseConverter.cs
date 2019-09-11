namespace Mappy.Converters
{
    public struct BaseConverter : ITypeConverter
    {
        public T Convert<T>(object value)
        {
            if (value == null) return default(T);

            var type = typeof(T);
            if (value.GetType() != type)
            {
                return (T)System.Convert.ChangeType(value, type);
            }
            return (T)value;
        }

        public bool CanConvert<T>(object value)
        {
            return true;
        }

        public int Order => int.MaxValue;
    }
}
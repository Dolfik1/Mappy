namespace Mappy.Converters
{
    public struct BaseConverter : ITypeConverter
    {
        public T Convert<T>(object value)
        {
            if (value == null) return default(T);
            
            var tp = typeof(T);
            if (value.GetType() != tp)
            {
                return (T)System.Convert.ChangeType(value, tp);
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
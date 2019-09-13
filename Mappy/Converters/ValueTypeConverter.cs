using System;

namespace Mappy.Converters
{
    /// <summary>
    /// Converts values types.
    /// </summary>
    public struct ValueTypeConverter : ITypeConverter
    {
        #region Implementation of ITypeConverter

        /// <summary>
        /// Converts the given value to the requested type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type the value is to be converted to.</param>
        /// <returns>Converted value.</returns>

        public T Convert<T>(object value)
        {
            if (value == null)
            {
                return default(T);
            }

            var type = typeof(T);
            var convertedValue = System.Convert.ChangeType(value, type);
            return (T)convertedValue;
        }

        /// <summary>
        /// Indicates whether it can convert the given value to the requested type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type the value needs to be converted to.</param>
        /// <returns>Boolean response.</returns>

        public bool CanConvert<T>(object value)
        {
            var type = typeof(T);
            return type.IsValueType && !type.IsEnum && type != typeof(Guid);
        }

        /// <summary>
        /// Order to execute an <see cref="ITypeConverter"/> in.
        /// </summary>
        public int Order => 1000;

        #endregion
    }
}
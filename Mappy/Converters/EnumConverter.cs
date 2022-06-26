using System;

namespace Mappy.Converters
{
    /// <summary>
    /// Converts values to Enums.
    /// </summary>
    public struct EnumConverter : ITypeConverter
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
                return default;
            }

            var type = typeof(T);
            return (T)Enum.Parse(type, value.ToString());
        }

        /// <summary>
        /// Indicates whether it can convert the given value to the requested type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type the value needs to be converted to.</param>
        /// <returns>Boolean response.</returns>

        public bool CanConvert<T>(object value)
        {
            return typeof(T).IsEnum;
        }

        /// <summary>
        /// Order to execute an <see cref="ITypeConverter"/> in.
        /// </summary>
        public int Order => 100;

        #endregion
    }
}
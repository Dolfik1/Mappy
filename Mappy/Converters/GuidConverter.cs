using System;
using System.Runtime.CompilerServices;

namespace Mappy.Converters
{
    /// <summary>
    /// Converts values to Guids.
    /// </summary>
    public struct GuidConverter : ITypeConverter
    {
        #region Implementation of ITypeConverter

        /// <summary>
        /// Converts the given value to the requested type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type the value is to be converted to.</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Convert<T>(object value)
        {
            var convertedValue = value;

            if (value is string s)
            {
                convertedValue = new Guid(s);
            }

            if (value is byte[] bytes)
            {
                convertedValue = new Guid(bytes);
            }

            return (T)convertedValue;
        }

        /// <summary>
        /// Indicates whether it can convert the given value to the requested type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="type">Type the value needs to be converted to.</param>
        /// <returns>Boolean response.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanConvert<T>(object value)
        {
            return typeof(T) == typeof(Guid);
        }

        /// <summary>
        /// Order to execute an <see cref="ITypeConverter"/> in.
        /// </summary>
        public int Order => 100;

        #endregion
    }
}
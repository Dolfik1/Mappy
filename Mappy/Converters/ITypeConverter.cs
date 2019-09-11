using System.Runtime.CompilerServices;

namespace Mappy.Converters
{
    /// <summary>
    /// Defines methods that can convert values from one type to another. 
    /// </summary>
    public interface ITypeConverter
    {
        /// <summary>
        /// Converts the given value to the requested type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <typeparam name="T">Type the value is to be converted to.</typeparam>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        T Convert<T>(object value);

        /// <summary>
        /// Indicates whether it can convert the given value to the requested type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <typeparam name="T">Type the value needs to be converted to.</typeparam>
        /// <returns>Boolean response.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool CanConvert<T>(object value);

        /// <summary>
        /// Order to execute an <see cref="ITypeConverter"/> in.
        /// </summary>
        int Order { get; }
    }
}
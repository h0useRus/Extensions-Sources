using System;

namespace NSW.Extensions.Internal
{
    /// <summary>
    /// Allow to wrap an object and use lambda function to dynamically implement ToString() from its’ properties
    /// </summary>
    /// <typeparam name="T">Input value type</typeparam>
    /// <example>
    /// <code>
    /// // Create stringable container
    /// Stringable&lt;CustomerModel&gt; customerItem = new Stringable&lt;CustomerModel&gt;(customer, c =&gt; c.Name);
    /// var toString = customerItem.ToString(); // return string stored in Name property
    /// // Get container's value
    /// CustomerModel selCustomer = (cbo.SelectedItem as Stringable&lt;CustomerModel&gt;).GetValue();
    /// </code>
    /// </example>
    internal class Stringable<T>
    {
        private readonly T _value;
        private readonly Func<T, string> _convertFn;
        /// <summary>
        /// Create <see cref="Stringable{T}"/> container
        /// </summary>
        /// <param name="value">Input value</param>
        /// <param name="convertFn">Lambda function to dynamically implement ToString()</param>
        public Stringable(T value, Func<T, string> convertFn)
        {
            _value = value;
            _convertFn = convertFn;
        }
        /// <summary>
        /// Get original value
        /// </summary>
        /// <returns>Original value</returns>
        public T GetValue() => _value;
        /// <inheritdoc />
        public override string ToString() => _convertFn(_value);
    }
}

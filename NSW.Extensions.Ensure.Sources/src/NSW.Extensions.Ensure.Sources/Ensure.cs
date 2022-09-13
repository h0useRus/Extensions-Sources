using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NSW.Extensions.Internal
{
    /// <summary>
    /// Helper class that will throw exceptions when conditions are not satisfied.
    /// </summary>
    internal static class Ensure
    {
        private static readonly Lazy<Argument> _argument = new();
        /// <summary>
        /// Ensures that the given expression is true
        /// </summary>
        /// <exception cref="Exception">Exception thrown if false condition</exception>
        /// <param name="condition">Condition to test/ensure</param>
        /// <param name="message">Message for the exception</param>
        /// <exception cref="Exception">Thrown when <paramref name="condition"/> is false</exception>
        [DebuggerStepThrough]
        public static void That(bool condition, string? message = null) => That<Exception>(condition, message);
        /// <summary>
        /// Ensures that the given expression is true
        /// </summary>
        /// <typeparam name="TException">Type of exception to throw</typeparam>
        /// <param name="condition">Condition to test/ensure</param>
        /// <param name="message">Message for the exception</param>
        /// <exception cref="Exception">Thrown when <paramref name="condition"/> is false</exception>
        /// <remarks><typeparamref name="TException"/> must have a constructor that takes a single string</remarks>
        [DebuggerStepThrough]
        public static void That<TException>(bool condition, string? message = null) where TException : Exception
        {
            if (!condition)
                throw ((TException)Activator.CreateInstance(typeof(TException), message ?? string.Empty)!)!;
        }
        /// <summary>
        /// Ensures given condition is false
        /// </summary>
        /// <typeparam name="TException">Type of exception to throw</typeparam>
        /// <param name="condition">Condition to test</param>
        /// <param name="message">Message for the exception</param>
        /// <exception cref="Exception">Thrown when <paramref name="condition"/> is true</exception>
        /// <remarks><typeparamref name="TException"/> must have a constructor that takes a single string</remarks>
        [DebuggerStepThrough]
        public static void Not<TException>(bool condition, string? message = null) where TException : Exception => That<TException>(!condition, message);
        /// <summary>
        /// Ensures given condition is false
        /// </summary>
        /// <param name="condition">Condition to test</param>
        /// <param name="message">Message for the exception</param>
        /// <exception cref="Exception">Thrown when <paramref name="condition"/> is true</exception>
        [DebuggerStepThrough]
        public static void Not(bool condition, string? message = null) => Not<Exception>(condition, message);
        /// <summary>
        /// Ensures given object is not null
        /// </summary>
        /// <param name="value">Value of the object to test for null reference</param>
        /// <param name="message">Message for the Null Reference Exception</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="value"/> is null</exception>
        [DebuggerStepThrough]
        public static void NotNull(object? value, string? message = "Value must be not null.") => That<NullReferenceException>(value != null, message);
        /// <summary>
        /// Ensures given objects are equal
        /// </summary>
        /// <typeparam name="T">Type of objects to compare for equality</typeparam>
        /// <param name="left">First Value to Compare</param>
        /// <param name="right">Second Value to Compare</param>
        /// <param name="message">Message of the exception when values equal</param>
        /// <exception cref="Exception">Exception is thrown when <c>left</c> not equal to <c>right</c></exception>
        /// <remarks>Null values will cause an exception to be thrown</remarks>
        [DebuggerStepThrough]
        public static void Equal<T>(T? left, T? right, string? message = "Values must be equal.") => That(Equals(left, right), message);
        /// <summary>
        /// Ensures given objects are not equal
        /// </summary>
        /// <typeparam name="T">Type of objects to compare for equality</typeparam>
        /// <param name="left">First Value to Compare</param>
        /// <param name="right">Second Value to Compare</param>
        /// <param name="message">Message of the exception when values equal</param>
        /// <exception cref="Exception">Thrown when <c>left</c> equal to <c>right</c></exception>
        /// <remarks>Null values will cause an exception to be thrown</remarks>
        [DebuggerStepThrough]
        public static void NotEqual<T>(T? left, T? right, string? message = "Values must not be equal.") => That(!Equals(left, right), message);
        /// <summary>
        /// Ensures given collection contains a value that satisfied a predicate
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to test</param>
        /// <param name="predicate">Predicate where one value in the collection must satisfy</param>
        /// <param name="message">Message of the exception if value not found</param>
        /// <exception cref="Exception">
        ///     Thrown if collection is null, empty or doesn't contain a value that satisfies <c>predicate</c>
        /// </exception>
        [DebuggerStepThrough]
        public static void Contains<T>(IEnumerable<T>? collection, Func<T, bool> predicate, string? message = null) => That(collection != null && collection.Any(predicate), message);
        /// <summary>
        /// Ensures ALL items in the given collection satisfy a predicate
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to test</param>
        /// <param name="predicate">Predicate that ALL values in the collection must satisfy</param>
        /// <param name="message">Message of the exception if not all values are valid</param>
        /// <exception cref="Exception">
        ///     Thrown if collection is null, empty or not all values satisfies <c>predicate</c>
        /// </exception>
        [DebuggerStepThrough]
        public static void Items<T>(IEnumerable<T>? collection, Func<T, bool> predicate, string? message = null) => That(collection != null && collection.All(predicate), message);
        /// <summary>
        /// Ensures given string is not null or empty
        /// </summary>
        /// <param name="value">String value to compare</param>
        /// <param name="message">Message of the exception if value is null or empty</param>
        /// <exception cref="Exception">string value is null or empty</exception>
        [DebuggerStepThrough]
        public static void NotNullOrEmpty(string? value, string? message = "String cannot be null or empty.") => That(!string.IsNullOrEmpty(value), message);
        /// <summary>
        /// Ensures given string is not null or white space
        /// </summary>
        /// <param name="value">String value to compare</param>
        /// <param name="message">Message of the exception if value is null or empty</param>
        /// <exception cref="Exception">string value is null or white space</exception>
        [DebuggerStepThrough]
        public static void NotNullOrWhiteSpace(string? value, string? message = "String cannot be null or white space.") => That(!string.IsNullOrWhiteSpace(value), message);
        /// <summary>
        /// Argument-specific ensure methods
        /// </summary>
        public static Argument Argument => _argument.Value;
    }

    /// <summary>
    /// Argument-specific ensure methods
    /// </summary>
    internal class Argument
    {
        /// <summary>
        /// Ensures given condition is true
        /// </summary>
        /// <param name="condition">Condition to test</param>
        /// <param name="message">Message of the exception if condition fails</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <c>condition</c> is false
        /// </exception>
        [DebuggerStepThrough]
        public void Is(bool condition, string? message = null) => Ensure.That<ArgumentException>(condition, message);
        /// <summary>
        /// Ensures given condition is false
        /// </summary>
        /// <param name="condition">Condition to test</param>
        /// <param name="message">Message of the exception if condition is true</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <c>condition</c> is true
        /// </exception>
        [DebuggerStepThrough]
        public void IsNot(bool condition, string? message = null) => Is(!condition, message);
        /// <summary>
        /// Check argument for specific type
        /// </summary>
        /// <typeparam name="T">Type for check</typeparam>
        /// <param name="value">Value to test</param>
        /// <param name="paramName">Name of the parameter in the method</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <c>value</c> is not <typeparamref name="T"/> type
        /// </exception>
        [DebuggerStepThrough]
        public void IsType<T>(object? value, string? paramName)
        {
            if(value is not T)
                throw new ArgumentException($"Value must be {typeof(T).FullName}.", paramName);
        }
        /// <summary>
        /// Check argument for specific type and return type cast result
        /// </summary>
        /// <typeparam name="T">Type for check</typeparam>
        /// <param name="value">Value to test</param>
        /// <param name="paramName">Name of the parameter in the method</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <c>value</c> is not <typeparamref name="T"/> type
        /// </exception>
        /// <returns>Return object cast to <typeparamref name="T"/> type</returns>
        [DebuggerStepThrough]
        public T Type<T>(object? value, string? paramName)
        {
            IsType<T>(value, paramName);
            return (T)value!;
        }
        /// <summary>
        /// Ensures given value is not null
        /// </summary>
        /// <param name="value">Value to test for null</param>
        /// <param name="paramName">Name of the parameter in the method</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <c>value</c> is null
        /// </exception>
        [DebuggerStepThrough]
        public void NotNull(object? value, string? paramName) => Ensure.That<ArgumentNullException>(value != null, paramName);
        /// <summary>
        /// Ensures the given <see cref="Guid"/> value is not <see cref="Guid.Empty"/>
        /// </summary>
        /// <param name="value">Value to test for <see cref="Guid.Empty"/></param>
        /// <param name="paramName">Name of the parameter in the method</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <c>value</c> is <see cref="Guid.Empty"/>
        /// </exception>
        [DebuggerStepThrough]
        public void NotEmpty(Guid value, string? paramName)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"Value cannot be {Guid.Empty}.", paramName);
            }
        }
        /// <summary>
        /// Ensures the given <see cref="Guid"/> value is not <see cref="Guid.Empty"/>
        /// </summary>
        /// <param name="value">Value to test for <see cref="Guid.Empty"/></param>
        /// <param name="paramName">Name of the parameter in the method</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <c>value</c> is <see cref="Guid.Empty"/>
        /// </exception>
        [DebuggerStepThrough]
        public void NotEmpty(Guid? value, string? paramName)
        {
            if (!value.HasValue || value == Guid.Empty)
            {
                throw new ArgumentException($"Value cannot be {Guid.Empty}.", paramName);
            }
        }
        /// <summary>
        /// Ensures the given string value is not null or empty
        /// </summary>
        /// <param name="value">Value to test for null or empty</param>
        /// <param name="paramName">Name of the parameter in the method</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <c>value</c> is null or empty string
        /// </exception>
        [DebuggerStepThrough]
        public void NotNullOrEmpty(string? value, string? paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(paramName, "String cannot be null or empty.");
            }
        }
        /// <summary>
        /// Ensures the given string value is not null or empty
        /// </summary>
        /// <param name="value">Value to test for null or empty</param>
        /// <param name="paramName">Name of the parameter in the method</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <c>value</c> is null or empty string
        /// </exception>
        [DebuggerStepThrough]
        public void NotNullOrWhiteSpace(string? value, string? paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(paramName, "String cannot be null or white space.");
            }
        }

        /// <summary>
        /// Ensures given enumerator is empty, but not null
        /// </summary>
        /// <typeparam name="T">Enumerator object type</typeparam>
        /// <param name="enumerable">Enumerator</param>
        /// <param name="paramName">Name of the parameter in the method</param>
        [DebuggerStepThrough]
        public void IsEmpty<T>(IEnumerable<T>? enumerable, string? paramName)
        {
            if(enumerable == null)
                throw new ArgumentNullException(paramName);
            if(enumerable.Any())
                throw new ArgumentException("Value must be empty enumerable.", paramName);
        }

        /// <summary>
        /// Ensures given enumerator is null or empty
        /// </summary>
        /// <typeparam name="T">Enumerator object type</typeparam>
        /// <param name="enumerable">Enumerator</param>
        /// <param name="paramName">Name of the parameter in the method</param>
        [DebuggerStepThrough]
        public void IsNullOrEmpty<T>(IEnumerable<T>? enumerable, string? paramName)
        {
            if(enumerable!= null && enumerable.Any())
                throw new ArgumentException("Value must be null or empty enumerable.", paramName);
        }
        /// <summary>
        /// Ensures given enumerator is not null or empty
        /// </summary>
        /// <typeparam name="T">Enumerator object type</typeparam>
        /// <param name="enumerable">Enumerator</param>
        /// <param name="paramName">Name of the parameter in the method</param>
        [DebuggerStepThrough]
        public void NotEmpty<T>(IEnumerable<T>? enumerable, string? paramName) => Ensure.That<ArgumentNullException>(enumerable != null && enumerable.Any(), paramName);
        /// <summary>
        /// Ensures given parameter not null
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="selector">Value selector</param>
        [DebuggerStepThrough]
        public void NotNull<T>(Expression<Func<T>> selector)
        {
            var memberSelector = (MemberExpression) selector.Body;
            var constantSelector = (ConstantExpression) memberSelector.Expression!;
            var value = ((FieldInfo) memberSelector.Member).GetValue(constantSelector.Value);

            if (value != null) return;

            var name = memberSelector.Member.Name;
            throw new ArgumentNullException(name);
        }
    }
}

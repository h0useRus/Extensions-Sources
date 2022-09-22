using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NSW.Extensions.Internal
{
    /// <summary> Fast property accessor </summary>
    internal class FastProperty
    {
        private readonly Func<object, object>? _getDelegate;
        private readonly Action<object, object?>? _setDelegate;
        /// <summary>
        /// Property name
        /// </summary>
        public string Name => Property.Name;
        /// <summary>
        /// Reflection <see cref="PropertyInfo"/>
        /// </summary>
        public PropertyInfo Property { get; }
        /// <summary>
        /// Property can set value
        /// </summary>
        public bool CanSet { get; }
        /// <summary>
        /// Property can get value
        /// </summary>
        public bool CanGet { get; }
        /// <summary>
        /// Create <see cref="FastProperty"/>
        /// </summary>
        /// <param name="property">Source property info</param>
        /// <param name="nonPublic">Indicates whether the accessor should be returned if it is non-public.</param>
        public FastProperty(PropertyInfo property, bool nonPublic = false)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            CanGet = FastPropertyHelper.GetGetter(property, nonPublic, out _getDelegate);
            CanSet = FastPropertyHelper.GetSetter(property, nonPublic, out _setDelegate);
        }

        /// <summary>
        /// Get value from property
        /// </summary>
        /// <param name="instance">Source instance</param>
        /// <returns>Value of given property</returns>
        public object Get(object instance) => _getDelegate != null && CanGet ? _getDelegate(instance) : throw new NotSupportedException($"Get {Name} is not supported.");
        /// <summary>
        /// Get value from property
        /// </summary>
        /// <param name="instance">Source instance</param>
        /// <returns>Value of given property</returns>
        public TValue Get<TValue>(object instance) => _getDelegate != null && CanGet ? (TValue)_getDelegate(instance) : throw new NotSupportedException($"Get {Name} is not supported.");

        /// <summary>
        /// Set value to property
        /// </summary>
        /// <param name="instance">Source instance</param>
        /// <param name="value">Value to be set</param>
        public void Set<TValue>(object instance, TValue? value)
        {
            if (_setDelegate == null || !CanSet)
                throw new NotSupportedException($"Set {Name} is not supported.");
            _setDelegate(instance, value);
        }
    }
    /// <summary>
    /// Generic fast property accessor
    /// </summary>
    /// <typeparam name="T">Generic type</typeparam>
    internal class FastProperty<T>
    {
        private readonly Func<T, object>? _getDelegate;
        private readonly Action<T, object?>? _setDelegate;
        /// <summary>
        /// Property name
        /// </summary>
        public string Name => Property.Name;
        /// <summary>
        /// Reflection <see cref="PropertyInfo"/>
        /// </summary>
        public PropertyInfo Property { get; }
        /// <summary>
        /// Property can set value
        /// </summary>
        public bool CanSet { get; }
        /// <summary>
        /// Property can get value
        /// </summary>
        public bool CanGet { get; }
        /// <summary>
        /// Create <see cref="FastProperty"/>
        /// </summary>
        /// <param name="property">Source property info</param>
        /// <param name="nonPublic">Indicates whether the accessor should be returned if it is non-public.</param>
        public FastProperty(PropertyInfo property, bool nonPublic = false)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            CanGet = FastPropertyHelper.GetGetter(property, nonPublic, out _getDelegate);
            CanSet = FastPropertyHelper.GetSetter(property, nonPublic, out _setDelegate);
        }
        /// <summary>
        /// Get value from property
        /// </summary>
        /// <param name="instance">Source instance</param>
        /// <returns>Value of given property</returns>
        public object Get(T instance) => _getDelegate != null && CanGet ? _getDelegate(instance) : throw new NotSupportedException($"Get {Name} is not supported.");

        /// <summary>
        /// Set value to property
        /// </summary>
        /// <param name="instance">Source instance</param>
        /// <param name="value">Value to be set</param>
        public void Set(T instance, object value)
        {
            if (_setDelegate == null || !CanSet)
                throw new NotSupportedException($"Set {Name} is not supported.");
            _setDelegate(instance, value);
        }
        /// <summary>
        /// Get value from property
        /// </summary>
        /// <param name="instance">Source instance</param>
        /// <returns>Value of given property</returns>
        public TValue Get<TValue>(T instance) => _getDelegate != null && CanGet ? (TValue)_getDelegate(instance) : throw new NotSupportedException($"Get {Name} is not supported.");

        /// <summary>
        /// Set value to property
        /// </summary>
        /// <param name="instance">Source instance</param>
        /// <param name="value">Value to be set</param>
        public void Set<TValue>(T instance, TValue value)
        {
            if (_setDelegate == null || !CanSet)
                throw new NotSupportedException($"Set {Name} is not supported.");
            _setDelegate(instance, value);
        }
    }

    internal static class FastPropertyHelper
    {
        public static bool GetSetter(PropertyInfo property, bool nonPublic, out Action<object, object?>? setter)
        {
            setter = null;
            var setMethod = property.GetSetMethod(nonPublic);
            if (property.DeclaringType == null || setMethod == null) return false;

            var instance = Expression.Parameter(typeof(object), "instance");
            var value = Expression.Parameter(typeof(object), "value");
            var instanceCast = (!property.DeclaringType.GetTypeInfo().IsValueType)
                ? Expression.TypeAs(instance, property.DeclaringType)
                : Expression.Convert(instance, property.DeclaringType);
            var valueCast = (!property.PropertyType.GetTypeInfo().IsValueType)
                ? Expression.TypeAs(value, property.PropertyType)
                : Expression.Convert(value, property.PropertyType);
            setter = Expression.Lambda<Action<object, object?>>(
                    Expression.Call(instanceCast, setMethod, valueCast), instance, value)
                .Compile();
            return true;
        }

        public static bool GetSetter<T>(PropertyInfo property, bool nonPublic, out Action<T, object?>? setter)
        {
            setter = null;
            var setMethod = property.GetSetMethod(nonPublic);
            if (setMethod == null) return false;
            var instance = Expression.Parameter(typeof(T), "instance");
            var value = Expression.Parameter(typeof(object), "value");
            var valueCast = !property.PropertyType.GetTypeInfo().IsValueType
                ? Expression.TypeAs(value, property.PropertyType)
                : Expression.Convert(value, property.PropertyType);
            setter = Expression.Lambda<Action<T, object?>>(
                Expression.Call(instance, setMethod, valueCast), instance, value).Compile();
            return true;
        }

        public static bool GetGetter(PropertyInfo property, bool nonPublic, out Func<object, object>? getter)
        {
            getter = null;
            var getMethod = property.GetGetMethod(nonPublic);
            if (property.DeclaringType == null || getMethod == null) return false;

            var instance = Expression.Parameter(typeof(object), "instance");
            var instanceCast = property.DeclaringType.GetTypeInfo().IsValueType
                ? Expression.Convert(instance, property.DeclaringType)
                : Expression.TypeAs(instance, property.DeclaringType);
            getter = Expression.Lambda<Func<object, object>>(
                    Expression.TypeAs(Expression.Call(instanceCast, getMethod),
                        typeof(object)),
                    instance)
                .Compile();
            return true;
        }

        public static bool GetGetter<T>(PropertyInfo property, bool nonPublic, out Func<T, object>? getter)
        {
            getter = null;
            var getMethod = property.GetGetMethod(nonPublic);
            if (getMethod == null) return false;
            var instance = Expression.Parameter(typeof(T), "instance");
            getter = Expression.Lambda<Func<T, object>>(
                    Expression.TypeAs(Expression.Call(instance, getMethod), typeof(object)),
                    instance)
                .Compile();
            return true;
        }
    }
}
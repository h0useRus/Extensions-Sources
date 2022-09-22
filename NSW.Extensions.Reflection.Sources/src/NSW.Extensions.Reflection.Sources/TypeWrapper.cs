using System;
using System.Collections.Generic;
using System.Reflection;

namespace NSW.Extensions.Internal
{
    /// <summary>
    /// Fast <see cref="Type"/> wrapper
    /// </summary>
    internal class TypeWrapper
    {
        private static readonly TypeInfoCache<Accessor> _cache = new();
        private class Accessor
        {
            public Dictionary<string, Action<object, object?>> Setters { get; } = new();
            public Dictionary<string, Func<object, object?>> Getters { get; } = new();
            public Dictionary<string, PropertyInfo> Properties { get; } = new();
        }

        private readonly Accessor _accessor;
        private readonly object _source;

        /// <summary>
        /// Object properties
        /// </summary>
        public Dictionary<string, PropertyInfo> Properties => _accessor.Properties;
        /// <summary>
        /// Create <see cref="TypeWrapper"/>
        /// </summary>
        /// <param name="obj">The wrapped object</param>
        public TypeWrapper(object obj)
        {
            _source = obj;
            var type = obj.GetType();

            _accessor = _cache.GetOrAdd(type.GetTypeInfo(), info =>
            {
                var accessor = new Accessor();
                foreach (var p in info.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    var name = p.Name;
                    // Set
                    if(FastPropertyHelper.GetSetter(p, false, out var setExpression) && setExpression != null)
                        accessor.Setters.Add(name, setExpression);
                    // Get
                    if (FastPropertyHelper.GetGetter(p, false, out var getExpression) && getExpression != null)
                        accessor.Getters.Add(name, getExpression);
                    accessor.Properties.Add(name, p);
                }
                return accessor;
            });
        }
        /// <summary>
        /// Set property value
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="value">The property value</param>
        public bool Set(string name, object value)
        {
            if (!_accessor.Setters.ContainsKey(name)) return false;
            _accessor.Setters[name](_source, value);
            return true;
        }
        /// <summary>
        /// Get property value
        /// </summary>
        /// <param name="name">The property name</param>
        /// <returns>Return property value</returns>
        public object? Get(string name)
            => _accessor.Getters.ContainsKey(name) ? _accessor.Getters[name](_source) : null;
        /// <summary>
        /// Get property value
        /// </summary>
        /// <typeparam name="TValue">The property type</typeparam>
        /// <param name="name">The property name</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>Return property value</returns>
        public TValue? GetOrDefault<TValue>(string name, TValue defaultValue = default!)
            => _accessor.Getters.ContainsKey(name) ? (TValue?)_accessor.Getters[name](_source) : defaultValue;
    }
    /// <summary>
    /// Fast <see cref="Type"/> wrapper
    /// </summary>
    internal class TypeWrapper<T>
    {
        private static readonly TypeInfoCache<Accessor> _cache = new();
        private class Accessor
        {
            public Dictionary<string, Action<T, object>> Setters { get; } = new();
            public Dictionary<string, Func<T, object>> Getters { get; } = new();
            public Dictionary<string, PropertyInfo> Properties { get; } = new();
        }

        private readonly Accessor _accessor;
        private readonly T _source;
        /// <summary>
        /// Object properties
        /// </summary>
        public Dictionary<string, PropertyInfo> Properties => _accessor.Properties;
        /// <summary>
        /// Create <see cref="TypeWrapper"/>
        /// </summary>
        /// <param name="obj">The wrapped object</param>
        public TypeWrapper(T obj)
        {
            _source = obj;
            _accessor = _cache.GetOrAdd(typeof(T).GetTypeInfo(), info =>
            {
                var accessor = new Accessor();
                foreach (var p in info.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    var name = p.Name;
                    // Set
                    if(FastPropertyHelper.GetSetter<T>(p, false, out var setExpression) && setExpression != null)
                        accessor.Setters.Add(name, setExpression);
                    // Get
                    if (FastPropertyHelper.GetGetter<T>(p, false, out var getExpression) && getExpression != null)
                        accessor.Getters.Add(name, getExpression);
                    accessor.Properties.Add(name, p);
                }
                return accessor;
            });
        }
        /// <summary>
        /// Set property value
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="value">The property value</param>
        public bool Set(string name, object value)
        {
            if (!_accessor.Setters.ContainsKey(name)) return false;
            _accessor.Setters[name](_source, value);
            return true;
        }
        /// <summary>
        /// Get property value
        /// </summary>
        /// <param name="name">The property name</param>
        /// <returns>Return property value</returns>
        public object? Get(string name)
            => _source!=null && _accessor.Getters.ContainsKey(name) ? _accessor.Getters[name](_source) : null;
        /// <summary>
        /// Get property value
        /// </summary>
        /// <typeparam name="TValue">The property type</typeparam>
        /// <param name="name">The property name</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>Return property value</returns>
        public TValue? GetOrDefault<TValue>(string name, TValue defaultValue = default!)
            => _source!=null && _accessor.Getters.ContainsKey(name) ? (TValue?)_accessor.Getters[name](_source) : defaultValue;
    }
}
using System.Collections.Concurrent;
using System.Reflection;

namespace NSW.Extensions.Internal
{
    /// <summary>
    /// Cache for <see cref="TypeInfo"/>
    /// </summary>
    internal class TypeInfoCache<TValue> : ConcurrentDictionary<TypeInfo, TValue>
    {

    }
}

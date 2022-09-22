using System.Collections.Generic;
using System.Reflection;

namespace NSW.Extensions.Internal
{
    /// <summary>
    /// Entity properties cache
    /// </summary>
    internal class PropertyCache : TypeInfoCache<Dictionary<string, PropertyInfo>>
    {
    }
}

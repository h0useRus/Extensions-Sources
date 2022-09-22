using System;
using System.Reflection;

namespace NSW.Extensions.Internal
{
    /// <summary>
    /// Gets the values from the AssemblyInfo.cs file for the current executing assembly
    /// </summary>
    /// <example>
    /// <code>
    /// string company = AssemblyInfo.Executing.Company;
    /// string product = AssemblyInfo.Executing.Product;
    /// string copyright = AssemblyInfo.Executing.Copyright;
    /// string trademark = AssemblyInfo.Executing.Trademark;
    /// string title = AssemblyInfo.Executing.Title;
    /// string description = AssemblyInfo.Executing.Description;
    /// string configuration = AssemblyInfo.Executing.Configuration;
    /// string fileVersion = AssemblyInfo.Executing.FileVersion;
    /// Version version = AssemblyInfo.Executing.Version;
    /// string versionFull = AssemblyInfo.Executing.AssemblyVersion;
    /// </code>
    /// </example>
    internal class AssemblyInfo
    {
        /// <summary> Current executing assembly </summary>
        public static AssemblyInfo Executing => new (Assembly.GetExecutingAssembly());
        /// <summary> Current entry assembly </summary>
        public static AssemblyInfo Entry => new (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly());
        /// <summary>
        /// Create <see cref="AssemblyInfo"/>
        /// </summary>
        /// <param name="assembly"></param>
        public AssemblyInfo(Assembly assembly)
        {
            Assembly = assembly;
        }
        /// <summary>
        /// Assembly
        /// </summary>
        public Assembly Assembly { get; }
        /// <summary>
        /// Company
        /// </summary>
        public string Company => GetExecutingAssemblyAttribute<AssemblyCompanyAttribute>(a => a.Company);
        /// <summary>
        /// Product
        /// </summary>
        public string Product => GetExecutingAssemblyAttribute<AssemblyProductAttribute>(a => a.Product);
        /// <summary>
        /// Copyright
        /// </summary>
        public string Copyright => GetExecutingAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright);
        /// <summary>
        /// Trademark
        /// </summary>
        public string Trademark => GetExecutingAssemblyAttribute<AssemblyTrademarkAttribute>(a => a.Trademark);
        /// <summary>
        /// Title
        /// </summary>
        public string Title => GetExecutingAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title);
        /// <summary>
        /// Description
        /// </summary>
        public string Description => GetExecutingAssemblyAttribute<AssemblyDescriptionAttribute>(a => a.Description);
        /// <summary>
        /// Configuration
        /// </summary>
        public string Configuration => GetExecutingAssemblyAttribute<AssemblyConfigurationAttribute>(a => a.Configuration);
        /// <summary>
        /// Culture
        /// </summary>
        public string Culture => GetExecutingAssemblyAttribute<AssemblyCultureAttribute>(a => a.Culture);
        /// <summary>
        /// File Version
        /// </summary>
        public string FileVersion => GetExecutingAssemblyAttribute<AssemblyFileVersionAttribute>(a => a.Version);
        /// <summary>
        /// Version
        /// </summary>
        public Version Version => Assembly.GetName().Version ?? new Version(1,0,0,0);
        /// <summary>
        /// Version Full
        /// </summary>
        public string AssemblyVersion => Version.ToString();

        public static implicit operator Assembly(AssemblyInfo assemblyInfo) => assemblyInfo.Assembly;

        public static implicit operator AssemblyInfo(Assembly assembly) => new (assembly);

        private string GetExecutingAssemblyAttribute<T>(Func<T, string> value) where T : Attribute
        {
            var attr = Assembly.GetCustomAttribute<T>();
            return attr != null ? value.Invoke(attr) : string.Empty;
        }

        #region Overrides of Object

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is AssemblyInfo other) return Equals(other);
            return false;
        }

        #region Equality members

        protected bool Equals(AssemblyInfo other) => Equals(Assembly, other.Assembly);

        /// <inheritdoc />
        public override int GetHashCode() => (Assembly != null ? Assembly.GetHashCode() : 0);

        #endregion

        #endregion


    }
}
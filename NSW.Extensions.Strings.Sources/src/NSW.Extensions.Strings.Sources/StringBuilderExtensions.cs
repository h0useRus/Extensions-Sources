using System.ComponentModel;
using System.Text;

namespace NSW.Extensions.Internal
{
    /// <summary>
    /// Extensions for StringBuilder
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static partial class StringBuilderExtensions
    {
        /// <summary>
        /// AppendLine version with format string parameters.
        /// </summary>
        public static void AppendLine(this StringBuilder builder, string value, params object[] parameters) 
            => builder.AppendLine(string.Format(value, parameters));

        /// <summary>
        /// Appends the value of the object's System.Object.ToString() method followed by the default line terminator to the end of the current
        /// System.Text.StringBuilder object if a condition is true
        /// </summary>
        /// <param name="sb">The input string builder</param>
        /// <param name="condition">The conditional expression to evaluate.</param>
        /// <param name="value">The input value</param>
        public static StringBuilder AppendLineIf(this StringBuilder sb, bool condition, object value)
        {
            if (condition) sb.AppendLine(value.ToString());
            return sb;
        }

        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains zero or more format items, followed by the default
        /// line terminator to the end of the current System.Text.StringBuilder object if a condition is true
        /// </summary>
        /// <param name="sb">The input string builder</param>
        /// <param name="condition">The conditional expression to evaluate.</param>
        /// <param name="format">The format string</param>
        /// <param name="args">The format string parameters</param>
        public static StringBuilder AppendLineIf(this StringBuilder sb, bool condition, string format, params object[] args)
        {
            if (condition) sb.AppendFormat(format, args).AppendLine();
            return sb;
        }
        /// <summary>
        /// Appends the value of the object's System.Object.ToString() method to the end of the current
        /// System.Text.StringBuilder object if a condition is true
        /// </summary>
        /// <param name="sb">The input string builder</param>
        /// <param name="condition">The conditional expression to evaluate.</param>
        /// <param name="value">The input value</param>
        public static StringBuilder AppendIf(this StringBuilder sb, bool condition, object value)
        {
            if (condition) sb.Append(value);
            return sb;
        }
        /// <summary>
        /// Appends the value of if <paramref name="value"/> is match <paramref name="pattern"/>.
        /// </summary>
        /// <param name="sb">The input string builder</param>
        /// <param name="pattern">
        /// The match pattern. In pattern can be used the following wildcards:
        ///   '?' - any single character
        ///   '*' - zero or more characters
        /// </param>
        /// <param name="value">The input value</param>
        public static StringBuilder AppendIfMatch(this StringBuilder sb, string pattern, string value)
        {
            if (value.MatchPattern(pattern)) sb.Append(value);
            return sb;
        }
        /// <summary>
        /// Appends line the value of if <paramref name="value"/> is match <paramref name="pattern"/>.
        /// </summary>
        /// <param name="sb">The input string builder</param>
        /// <param name="pattern">
        /// The match pattern. In pattern can be used the following wildcards:
        ///   '?' - any single character
        ///   '*' - zero or more characters
        /// </param>
        /// <param name="value">The input value</param>
        public static StringBuilder AppendLineIfMatch(this StringBuilder sb, string pattern, string value)
        {
            if (value.MatchPattern(pattern)) sb.AppendLine(value);
            return sb;
        }
        /// <summary>
        /// Appends the string returned by processing a composite format string, which contains zero or more format items, 
        /// to the end of the current System.Text.StringBuilder object if a condition is true
        /// </summary>
        /// <param name="sb">The input string builder</param>
        /// <param name="condition">The conditional expression to evaluate.</param>
        /// <param name="format">The format string</param>
        /// <param name="args">The format string parameters</param>
        public static StringBuilder AppendFormatIf(this StringBuilder sb, bool condition, string format, params object[] args)
        {
            if (condition) sb.AppendFormat(format, args);
            return sb;
        }
        /// <summary>
        /// Appends the <paramref name="value"/> with provided <paramref name="mask"/>
        /// </summary>
        /// <param name="sb">The input string builder</param>
        /// <param name="mask">The mask for formatting. Like "A##-##-T-###Z"</param>
        /// <param name="value">The input value</param>
        public static StringBuilder AppendMask(this StringBuilder sb, string mask, string value)
            => sb.Append(value.FormatWithMask(mask));
        /// <summary>
        /// Appends line the <paramref name="value"/> with provided <paramref name="mask"/>
        /// </summary>
        /// <param name="sb">The input string builder</param>
        /// <param name="mask">The mask for formatting. Like "A##-##-T-###Z"</param>
        /// <param name="value">The input value</param>
        public static StringBuilder AppendLineMask(this StringBuilder sb, string mask, string value)
            => sb.AppendLine(value.FormatWithMask(mask));
    }
}

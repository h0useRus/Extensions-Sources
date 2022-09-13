using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#nullable enable

namespace NSW.Extensions.Internal
{
    /// <summary>
    /// <see cref="string"/> extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static partial class StringExtensions
    {
        /// <summary>
        /// Crop string
        /// </summary>
        /// <param name="s">Source string</param>
        /// <param name="max">Maximum letters for crop</param>
        /// <param name="cropEnd">Ending string which added to croped strings</param>
        /// <returns>Croped string</returns>
        public static string Crop(this string s, int max, string cropEnd = "…")
        {
            if (!string.IsNullOrEmpty(s) && s.Length > max)
            {
                return !string.IsNullOrEmpty(cropEnd) ? s[..(max - cropEnd.Length)] + cropEnd : s[..max];
            }
            return s;
        }
        /// <summary>
        /// Replace line breaks to specific <paramref name="replacement"/>
        /// </summary>
        /// <param name="s">Source string</param>
        /// <param name="replacement">Replacement string</param>
        /// <returns>Result string</returns>
        public static string ReplaceLineBreaks(this string s, string replacement) => !string.IsNullOrEmpty(s) ? Regex.Replace(s, @"\r\n?|\n", replacement) : s;
        /// <summary>
        /// Safety split strings
        /// </summary>
        /// <param name="s">Source string</param>
        /// <param name="separator">Separator char</param>
        /// <returns>Return slitted string or empty array if string null</returns>
        public static string[] SafeSplit(this string s, char separator) => string.IsNullOrEmpty(s) ? Array.Empty<string>() : s.Split(separator);
        /// <summary>
        /// Replicates an input string n number of times
        /// </summary>
        /// <param name="s">The input string</param>
        /// <param name="count">Amount of copies</param>
        /// <returns>Return result string</returns>
        public static string Replicate(this string s, int count) => new StringBuilder().Insert(0, s, count).ToString();
        /// <summary>
        /// Replicates a character n number of times and returns a string
        /// </summary>
        /// <param name="character">The replicated character</param>
        /// <param name="count">Amount of copies</param>
        /// <returns>Return result string</returns>
        public static string Replicate(this char character, int count) => new(character, count);

        /// <summary>
        /// Check that string <paramref name="toCheck"/> contains in string <paramref name="source"/> with <see cref="StringComparison.OrdinalIgnoreCase"/>
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="toCheck">Checking string</param>
        /// <returns>Return true if string <paramref name="toCheck"/> contains in string <paramref name="source"/></returns>
        public static bool IgnoreCaseContains(this string source, string toCheck)
        {
            if (string.IsNullOrEmpty(source)) return false;
            if (string.IsNullOrEmpty(toCheck)) return false;
            return source.Contains(toCheck, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Fast ignore case compare two strings with <see cref="StringComparison.OrdinalIgnoreCase"/>
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="toCheck">Checking string</param>
        /// <returns>Return true if strings are equal</returns>
        public static bool IgnoreCaseEqual(this string source, string toCheck)
            => string.Compare(source, toCheck, StringComparison.OrdinalIgnoreCase) == 0;
        /// <summary>
        /// Checks string object's value to array of string values
        /// </summary>
        /// <param name="value">Source string</param>
        /// <param name="stringValues">Array of string values to compare</param>
        /// <returns>Return true if any string value matches</returns>
        public static bool In(this string value, params string[] stringValues)
            => stringValues.Any(s => string.CompareOrdinal(value, s) == 0);

        /// <summary>
        /// Remove all white spaces from the string
        /// </summary>
        /// <param name="source">Source string</param>
        /// <returns>String with no whitespace characters</returns>
        public static string RemoveWhitespaces(this string source)
            => string.IsNullOrEmpty(source)
                ? source
                : string.Join("", source.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

        /// <summary>
        /// Returns characters from right of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of characters to return</param>
        /// <returns>Returns string from right</returns>
        public static string Right(this string value, int length)
            => !string.IsNullOrEmpty(value) && value.Length > length
                ? value[^length..]
                : value;

        /// <summary>
        /// Returns characters from left of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of characters to return</param>
        /// <returns>Returns string from left</returns>
        public static string Left(this string value, int length)
            => !string.IsNullOrEmpty(value) && value.Length > length
                ? value[..length]
                : value;
        /// <summary>
        /// To English plural
        /// </summary>
        /// <param name="singular">Singular word</param>
        /// <returns>Plural result</returns>
        public static string ToPlural(this string singular)
        {
            if (string.IsNullOrEmpty(singular)) return singular;
            //sibilant ending rule
            if (singular.EndsWith("sh")) return singular + "es";
            if (singular.EndsWith("ch")) return singular + "es";
            if (singular.EndsWith("us")) return singular + "es";
            if (singular.EndsWith("ss")) return singular + "es";
            //-ies rule
            if (singular.EndsWith("y")) return singular.Remove(singular.Length - 1, 1) + "ies";
            // -oes rule
            if (singular.EndsWith("o")) return singular.Remove(singular.Length - 1, 1) + "oes";
            // -s suffix rule
            return singular + "s";
        }
        /// <summary>
        /// Convert string to Title case variant ignoring all capital words
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Title cased string</returns>
        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var words = input.Split(' ');
            var result = new List<string>();
            foreach (var word in words)
            {
                if (word.Length == 0 || IsAllCapitals(word))
                    result.Add(word);
                else if (word.Length == 1)
                    result.Add(word.ToUpper());
                else
                    result.Add(char.ToUpper(word[0]) + word.Remove(0, 1).ToLower());
            }

            return string.Join(" ", result);
        }
        /// <summary>
        /// Converts snake_tail strings to PascalCase also removing underscores
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Pascal case string</returns>
        public static string ToPascalCase(this string input)
            => string.IsNullOrWhiteSpace(input)
                ? input
                : Regex.Replace(input, "(?:^|_)(.)", match => match.Groups[1].Value.ToUpper());
        /// <summary>
        /// Converts snake_tail strings to camelCase also removing underscores
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Camel case string</returns>
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var word = ToPascalCase(input);
            return word[..1].ToLower() + word[1..];
        }
        /// <summary>
        /// Check input string contains only capital letters
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Return <c>true</c> if string contains only capital letters</returns>
        public static bool IsAllCapitals(this string input)
            => !string.IsNullOrWhiteSpace(input) && input.ToCharArray().All(char.IsUpper);
        /// <summary>
        /// Formats the string according to the specified mask
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="mask">The mask for formatting. Like "A##-##-T-###Z"</param>
        /// <returns>The formatted string</returns>
        public static string FormatWithMask(this string input, string mask)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if (string.IsNullOrEmpty(mask)) return input;

            var output = string.Empty;
            var index = 0;
            foreach (var m in mask)
            {
                if (m == '#')
                {
                    if (index < input.Length)
                    {
                        output += input[index];
                        index++;
                    }
                }
                else
                    output += m;
            }
            return output;
        }
        /// <summary>
        /// Method to compare Strings with wildcard characters.
        ///
        ///In pattern can be used the following wildcards:
        ///
        ///   '?' - any single character
        ///   '*' - zero or more characters
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="pattern">The input wildcard pattern</param>
        /// <returns>Return true if input string matched pattern</returns>
        public static bool MatchPattern(this string input, string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return false;

            var regexPattern = pattern.Aggregate("^", (current, c) => current + c switch
            {
                '*' => ".*",
                '?' => ".",
                _ => "[" + c + "]"
            });
            return new Regex(regexPattern + "$").IsMatch(input);
        }
        /// <summary>
        /// Concatenates the specified string value with the passed additional strings.
        /// </summary>
        /// <param name = "value">The original value.</param>
        /// <param name = "values">The additional string values to be concatenated.</param>
        /// <returns>The concatenated string.</returns>
        public static string ConcatWith(this string value, params string[] values)
            => string.Concat(value, string.Concat(values));
        /// <summary>
        /// Return ordinal number string
        /// </summary>
        /// <param name="number">Input numeric value</param>
        /// <returns>Return ordinal number string</returns>
        public static string ToOrdinal(this int number)
            => (number % 100) switch
            {
                11 or 12 or 13 => number + "th",
                _ => (number % 10) switch
                {
                    1 => number + "st",
                    2 => number + "nd",
                    3 => number + "rd",
                    _ => number + "th"
                },
            };
        /// <summary>
        /// Creates a Stream from a string. Internally creates a memory stream and returns that.
        /// </summary>
        /// <param name="s">The input string</param>
        /// <param name="encoding">String encoding</param>
        /// <returns>Return string as the stream</returns>
        public static Stream ToStream(this string s, Encoding? encoding = null) => new MemoryStream((encoding ?? Encoding.UTF8).GetBytes(s ?? ""));

        #region Slug string

        // white space, em-dash, en-dash, underscore
        private static readonly Regex _wordDelimiters = new(@"[\s—–_]", RegexOptions.Compiled);
        // characters that are not valid
        private static readonly Regex _invalidChars = new (@"[^a-z0-9\-]", RegexOptions.Compiled);
        // multiple hyphens
        private static readonly Regex _multipleHyphens = new (@"-{2,}", RegexOptions.Compiled);
        /// See: http://www.siao2.com/2007/05/14/2629747.aspx
        private static string RemoveDiacritics(string stIn)
        {
            var stFormD = stIn.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            for (var ich = 0; ich < stFormD.Length; ich++)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
        /// <summary>
        /// Slugify string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSlug(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            // convert to lower case
            value = value.ToLowerInvariant();

            // remove diacritics (accents)
            value = RemoveDiacritics(value);

            value = value.Replace("#", "-sharp ")
                .Replace("@", "-at ")
                .Replace("$", "-dollar ")
                .Replace("%", "-percent ")
                .Replace("&", "-and ")
                .Replace("||", "-or ");

            // ensure all word delimiters are hyphens
            value = _wordDelimiters.Replace(value, "-");

            // strip out invalid characters
            value = _invalidChars.Replace(value, "");

            // replace multiple hyphens (-) with a single hyphen
            value = _multipleHyphens.Replace(value, "-");

            // trim hyphens (-) from ends
            return value.Trim('-');
        }
        /// <summary>
        /// Converts a string into a slug that allows segments e.g. <example>.blog/2012/07/01/title</example>.
        /// Normally used to validate user entered slugs.
        /// </summary>
        /// <param name="value">The string value to slugify</param>
        /// <param name="separator">The string separator for slugify</param>
        /// <returns>A URL safe slug with segments.</returns>
        public static string ToSlugWithSegments(this string value, char separator = '/')
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            var segments = value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            var result = segments.Aggregate(string.Empty, (slug, segment) => slug += separator + segment.ToSlug());
            return result.Trim(separator);
        }
        #endregion

        #region ToUrlFriendly
        
        /// <summary>
        /// Converts the specified title so that it is more human and search engine readable e.g.
        /// http://example.com/product/123/this-is-the-seo-and-human-friendly-product-title. Note that the ID of the
        /// product is still included in the URL, to avoid having to deal with two titles with the same name. Search
        /// Engine Optimization (SEO) friendly URL's gives your site a boost in search rankings by including keywords
        /// in your URL's. They are also easier to read by users and can give them an indication of what they are
        /// clicking on when they look at a URL. Refer to the code example below to see how this helper can be used.
        /// Go to definition on this method to see a code example. To learn more about friendly URL's see
        /// https://moz.com/blog/15-seo-best-practices-for-structuring-urls.
        /// To learn more about how this was implemented see
        /// http://stackoverflow.com/questions/25259/how-does-stack-overflow-generate-its-seo-friendly-urls/25486
        /// </summary>
        /// <param name="title">The title of the URL.</param>
        /// <param name="remapToAscii">if set to <c>true</c>, remaps special UTF8 characters like 'è' to their ASCII
        /// equivalent 'e'. All modern browsers except Internet Explorer display the 'è' correctly. Older browsers and
        /// Internet Explorer percent encode these international characters so they are displayed as'%C3%A8'. What you
        /// set this to depends on whether your target users are English speakers or not.</param>
        /// <param name="maxlength">The maximum allowed length of the title.</param>
        /// <returns>The SEO and human friendly title.</returns>
        /// <code>
        /// [HttpGet("product/{id}/{title}", Name = "GetDetails")]
        /// public IActionResult Product(int id, string title)
        /// {
        ///     // Get the product as indicated by the ID from a database or some repository.
        ///     var product = ProductRepository.Find(id);
        ///
        ///     // If a product with the specified ID was not found, return a 404 Not Found response.
        ///     if (product == null)
        ///     {
        ///         return this.HttpNotFound();
        ///     }
        ///
        ///     // Get the actual friendly version of the title.
        ///     var friendlyTitle = product.Title.ToUrlFriendly();
        ///
        ///     // Compare the title with the friendly title.
        ///     if (!string.Equals(friendlyTitle, title, StringComparison.Ordinal))
        ///     {
        ///         // If the title is null, empty or does not match the friendly title, return a 301 Permanent
        ///         // Redirect to the correct friendly URL.
        ///         return this.RedirectToRoutePermanent("GetProduct", new { id = id, title = friendlyTitle });
        ///     }
        ///
        ///     // The URL the client has browsed to is correct, show them the view containing the product.
        ///     return this.View(product);
        /// }
        /// </code>
        public static string ToUrlFriendly(this string title, bool remapToAscii = false, int maxlength = 80)
        {
            if (title == null) return string.Empty;

            var length = title.Length;
            var prevdash = false;
            var stringBuilder = new StringBuilder(length);

            for (var i = 0; i < length; ++i)
            {
                var c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    stringBuilder.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lower-case
                    stringBuilder.Append((char)(c | 32));
                    prevdash = false;
                }
                else if ((c == ' ') || (c == ',') || (c == '.') || (c == '/') ||
                    (c == '\\') || (c == '-') || (c == '_') || (c == '='))
                {
                    if (!prevdash && (stringBuilder.Length > 0))
                    {
                        stringBuilder.Append('-');
                        prevdash = true;
                    }
                }
                else if (c >= 128)
                {
                    var previousLength = stringBuilder.Length;

                    if (remapToAscii)
                    {
                        stringBuilder.Append(RemapInternationalCharToAscii(c));
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }

                    if (previousLength != stringBuilder.Length)
                    {
                        prevdash = false;
                    }
                }

                if (stringBuilder.Length >= maxlength)
                {
                    break;
                }
            }

            if (prevdash || stringBuilder.Length > maxlength)
            {
                return stringBuilder.ToString()[..(stringBuilder.Length - 1)];
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Remaps the international character to their equivalent ASCII characters. See
        /// http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
        /// </summary>
        /// <param name="character">The character to remap to its ASCII equivalent.</param>
        /// <returns>The remapped character</returns>
        private static string RemapInternationalCharToAscii(char character)
        {
            var s = character.ToString().ToLowerInvariant();

            if ("àåáâäãåąā".Contains(s)) return "a";
            if ("òóôõöøőð".Contains(s)) return "o";
            if ("èéêěëę".Contains(s)) return "e";
            if ("ùúûüŭů".Contains(s)) return "u";
            if ("ìíîïı".Contains(s)) return "i";
            if ("śşšŝ".Contains(s)) return "s";
            if ("çćčĉ".Contains(s)) return "c";
            if ("żźž".Contains(s)) return "z";
            if ("ĺľł".Contains(s)) return "l";
            if ("ñń".Contains(s)) return "n";
            if ("ýÿ".Contains(s)) return "y";
            if ("ğĝ".Contains(s)) return "g";
            if ("ŕř".Contains(s)) return "r";
            if ("úů".Contains(s)) return "u";
            if ("đď".Contains(s)) return "d";
            if (character == 'ß') return "ss";
            if (character == 'Þ') return "th";
            if (character == 'ť') return "t";
            if (character == 'ž') return "z";
            if (character == 'ĥ') return "h";
            if (character == 'ĵ') return "j";

            return string.Empty;
        }
        #endregion

        #region Html

        /// <summary>
        /// Strips HTML tags out of an HTML string and returns just the text.
        /// </summary>
        /// <param name="html">Html String</param>
        /// <returns></returns>
        public static string StripHtml(this string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;
            html = Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
            html = html.Replace("\t", " ");
            html = html.Replace("\r\n", string.Empty);
            html = html.Replace("   ", " ");
            return html.Replace("  ", " ");
        }
        /// <summary>
        /// HTML-encodes a string and returns the encoded string.
        /// </summary>
        /// <param name="text">The text string to encode. </param>
        /// <returns>The HTML-encoded text.</returns>
        public static string HtmlEncode(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var sb = new StringBuilder(text.Length);

            var len = text.Length;
            for (var i = 0; i < len; i++)
            {
                switch (text[i])
                {

                    case '<':
                        sb.Append("&lt;");
                        break;
                    case '>':
                        sb.Append("&gt;");
                        break;
                    case '"':
                        sb.Append("&quot;");
                        break;
                    case '&':
                        sb.Append("&amp;");
                        break;
                    default:
                        if (text[i] > 159)
                        {
                            // decimal numeric entity
                            sb.Append("&#");
                            sb.Append(((int)text[i]).ToString(CultureInfo.InvariantCulture));
                            sb.Append(';');
                        }
                        else
                            sb.Append(text[i]);
                        break;
                }
            }
            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// Return human friendly relative time text
        /// </summary>
        /// <param name="from">From date</param>
        /// <param name="to">To date</param>
        /// <returns>Human friendly text</returns>
        public static string ToRelativeTimeString(this DateTime from, DateTime to)
        {
            var ts = new TimeSpan(from.Ticks - to.Ticks);
            var deltaSeconds = Math.Abs(ts.TotalSeconds);

            if (deltaSeconds < 5)
            {
                return "just now";
            }

            var suffix = ts.Ticks < 0 ? "from now" : "ago";

            if (deltaSeconds < 60)
            {
                return $"{Math.Floor(deltaSeconds)} seconds {suffix}";
            }
            if (deltaSeconds < 120)
            {
                return $"a minute {suffix}";
            }

            var deltaMinutes = deltaSeconds / 60.0f;

            if (deltaMinutes < 60)
            {
                return $"{Math.Floor(deltaMinutes)} minutes {suffix}";
            }
            if (deltaMinutes < 120)
            {
                return $"an hour {suffix}";
            }
            if (deltaMinutes < (24 * 60))
            {
                return $"{(int)Math.Floor(deltaMinutes / 60)} hours {suffix}";
            }

            var prefix = ts.Ticks > 0 ? "last" : "next";

            if (deltaMinutes < (24 * 60 * 2))
            {
                return $"{prefix} day";
            }
            if (deltaMinutes < (24 * 60 * 7))
            {
                return $"{(int)Math.Floor(deltaMinutes / (60 * 24))} days {suffix}";
            }
            if (deltaMinutes < (24 * 60 * 14))
            {
                return $"{prefix} week";
            }
            if (deltaMinutes < (24 * 60 * 31))
            {
                return $"{(int)Math.Floor(deltaMinutes / (60 * 24 * 7))} weeks {suffix}";
            }
            if (deltaMinutes < (24 * 60 * 61))
            {
                return $"{prefix} month";
            }
            if (deltaMinutes < (24 * 60 * 365.25))
            {
                return $"{(int)Math.Floor(deltaMinutes / (60 * 24 * 30))} months {suffix}";
            }
            if (deltaMinutes < (24 * 60 * 731))
            {
                return $"{prefix} year";
            }

            return $"{(int)Math.Floor(deltaMinutes / (60 * 24 * 365))} years {suffix}";
        }
    }
}

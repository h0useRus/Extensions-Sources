using System;
using System.Collections;
using NUnit.Framework;

namespace NSW.Extensions.Internal
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestCase("", "", 10)]
        [TestCase(null, null, 10)]
        [TestCase("qwertyuiop", "qwertyuiop", 10)]
        [TestCase("qwertyuiopasdfghjkl", "qwertyuio…", 10)]
        [TestCase("qwertyuiopasdfghjkl", "qwertyu...", 10, "...")]
        public void Crop(string actual, string expected, int cropSize, string cropEnd = "…") => Assert.AreEqual(expected, actual.Crop(cropSize, cropEnd));

        [TestCase("", "", "")]
        [TestCase(null, null, "")]
        [TestCase("qwertyuiop", "qwertyuiop", "")]
        [TestCase("qwer\ntyuiop", "qwertyuiop", "")]
        [TestCase("qwer\rtyuiop", "qwertyuiop", "")]
        [TestCase("qwer\r\ntyuiop", "qwertyuiop", "")]
        [TestCase("qwer\r\ntyuiop", "qwer tyuiop", " ")]
        public void ReplaceLineBreaks(string actual, string expected, string replacement) => Assert.AreEqual(expected, actual.ReplaceLineBreaks(replacement));

        [Test]
        public void SafeSplit()
        {
            string? actual = null;
            CollectionAssert.AreEqual(Array.Empty<string>(), actual!.SafeSplit(','));

            actual = string.Empty;
            CollectionAssert.AreEqual(Array.Empty<string>(), actual.SafeSplit(','));

            actual = "data";
            CollectionAssert.AreEqual(actual.Split(','), actual.SafeSplit(','));

            actual = "data,set";
            CollectionAssert.AreEqual(actual.Split(','), actual.SafeSplit(','));
        }

        [Test]
        public void ReplicateString() => Assert.AreEqual("datadatadatadatadata", "data".Replicate(5));

        [Test]
        public void ReplicateChar() => Assert.AreEqual("aaaaa", 'a'.Replicate(5));

        [TestCase("DataSet", "Set", true)]
        [TestCase("DataSet", "set", true)]
        [TestCase("DataSet", "get", false)]
        [TestCase("DataSet", null, false)]
        [TestCase(null, "set", false)]
        [TestCase(null, null, false)]
        public void IgnoreCaseContains(string actual, string toCheck, bool expected) => Assert.AreEqual(expected, actual.IgnoreCaseContains(toCheck));

        [TestCase(null, null, true)]
        [TestCase("DataSet", "DataSet", true)]
        [TestCase("DataSet", "dataset", true)]
        [TestCase("DataSet", "DataGet", false)]
        [TestCase("DataSet", null, false)]
        [TestCase(null, "set", false)]
        public void IgnoreCaseEqual(string actual, string toCheck, bool expected) => Assert.AreEqual(expected, actual.IgnoreCaseEqual(toCheck));

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("Data\a\b\f\n\r\t\vSet", "Data\a\bSet")]
        [TestCase("\a\b\f\n\r\t\v", "\a\b")]
        [TestCase("\f\n\r\t\v", "")]
        public void RemoveWhitespaces(string actual, string expected) => Assert.AreEqual(expected, actual.RemoveWhitespaces());

        [TestCase(null, 3, null)]
        [TestCase("", 3, "")]
        [TestCase("DataSet", 3, "Set")]
        [TestCase("DataSet", 7, "DataSet")]
        [TestCase("DataSet", 10, "DataSet")]
        public void Right(string actual, int count, string expected) => Assert.AreEqual(expected, actual.Right(count));

        [TestCase(null, 3, null)]
        [TestCase("", 3, "")]
        [TestCase("DataSet", 4, "Data")]
        [TestCase("DataSet", 7, "DataSet")]
        [TestCase("DataSet", 10, "DataSet")]
        public void Left(string actual, int count, string expected) => Assert.AreEqual(expected, actual.Left(count));

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("DataSet", "DataSets")]
        [TestCase("Brunch", "Brunches")]
        [TestCase("Bush", "Bushes")]
        [TestCase("Bus", "Buses")]
        [TestCase("Dolly", "Dollies")]
        [TestCase("Dido", "Didoes")]
        public void ToPlural(string actual, string expected) => Assert.AreEqual(expected, actual.ToPlural());

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("data set", "Data Set")]
        [TestCase("a data set", "A Data Set")]
        [TestCase("a DATA set", "A DATA Set")]
        public void ToTitleCase(string actual, string expected) => Assert.AreEqual(expected, actual.ToTitleCase());

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("data_set", "DataSet")]
        public void ToPascalCase(string actual, string expected) => Assert.AreEqual(expected, actual.ToPascalCase());

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("data_set", "dataSet")]
        public void ToCamelCase(string actual, string expected) => Assert.AreEqual(expected, actual.ToCamelCase());

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("data_set", false)]
        [TestCase("Data Set", false)]
        [TestCase("DATA", true)]
        public void IsAllCapitals(string actual, bool expected) => Assert.AreEqual(expected, actual.IsAllCapitals());

        [TestCase(null, null, null)]
        [TestCase("", "", "")]
        [TestCase("1234567890", "", "1234567890")]
        [TestCase("1234567890", null, "1234567890")]
        [TestCase("1234567890", "A###-B###-C### D#", "A123-B456-C789 D0")]
        [TestCase("1234567890", "A###-B###-C###", "A123-B456-C789")]
        [TestCase("12345678", "A###-B###-C### D#", "A123-B456-C78 D")]
        [TestCase(null, "A###-B###-C### D#", null)]
        public void FormatWithMask(string actual, string mask, string expected) => Assert.AreEqual(expected, actual.FormatWithMask(mask));
        [TestCase("", "*")]
        [TestCase(" ", "?")]
        [TestCase("a", "*")]
        [TestCase("a", "?")]
        [TestCase("a", "*?")]
        [TestCase("a", "?*")]
        [TestCase("a", "*?*")]
        [TestCase("ab", "*")]
        [TestCase("ab", "**")]
        [TestCase("ab", "***")]
        [TestCase("ab", "*b")]
        [TestCase("ab", "a*")]
        [TestCase("abc", "*?")]
        [TestCase("abc", "*??")]
        [TestCase("abc", "??*")]
        [TestCase("abc", "?*?")]
        [TestCase("abc", "???")]
        [TestCase("abc", "?*")]
        [TestCase("abc", "*abc")]
        [TestCase("abc", "*abc*")]
        [TestCase("abc", "*a*b*c*")]
        [TestCase("aXXXbc", "*a*bc*")]
        [TestCase("abc", "abc")]
        public void MatchPatternTrue(string input, string pattern) => Assert.IsTrue(input.MatchPattern(pattern));
        [TestCase("", "*a")]
        [TestCase("", "a*")]
        [TestCase("", "?")]
        [TestCase("a", "*b*")]
        [TestCase("ab", "b*a")]
        [TestCase("ab", "b?a")]
        [TestCase("ab", "a?b")]
        [TestCase("ab", "abc")]
        [TestCase("ab", "cab")]
        [TestCase("abc", "ab")]
        [TestCase("ab", "AB")]
        [TestCase("cab", "ab")]
        [TestCase("a", "??")]
        [TestCase("", "*?")]
        [TestCase("", "?*")]
        [TestCase("abc", "????")]
        [TestCase("a", "??*")]
        [TestCase("abX", "*abc")]
        [TestCase("Xbc", "*abc*")]
        [TestCase("ac", "*a*bc*")]
        public void MatchPatternFalse(string input, string pattern) => Assert.IsFalse(input.MatchPattern(pattern));

        [TestCase(null, "")]
        [TestCase(null, "The quick brown fox jumps over the lazy dog", "The ", "quick ", "brown ", "fox ", "jumps ", "over ", "the ", "lazy ", "dog")]
        [TestCase("The quick brown fox ", "The quick brown fox jumps over the lazy dog", "jumps ", "over ", "the ", "lazy ", "dog")]
        public void ConcatWith(string actual, string expected, params string[] values) => Assert.AreEqual(expected, actual.ConcatWith(values));

        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase("DataSet", 7)]
        public void ToStream(string actual, int expectedLength) => Assert.AreEqual(expectedLength, actual.ToStream().Length);

        [TestCase(1, "1st")]
        [TestCase(42, "42nd")]
        [TestCase(3, "3rd")]
        [TestCase(11, "11th")]
        [TestCase(112, "112th")]
        [TestCase(1113, "1113th")]
        [TestCase(69, "69th")]
        public void ToOrdinal(int actual, string expected) => Assert.AreEqual(expected, actual.ToOrdinal());

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("The quick brown fox jumps over the lazy dog", "the-quick-brown-fox-jumps-over-the-lazy-dog")]
        [TestCase("The price $10 is more than 10% of Rock&Roll song price.", "the-price-dollar-10-is-more-than-10-percent-of-rock-and-roll-song-price")]
        [TestCase("El zorro marrón rápido salta sobre el perro perezoso", "el-zorro-marron-rapido-salta-sobre-el-perro-perezoso")]
        public void ToSlug(string actual, string expected) => Assert.AreEqual(expected, actual.ToSlug());

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("The quick brown fox jumps over the lazy dog", "the-quick-brown-fox-jumps-over-the-lazy-dog")]
        [TestCase("blog/1020/El zorro marrón rápido salta sobre el perro perezoso", "blog/1020/el-zorro-marron-rapido-salta-sobre-el-perro-perezoso")]
        public void ToSlugWithSegments(string actual, string expected) => Assert.AreEqual(expected, actual.ToSlugWithSegments());

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("<!DOCTYPE html>\r\n<html>\r\n<body>\r\n\r\n<h1>My First Heading</h1>\r\n\r\n<p>My first paragraph.</p>\r\n\r\n</body>\r\n</html>", "My First HeadingMy first paragraph.")]
        public void StripHtml(string actual, string expected) => Assert.AreEqual(expected, actual.StripHtml());

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("<!DOCTYPE html>\r\n<html>\r\n<body>\r\n\r\n<h1>My First Heading</h1>\r\n\r\n<p>My first paragraph.</p>\r\n\r\n</body>\r\n</html>", "&lt;!DOCTYPE html&gt;\r\n&lt;html&gt;\r\n&lt;body&gt;\r\n\r\n&lt;h1&gt;My First Heading&lt;/h1&gt;\r\n\r\n&lt;p&gt;My first paragraph.&lt;/p&gt;\r\n\r\n&lt;/body&gt;\r\n&lt;/html&gt;")]
        public void HtmlEncode(string actual, string expected) => Assert.AreEqual(expected, actual.HtmlEncode());

        [TestCaseSource(nameof(ToRelativeTimeStringCases))]
        public string ToRelativeTimeString(DateTime from, DateTime to)
        {
            return from.ToRelativeTimeString(to);
        }

        private static IEnumerable ToRelativeTimeStringCases
        {
            get
            {
                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,1,1,0,0,4)).Returns("just now");
                yield return new TestCaseData(new DateTime(2000,1,1,0,0,4), new DateTime(2000,1,1,0,0,0)).Returns("just now");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,1,1,0,0,59)).Returns("59 seconds from now");
                yield return new TestCaseData(new DateTime(2000,1,1,0,0,59), new DateTime(2000,1,1,0,0,0)).Returns("59 seconds ago");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,1,1,0,1,59)).Returns("a minute from now");
                yield return new TestCaseData(new DateTime(2000,1,1,0,1,59), new DateTime(2000,1,1,0,0,0)).Returns("a minute ago");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,1,1,0,20,59)).Returns("20 minutes from now");
                yield return new TestCaseData(new DateTime(2000,1,1,0,20,59), new DateTime(2000,1,1,0,0,0)).Returns("20 minutes ago");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,1,1,0,59,59)).Returns("59 minutes from now");
                yield return new TestCaseData(new DateTime(2000,1,1,0,59,59), new DateTime(2000,1,1,0,0,0)).Returns("59 minutes ago");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,1,1,1,59,59)).Returns("an hour from now");
                yield return new TestCaseData(new DateTime(2000,1,1,1,59,59), new DateTime(2000,1,1,0,0,0)).Returns("an hour ago");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,1,1,23,59,59)).Returns("23 hours from now");
                yield return new TestCaseData(new DateTime(2000,1,1,23,59,59), new DateTime(2000,1,1,0,0,0)).Returns("23 hours ago");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,1,2,23,59,59)).Returns("next day");
                yield return new TestCaseData(new DateTime(2000,1,2,23,59,59), new DateTime(2000,1,1,0,0,0)).Returns("last day");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,1,7,23,59,59)).Returns("6 days from now");
                yield return new TestCaseData(new DateTime(2000,1,7,23,59,59), new DateTime(2000,1,1,0,0,0)).Returns("6 days ago");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,1,14,23,59,59)).Returns("next week");
                yield return new TestCaseData(new DateTime(2000,1,14,23,59,59), new DateTime(2000,1,1,0,0,0)).Returns("last week");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,1,30,23,59,59)).Returns("4 weeks from now");
                yield return new TestCaseData(new DateTime(2000,1,30,23,59,59), new DateTime(2000,1,1,0,0,0)).Returns("4 weeks ago");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,2,28,23,59,59)).Returns("next month");
                yield return new TestCaseData(new DateTime(2000,2,28,23,59,59), new DateTime(2000,1,1,0,0,0)).Returns("last month");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2000,12,30,23,59,59)).Returns("12 months from now");
                yield return new TestCaseData(new DateTime(2000,12,30,23,59,59), new DateTime(2000,1,1,0,0,0)).Returns("12 months ago");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2001,12,30,23,59,59)).Returns("next year");
                yield return new TestCaseData(new DateTime(2001,12,30,23,59,59), new DateTime(2000,1,1,0,0,0)).Returns("last year");

                yield return new TestCaseData(new DateTime(2000,1,1,0,0,0), new DateTime(2005,12,30,23,59,59)).Returns("6 years from now");
                yield return new TestCaseData(new DateTime(2005,12,30,23,59,59), new DateTime(2000,1,1,0,0,0)).Returns("6 years ago");
            }
        }
    }
}
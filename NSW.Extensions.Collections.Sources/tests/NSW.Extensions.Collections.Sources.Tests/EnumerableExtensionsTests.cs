using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NSW.Extensions.Internal
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void NotNullOrEmpty()
        {
            Assert.False(((IEnumerable<int>?)null).NotNullOrEmpty());
            Assert.False(Enumerable.Empty<int>().NotNullOrEmpty());
            Assert.True(Enumerable.Range(1,9).NotNullOrEmpty());
        }

        [Test]
        public void GetPage()
        {
            var actual = Enumerable.Range(1, 100);
            var expected = Enumerable.Range(1, 9);
            CollectionAssert.AreEqual(expected, actual.GetPage(0,9));
            expected = Enumerable.Range(31, 10);
            CollectionAssert.AreEqual(expected, actual.GetPage(3,10));

            CollectionAssert.AreEqual(Enumerable.Empty<int>(), actual.GetPage(-3,10));
            CollectionAssert.AreEqual(Enumerable.Empty<int>(), actual.GetPage(3, 0));
            actual = null;
            CollectionAssert.AreEqual(Enumerable.Empty<int>(), actual.GetPage(0, 10));
        }
        [Test]
        public void RemoveWhere()
        {
            var actual = new[] { 1, 2, 3, 4, 6, 6, 6, 7, 8, 9, 9, 9 };
            var expected = new List<int> { 1, 2, 3, 4, 7, 8 };
            CollectionAssert.AreEqual(expected, actual.RemoveWhere(i => i == 6 || i == 9));

            CollectionAssert.AreEqual(Enumerable.Empty<int>(), actual.RemoveWhere(null));

            actual = null;
            CollectionAssert.AreEqual(Enumerable.Empty<int>(), actual.RemoveWhere(i => i == 6 || i == 9));
        }

        [Test]
        public void IgnoreNulls()
        {
            var actual = new int?[] { 1, 2, 3, 4, 5, 6, null, null, 7, 8, null, null, 9 };
            var expected = Enumerable.Range(1, 9);
            CollectionAssert.AreEqual(expected, actual.IgnoreNulls());

            actual = null;
            CollectionAssert.AreEqual(Enumerable.Empty<int>(), actual.IgnoreNulls());
        }

        [Test]
        public void OrEmptyIfNull()
        {
            var expected = Enumerable.Range(1, 9);
            Assert.AreEqual(expected, expected.OrEmptyIfNull());

            expected = null;

            Assert.AreEqual(Enumerable.Empty<int>(), expected!.OrEmptyIfNull());
        }
    }
}
using System.Collections.Generic;
using NUnit.Framework;

namespace NSW.Extensions.Internal
{
    [TestFixture]
    public class DictionaryExtensionsTests
    {
        [Test]
        public void ValuesToArray()
        {
            var actual = new Dictionary<int, int>
            {
                {1, 0},
                {2, 9},
                {3, 8},
                {4, 7},
                {5, 6}
            };

            var expected = new[] {0, 9, 8, 7, 6};
            CollectionAssert.AreEqual(expected, actual.ValuesToArray());

            actual = null;
            Assert.Null(actual.ValuesToArray());
        }

        [Test]
        public void ValuesToList()
        {
            var actual = new Dictionary<int, int>
            {
                {1, 0},
                {2, 9},
                {3, 8},
                {4, 7},
                {5, 6}
            };

            var expected = new List<int>
            {
                0,
                9,
                8,
                7,
                6
            };
            CollectionAssert.AreEqual(expected, actual.ValuesToList());
            actual = null;
            Assert.Null(actual.ValuesToArray());
        }

        [Test]
        public void KeysToArray()
        {
            var actual = new Dictionary<int, int>
            {
                {1, 0},
                {2, 9},
                {3, 8},
                {4, 7},
                {5, 6}
            };

            var expected = new[] {1, 2, 3, 4, 5};
            CollectionAssert.AreEqual(expected, actual.KeysToArray());

            actual = null;
            Assert.Null(actual!.KeysToArray());
        }

        [Test]
        public void KeysToList()
        {
            var actual = new Dictionary<int, int>
            {
                {1, 0},
                {2, 9},
                {3, 8},
                {4, 7},
                {5, 6}
            };

            var expected = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };
            CollectionAssert.AreEqual(expected, actual.KeysToList());

            actual = null;
            Assert.Null(actual!.KeysToList());
        }

        [Test]
        public void Append()
        {
            var actual = new Dictionary<int, int>
            {
                {1, 0},
                {2, 9},
                {3, 8},
                {4, 7},
                {5, 6}
            };

            var addDic = new Dictionary<int, int>
            {
                {1, 3},
                {2, 6},
                {6, 18},
                {7, 21},
                {8, 24}
            };

            var expected = new Dictionary<int, int>
            {
                {1, 0},
                {2, 9},
                {3, 8},
                {4, 7},
                {5, 6},
                {6, 18},
                {7, 21},
                {8, 24}
            };

            CollectionAssert.AreEqual(expected, actual.Append(addDic));
        }
    }
}
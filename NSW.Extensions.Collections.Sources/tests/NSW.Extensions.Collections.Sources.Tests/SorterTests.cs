using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NSW.Extensions.Internal
{
    [TestFixture]
    public class SorterTests
    {
        internal class User
        {
            public string Name { get; set; } = null!;
            public int Age { get; set; }
        }
        [Test]
        public void SimpeSortingTest()
        {
            IEnumerable<User> expected = new[]
            {
                new User{Name="Denis", Age = 41},
                new User{Name="Andrei", Age = 7},
                new User{Name="Dmitry", Age = 41},
                new User{Name="Andrei", Age = 63}
            };

            var sorter = new Sorter<User>();
            var actual = sorter.Sort(expected);
            CollectionAssert.AreEqual(expected, actual);

            sorter.AddAsc(u=>u.Name);
            actual = sorter.Sort(expected);
            Assert.AreEqual(expected.ToList()[1], actual.ToList()[0]);

            sorter.AddDesc(u=>u.Age);
            actual = sorter.Sort(expected);
            Assert.AreEqual(expected.ToList()[3], actual.ToList()[0]);
        }
    }
}

using System;
using NSW.Testing.Internal;
using NUnit.Framework;

namespace NSW.Extensions.Internal
{
    class TestStringableClass
    {
        public int IntVal { get; set; }
        public Guid GuidVal { get; set; }
    }

    [TestFixture]
    public class StringableTests
    {
        [Test]
        public void ShouldToString()
        {
            var initObj = new TestStringableClass().Randomize();
            var stringable = new Stringable<TestStringableClass>(initObj, o => o.GuidVal.ToString());
            Assert.AreEqual(initObj.GuidVal.ToString(), stringable.ToString());
            Assert.AreEqual(initObj, stringable.GetValue());
        }
    }
}
using NSW.Testing.Internal;
using NUnit.Framework;

namespace NSW.Extensions.Internal
{
    [TestFixture]
    public class TypeWrapperTests
    {
        public class TestClass
        {
            public string Test1 { get; set; } = null!;
            public string Test2 { get; }
            public int? Test3 { get; set; }

            public TestClass(string test2)
            {
                Test2 = test2;
            }
        }
        [Test]
        public void ShouldReadPerformanceTest()
        {
            var expected = new TestClass(RandomHelper.NextString()) { Test1 = RandomHelper.NextString(), Test3 = RandomHelper.NextInt()};

            var wrapper = new TypeWrapper(expected);
            Assert.AreEqual(expected.Test1, (string)wrapper.Get("Test1")!);
            Assert.AreEqual(expected.Test2, wrapper.GetOrDefault<string>("Test2"));
            Assert.AreEqual(expected.Test3, wrapper.GetOrDefault<int?>("Test3"));

            Assert.Null(wrapper.Get("Test4"));
            Assert.Null(wrapper.GetOrDefault<string>("Test4"));

            var wrapperOfT = new TypeWrapper<TestClass>(expected);
            Assert.AreEqual(expected.Test1, (string)wrapperOfT.Get("Test1")!);
            Assert.AreEqual(expected.Test2, wrapperOfT.GetOrDefault<string>("Test2"));
            Assert.AreEqual(expected.Test3, wrapperOfT.GetOrDefault<int?>("Test3"));

            Assert.Null(wrapperOfT.Get("Test4"));
            Assert.Null(wrapperOfT.GetOrDefault<string>("Test4"));
        }
    }
}
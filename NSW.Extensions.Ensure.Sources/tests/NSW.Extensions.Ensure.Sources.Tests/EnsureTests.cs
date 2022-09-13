using System;
using NSW.Testing.Internal;
using NUnit.Framework;

namespace NSW.Extensions.Internal
{
    [TestFixture(TestOf = typeof(Ensure))]
    public class EnsureTests
    {
        [Test]
        public void ThatTTest()
        {
            var message = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.That<ArgumentNullException>(true, message));

            var exception = Assert.Throws<ArgumentNullException>(() => Ensure.That<ArgumentNullException>(false, message));
            Assert.AreEqual(message, exception!.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => Ensure.That<ArgumentNullException>(false, null));
            Assert.AreEqual(string.Empty, exception!.ParamName);
        }

        [GenericTestCase(typeof(AggregateException), TestName = "Ensure.That<" + nameof(AggregateException) + ">")]
        [GenericTestCase(typeof(NotSupportedException), TestName = "Ensure.That<" + nameof(NotSupportedException) + ">")]
        [GenericTestCase(typeof(NotImplementedException), TestName = "Ensure.That<" + nameof(NotImplementedException) + ">")]
        [GenericTestCase(typeof(IndexOutOfRangeException), TestName = "Ensure.That<" + nameof(IndexOutOfRangeException) + ">")]
        [GenericTestCase(typeof(NullReferenceException), TestName = "Ensure.That<" + nameof(NullReferenceException) + ">")]
        public void ThatTTest<T>() where T:Exception
        {
            var message = RandomHelper.NextString(10);
            var exception = Assert.Throws<T>(() => Ensure.That<T>(false, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<T>(() => Ensure.That<T>(false));
            Assert.AreEqual(string.Empty, exception!.Message);

            exception = Assert.Throws<T>(() => Ensure.That<T>(false, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }

        [Test]
        public void ThatTest()
        {
            var message = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.That(true, message));

            var exception = Assert.Throws<Exception>(() => Ensure.That(false, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.That(false));
            Assert.AreEqual(string.Empty, exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.That(false, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }

        [Test]
        public void NotTTest()
        {
            var message = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.Not<ArgumentNullException>(false, message));

            var exception = Assert.Throws<ArgumentNullException>(() => Ensure.Not<ArgumentNullException>(true, message));
            Assert.AreEqual(message, exception!.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => Ensure.Not<ArgumentNullException>(true, null));
            Assert.AreEqual(string.Empty, exception!.ParamName);
        }

        [GenericTestCase(typeof(AggregateException), TestName = "Ensure.Not<" + nameof(AggregateException) + ">")]
        [GenericTestCase(typeof(NotSupportedException), TestName = "Ensure.Not<" + nameof(NotSupportedException) + ">")]
        [GenericTestCase(typeof(NotImplementedException), TestName = "Ensure.Not<" + nameof(NotImplementedException) + ">")]
        [GenericTestCase(typeof(IndexOutOfRangeException), TestName = "Ensure.Not<" + nameof(IndexOutOfRangeException) + ">")]
        [GenericTestCase(typeof(NullReferenceException), TestName = "Ensure.Not<" + nameof(NullReferenceException) + ">")]
        public void NotTTest<T>() where T:Exception
        {
            var message = RandomHelper.NextString(10);
            var exception = Assert.Throws<T>(() => Ensure.Not<T>(true, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<T>(() => Ensure.Not<T>(true));
            Assert.AreEqual(string.Empty, exception!.Message);

            exception = Assert.Throws<T>(() => Ensure.Not<T>(true, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }

        [Test]
        public void NotTest()
        {
            var message = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.Not(false, message));

            var exception = Assert.Throws<Exception>(() => Ensure.Not(true, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.Not(true));
            Assert.AreEqual(string.Empty, exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.Not(true, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }

        [Test]
        public void NotNullTest()
        {
            var message = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.NotNull(new object(), message));

            var exception = Assert.Throws<NullReferenceException>(() => Ensure.NotNull(null, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<NullReferenceException>(() => Ensure.NotNull(null));
            Assert.AreEqual("Value must be not null.", exception!.Message);

            exception = Assert.Throws<NullReferenceException>(() => Ensure.NotNull(null, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }

        [Test]
        public void EqualTest()
        {
            var message = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.Equal(1,1, message));

            var exception = Assert.Throws<Exception>(() => Ensure.Equal(1, 2, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.Equal(1, 2));
            Assert.AreEqual("Values must be equal.", exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.Equal(1, 2, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }

        [Test]
        public void NotEqualTest()
        {
            var message = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.NotEqual(1,2, message));

            var exception = Assert.Throws<Exception>(() => Ensure.NotEqual(1, 1, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.NotEqual(1, 1));
            Assert.AreEqual("Values must not be equal.", exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.NotEqual(1, 1, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }

        [Test]
        public void ContainsTest()
        {
            var message = RandomHelper.NextString(10);
            var array = new[] {1, 2, 3, 4, 5};

            Assert.DoesNotThrow(() => Ensure.Contains(array, i => i==4, message));

            var exception = Assert.Throws<Exception>(() => Ensure.Contains(array, i => i==6, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.Contains(array, i => i==6));
            Assert.AreEqual(string.Empty, exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.Contains(array, i => i==6, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }

        [Test]
        public void ItemsTest()
        {
            var message = RandomHelper.NextString(10);
            var array = new[] {1, 1, 1, 1, 1};

            Assert.DoesNotThrow(() => Ensure.Items(array, i => i==1, message));

            var exception = Assert.Throws<Exception>(() => Ensure.Items(array, i => i==6, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.Items(array, i => i==6));
            Assert.AreEqual(string.Empty, exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.Items(array, i => i==6, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }

        [TestCase("")]
        [TestCase(null)]
        public void NotNullOrEmptyTest(string nullString)
        {
            var message = RandomHelper.NextString(10);

            Assert.DoesNotThrow(() => Ensure.NotNullOrEmpty(RandomHelper.NextString(10), message));

            var exception = Assert.Throws<Exception>(() => Ensure.NotNullOrEmpty(nullString, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.NotNullOrEmpty(nullString));
            Assert.AreEqual("String cannot be null or empty.", exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.NotNullOrEmpty(nullString, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }

        [TestCase("")]
        [TestCase(null)]
        public void NotNullOrWhiteSpaceTest(string nullString)
        {
            var message = RandomHelper.NextString(10);

            Assert.DoesNotThrow(() => Ensure.NotNullOrWhiteSpace(RandomHelper.NextString(10), message));

            var exception = Assert.Throws<Exception>(() => Ensure.NotNullOrWhiteSpace(nullString, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.NotNullOrWhiteSpace(nullString));
            Assert.AreEqual("String cannot be null or white space.", exception!.Message);

            exception = Assert.Throws<Exception>(() => Ensure.NotNullOrWhiteSpace(nullString, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }
    }
}
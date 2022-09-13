using System;
using System.Collections.Generic;
using NSW.Testing.Internal;
using NUnit.Framework;

namespace NSW.Extensions.Internal
{
    [TestFixture(TestOf = typeof(Argument))]
    public class EnsureArgumentTests
    {
        [Test]
        public void IsTests()
        {
            var message = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.Argument.Is(true, message));

            var exception = Assert.Throws<ArgumentException>(() => Ensure.Argument.Is(false, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<ArgumentException>(() => Ensure.Argument.Is(false));
            Assert.AreEqual(string.Empty, exception!.Message);

            exception = Assert.Throws<ArgumentException>(() => Ensure.Argument.Is(false, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }

        [Test]
        public void IsNotTests()
        {
            var message = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.Argument.IsNot(false, message));

            var exception = Assert.Throws<ArgumentException>(() => Ensure.Argument.IsNot(true, message));
            Assert.AreEqual(message, exception!.Message);

            exception = Assert.Throws<ArgumentException>(() => Ensure.Argument.IsNot(true));
            Assert.AreEqual(string.Empty, exception!.Message);

            exception = Assert.Throws<ArgumentException>(() => Ensure.Argument.IsNot(true, null));
            Assert.AreEqual(string.Empty, exception!.Message);
        }

        [Test]
        public void IsTypeTests()
        {
            var parameter = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.Argument.IsType<int>(1, parameter));

            var exception = Assert.Throws<ArgumentException>(() => Ensure.Argument.IsType<int>(true, parameter));
            Assert.AreEqual($"Value must be {typeof(int).FullName}. (Parameter '{parameter}')", exception!.Message);
            Assert.AreEqual(parameter, exception.ParamName);

            exception = Assert.Throws<ArgumentException>(() => Ensure.Argument.IsType<int>(true, null));
            Assert.AreEqual($"Value must be {typeof(int).FullName}.", exception!.Message);
            Assert.AreEqual(null, exception.ParamName);
        }

        [Test]
        public void TypeTests()
        {
            var parameter = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() =>
            {
                var p = Ensure.Argument.Type<int>(1, parameter);
                Assert.AreEqual(1, p);
            });

            var exception = Assert.Throws<ArgumentException>(() => Ensure.Argument.Type<int>(true, parameter));
            Assert.AreEqual($"Value must be {typeof(int).FullName}. (Parameter '{parameter}')", exception!.Message);
            Assert.AreEqual(parameter, exception.ParamName);

            exception = Assert.Throws<ArgumentException>(() => Ensure.Argument.Type<int>(true, null));
            Assert.AreEqual($"Value must be {typeof(int).FullName}.", exception!.Message);
            Assert.AreEqual(null, exception.ParamName);
        }

        [Test]
        public void NotNullTests()
        {
            var parameter = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.Argument.NotNull(new object(), parameter));

            var exception = Assert.Throws<ArgumentNullException>(() => Ensure.Argument.NotNull(null, parameter));
            Assert.AreEqual($"Value cannot be null. (Parameter '{parameter}')", exception!.Message);
            Assert.AreEqual(parameter, exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => Ensure.Argument.NotNull(null, null));
            Assert.AreEqual("Value cannot be null.", exception!.Message);
            Assert.AreEqual(string.Empty, exception.ParamName);
        }

        [Test]
        public void NotEmptyTests()
        {
            var parameter = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.Argument.NotEmpty(Guid.NewGuid(), parameter));

            var exception = Assert.Throws<ArgumentException>(() => Ensure.Argument.NotEmpty(Guid.Empty, parameter));
            Assert.AreEqual($"Value cannot be {Guid.Empty}. (Parameter '{parameter}')", exception!.Message);
            Assert.AreEqual(parameter, exception.ParamName);

            exception = Assert.Throws<ArgumentException>(() => Ensure.Argument.NotEmpty(Guid.Empty, null));
            Assert.AreEqual($"Value cannot be {Guid.Empty}.", exception!.Message);
            Assert.AreEqual(null, exception.ParamName);
        }

        [Test]
        public void NotNullOrEmptyTests()
        {
            var parameter = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.Argument.NotNullOrEmpty(RandomHelper.NextString(10), parameter));

            var exception = Assert.Throws<ArgumentNullException>(() => Ensure.Argument.NotNullOrEmpty(null, parameter));
            Assert.AreEqual($"String cannot be null or empty. (Parameter '{parameter}')", exception!.Message);
            Assert.AreEqual(parameter, exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => Ensure.Argument.NotNullOrEmpty(null, null));
            Assert.AreEqual("String cannot be null or empty.", exception!.Message);
            Assert.AreEqual(null, exception.ParamName);
        }

        [Test]
        public void NotNullOrWhiteSpaceTests()
        {
            var parameter = RandomHelper.NextString(10);
            Assert.DoesNotThrow(() => Ensure.Argument.NotNullOrWhiteSpace(RandomHelper.NextString(10), parameter));

            var exception = Assert.Throws<ArgumentNullException>(() => Ensure.Argument.NotNullOrWhiteSpace(null, parameter));
            Assert.AreEqual($"String cannot be null or white space. (Parameter '{parameter}')", exception!.Message);
            Assert.AreEqual(parameter, exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => Ensure.Argument.NotNullOrWhiteSpace(null, null));
            Assert.AreEqual("String cannot be null or white space.", exception!.Message);
            Assert.AreEqual(null, exception.ParamName);
        }

        [Test]
        public void IsEmptyEnumerableTests()
        {
            var parameter = RandomHelper.NextString(10);
            var list = new List<int>();
            Assert.DoesNotThrow(() => Ensure.Argument.IsEmpty(list, parameter));

            var exception = Assert.Throws<ArgumentNullException>(() => Ensure.Argument.IsEmpty((int[]?)null, parameter));
            Assert.AreEqual($"Value cannot be null. (Parameter '{parameter}')", exception?.Message);
            Assert.AreEqual(parameter, exception?.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => Ensure.Argument.IsEmpty((int[]?)null, null));
            Assert.AreEqual("Value cannot be null.", exception?.Message);
            Assert.AreEqual(null, exception!.ParamName);

            list.Add(1);
            var exception2 = Assert.Throws<ArgumentException>(() => Ensure.Argument.IsEmpty(list, parameter));
            Assert.AreEqual($"Value must be empty enumerable. (Parameter '{parameter}')", exception2!.Message);
            Assert.AreEqual(parameter, exception2.ParamName);

            exception2 = Assert.Throws<ArgumentException>(() => Ensure.Argument.IsEmpty(list, null));
            Assert.AreEqual($"Value must be empty enumerable.", exception2!.Message);
            Assert.AreEqual(null, exception2.ParamName);
        }

        [Test]
        public void IsNullOrEmptyEnumerableTests()
        {
            var parameter = RandomHelper.NextString(10);
            var list = new List<int>();
            Assert.DoesNotThrow(() => Ensure.Argument.IsNullOrEmpty(list, parameter));
            Assert.DoesNotThrow(() => Ensure.Argument.IsNullOrEmpty((int[]?)null, parameter));

            list.Add(1);
            var exception2 = Assert.Throws<ArgumentException>(() => Ensure.Argument.IsNullOrEmpty(list, parameter));
            Assert.AreEqual($"Value must be null or empty enumerable. (Parameter '{parameter}')", exception2!.Message);
            Assert.AreEqual(parameter, exception2.ParamName);

            exception2 = Assert.Throws<ArgumentException>(() => Ensure.Argument.IsNullOrEmpty(list, null));
            Assert.AreEqual("Value must be null or empty enumerable.", exception2!.Message);
            Assert.AreEqual(null, exception2.ParamName);
        }

        [Test]
        public void NotEmptyEnumerableTests()
        {
            var parameter = RandomHelper.NextString(10);
            var list = new List<int> {1};
            Assert.DoesNotThrow(() => Ensure.Argument.NotEmpty(list, parameter));

            var exception = Assert.Throws<ArgumentNullException>(() => Ensure.Argument.IsEmpty((int[]?)null, parameter));
            Assert.AreEqual($"Value cannot be null. (Parameter '{parameter}')", exception!.Message);
            Assert.AreEqual(parameter, exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => Ensure.Argument.IsEmpty((int[]?)null, null));
            Assert.AreEqual("Value cannot be null.", exception!.Message);
            Assert.AreEqual(null, exception.ParamName);
        }

        [Test]
        public void NotNullExpressionTests()
        {
            var list = new List<int> {1};
            Assert.DoesNotThrow(() => Ensure.Argument.NotNull(() => list));

            object? data = null;
            var exception = Assert.Throws<ArgumentNullException>(() => Ensure.Argument.NotNull(() => data));
            Assert.AreEqual($"Value cannot be null. (Parameter '{nameof(data)}')", exception!.Message);
            Assert.AreEqual(nameof(data), exception.ParamName);
        }
    }
}
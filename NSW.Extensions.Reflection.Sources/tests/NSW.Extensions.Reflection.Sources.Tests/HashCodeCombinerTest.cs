using NUnit.Framework;

namespace NSW.Extensions.Internal
{
    [TestFixture]
    public class HashCodeCombinerTest
    {
        [Test]
        public void GivenTheSameInputs_ItProducesTheSameOutput()
        {
            var hashCode1 = new HashCodeCombiner();
            var hashCode2 = new HashCodeCombiner();

            hashCode1.Add(42);
            hashCode1.Add("foo");
            hashCode2.Add(42);
            hashCode2.Add("foo");

            Assert.AreEqual(hashCode1.CombinedHash, hashCode2.CombinedHash);
        }

        [Test]
        public void HashCode_Is_OrderSensitive()
        {
            var hashCode1 = HashCodeCombiner.Start();
            var hashCode2 = HashCodeCombiner.Start();

            hashCode1.Add(42);
            hashCode1.Add("foo");

            hashCode2.Add("foo");
            hashCode2.Add(42);

            Assert.AreNotEqual(hashCode1.CombinedHash, hashCode2.CombinedHash);
        }
    }
}
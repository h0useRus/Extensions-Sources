using System.Reflection;
using NUnit.Framework;

namespace NSW.Extensions.Internal
{
    [TestFixture]
    public class AssemblyInfoTests
    {
        [Test]
        public void CustomShouldReturnAssemblyInfo()
        {
            var assembly = typeof(AssemblyInfoTests).GetTypeInfo().Assembly;
            AssemblyInfo info = assembly;
            Assert.IsNotNull(info.Version);
            Assert.AreEqual("1.0.0.0", info.AssemblyVersion);
            Assert.AreEqual("NSW.Extensions.Reflection.Sources.Tests", info.Company);
            Assert.AreEqual("NSW.Extensions.Reflection.Sources.Tests", info.Product);
            Assert.AreEqual("NSW.Extensions.Reflection.Sources.Tests", info.Title);
            Assert.AreEqual(info.Culture, string.Empty);
            Assert.NotNull(AssemblyInfo.Entry);
            Assert.NotNull(AssemblyInfo.Executing);
        }
    }
}
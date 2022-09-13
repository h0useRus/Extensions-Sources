using System.Text;
using NUnit.Framework;

namespace NSW.Extensions.Internal
{
    [TestFixture]
    public class StringBuilderExtensionsTests
    {
        [Test]
        public void AppendLineFormat()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{0}% of {1} data", 1, "post");
            sb.Append("other line");
            Assert.AreEqual("1% of post data\r\nother line", sb.ToString());
        }

        [Test]
        public void AppendLineIf()
        {
            var sb = new StringBuilder();
            sb.AppendLineIf(true, new {data = 1234});
            sb.AppendLineIf(false, new {data = -1});
            sb.Append("other line");
            Assert.AreEqual("{ data = 1234 }\r\nother line", sb.ToString());
        }

        [Test]
        public void AppendLineIfFormat()
        {
            var sb = new StringBuilder();
            sb.AppendLineIf(true, "{0}% of {1} data", 1, "post");
            sb.AppendLineIf(false, "Shouldn't see this");
            sb.Append("other line");
            Assert.AreEqual("1% of post data\r\nother line", sb.ToString());
        }

        [Test]
        public void AppendIf()
        {
            var sb = new StringBuilder();
            sb.AppendIf(true, new {data = 1234});
            sb.AppendIf(false, new {data = -1});
            Assert.AreEqual("{ data = 1234 }", sb.ToString());
        }

        [Test]
        public void AppendFormatIf()
        {
            var sb = new StringBuilder();
            sb.AppendFormatIf(true, "{0}% of {1} data", 1, "post");
            sb.AppendFormatIf(false, "Shouldn't see this");
            Assert.AreEqual("1% of post data", sb.ToString());
        }
    }
}
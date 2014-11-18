using System.Configuration;
using NUnit.Framework;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Tests
{
    [TestFixture]
    public class MarkupClientTests
    {
        [TestFixtureSetUp]
        public void SetupAppSettings()
        {
            ConfigurationManager.AppSettings["LocalCacheFolder"] = "C:\\dev\\Veneer\\Client\\";
        }

        [Test, Category("Integration")]
        public void SmokeTest()
        {

            // Arrange
            ConfigurationManager.AppSettings["MarkupServiceUrl"] = "http://localhost:2222/api/Markup/";
            var client = new MarkupClient();
            
            // Act
            var content = client.Get(ContentTypes.Footer);
            
            // Assert
            Assert.That(content, Is.Not.Null);
            Assert.That(content, Is.InstanceOf<string>());
        }
    }
}


using System.Configuration;
using NUnit.Framework;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Tests
{
    [TestFixture]
    public class ContentClientTests
    {

        [Test, Category("Integration")]
        public void SmokeTest()
        {

            // Arrange
            ConfigurationManager.AppSettings["VeneerServiceUrl"] = "http://localhost:2222/api/Content/";
            var client = new ContentClient();
            
            // Act
            var content = client.Get(ContentTypes.Footer);
            
            // Assert
            Assert.That(content, Is.Not.Null);
        }

        [Test]
        public void Content_service_reference_is_only_initialised_once()
        {
            // Arrange
            ConfigurationManager.AppSettings["VeneerServiceUrl"] = "http://localhost:2222/api/Content/";
            var client = new ContentClient();

            // Act
            var footerContent = client.Get(ContentTypes.Footer);            
            ConfigurationManager.AppSettings["VeneerServiceUrl"] = "62fg5432adas@;_90382";  // invalid url so would break if tried to re-init
            var fatFooterContent = client.Get(ContentTypes.FatFooter);

            // Assert
            Assert.That(footerContent, Is.Not.Null);
            Assert.That(fatFooterContent, Is.Not.Null);
        }
    }
}


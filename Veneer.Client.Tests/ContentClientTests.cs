using System.Configuration;
using NUnit.Framework;

namespace Veneer.Client.Tests
{
    [TestFixture]
    public class ContentClientTests
    {

        [Test, Category("Integration")]
        public void SmokeTest()
        {

            // Arrange
            ConfigurationManager.AppSettings["VeneerServiceUrl"] = "http://veneer.service.local.parliament.uk/api/Content/";
            var client = new ContentClient();
            
            // Act
            var content = client.Get("footer", string.Empty);
            
            // Assert
            Assert.That(content, Is.Not.Null);
        }
    }
}

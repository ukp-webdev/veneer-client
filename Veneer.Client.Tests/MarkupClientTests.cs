using System;
using System.Configuration;
using Moq;
using NUnit.Framework;
using Veneer.Client.Caching;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;

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

        [Test]
        public void Markup_client_stores_last_known_good_response_to_local_cache()
        {
            // Arrange
            var service = new Mock<IMarkupService>();
            const string content = "<html />";
            service.Setup(x => x.Get(ContentTypes.Intranet_FatFooter)).Returns(content);
            var cache = new Mock<ILocalCache<string>>();
            cache.Setup(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<string>(), It.IsAny<DateTime>()));
            var client = new MarkupClient(service.Object, cache.Object);

            // Act
            client.Get(ContentTypes.Intranet_FatFooter);

            // Assert
            service.Verify(x => x.Get(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void Markup_client_retrieves_last_known_good_content_from_local_cache_if_service_throws_exception()
        {
            // Arrange
            var service = new Mock<IMarkupService>();

            service.Setup(x => x.Get(ContentTypes.Intranet_FatFooter)).Throws(new Exception("Unit test exception"));
            var cache = new Mock<ILocalCache<string>>();
            cache.Setup(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<string>(), It.IsAny<DateTime>()));
            const string content = "<html />";
            cache.Setup(x => x.ReadFromCache(ContentTypes.Intranet_FatFooter)).Returns(content);
            var client = new MarkupClient(service.Object, cache.Object);

            // Act
            client.Get(ContentTypes.Intranet_FatFooter);

            // Assert
            service.Verify(x => x.Get(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.ReadFromCache(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void Markup_client_retrieves_last_known_good_content_from_local_cache_if_service_returns_no_data()
        {
            // Arrange
            var service = new Mock<IMarkupService>();

            service.Setup(x => x.Get(ContentTypes.Intranet_FatFooter)).Returns((string)null);
            var cache = new Mock<ILocalCache<string>>();
            cache.Setup(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<string>(), It.IsAny<DateTime>()));
            cache.Setup(x => x.ReadFromCache(ContentTypes.Intranet_FatFooter)).Returns((string)null);
            var client = new MarkupClient(service.Object, cache.Object);

            // Act
            client.Get(ContentTypes.Intranet_FatFooter);

            // Assert
            service.Verify(x => x.Get(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.ReadFromCache(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
        }
    }
}


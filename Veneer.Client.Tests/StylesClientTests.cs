using System;
using System.Collections.Generic;
using System.Configuration;
using Moq;
using NUnit.Framework;
using Veneer.Client.Caching;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;

namespace Veneer.Client.Tests
{
    [TestFixture]
    public class StylesClientTests
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
            ConfigurationManager.AppSettings["StylesServiceUrl"] = "http://localhost:2222/api/Styles/";
            var client = new StylesClient();
            
            // Act
            var content = client.Get(ContentTypes.Intranet_FatFooter);
            
            // Assert
            Assert.That(content, Is.Not.Null);
            Assert.That(content, Is.InstanceOf<List<ContentStyle>>());
        }

        [Test]
        public void Styles_client_stores_last_known_good_response_to_local_cache()
        {
            // Arrange
            var service = new Mock<IStylesService>();
            var styles = new List<ContentStyle>
            {
                new ContentStyle
                {
                    Url = new Uri("http://styles.com/1.css")
                }
            };
            service.Setup(x => x.Get(ContentTypes.Intranet_FatFooter)).Returns(styles);
            var cache = new Mock<ILocalCache<List<ContentStyle>>>();
            cache.Setup(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<List<ContentStyle>>(), It.IsAny<DateTime>()));
            var client = new StylesClient(service.Object, cache.Object);

            // Act
            client.Get(ContentTypes.Intranet_FatFooter);

            // Assert
            service.Verify(x => x.Get(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<List<ContentStyle>>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void Styles_client_retrieves_last_known_good_content_from_local_cache_if_service_throws_exception()
        {
            // Arrange
            var service = new Mock<IStylesService>();

            service.Setup(x => x.Get(ContentTypes.Intranet_FatFooter)).Throws(new Exception("Unit test exception"));
            var cache = new Mock<ILocalCache<List<ContentStyle>>>();
            cache.Setup(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<List<ContentStyle>>(), It.IsAny<DateTime>()));
            var styles = new List<ContentStyle>
            {
                new ContentStyle
                {
                    Url = new Uri("http://styles.com/1.css")
                }
            };            
            cache.Setup(x => x.ReadFromCache(ContentTypes.Intranet_FatFooter)).Returns(styles);
            var client = new StylesClient(service.Object, cache.Object);

            // Act
            client.Get(ContentTypes.Intranet_FatFooter);

            // Assert
            service.Verify(x => x.Get(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.ReadFromCache(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<List<ContentStyle>>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void Styles_client_retrieves_last_known_good_content_from_local_cache_if_service_returns_no_data()
        {
            // Arrange
            var service = new Mock<IStylesService>();

            service.Setup(x => x.Get(ContentTypes.Intranet_FatFooter)).Returns((List<ContentStyle>)null);
            var cache = new Mock<ILocalCache<List<ContentStyle>>>();
            cache.Setup(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<List<ContentStyle>>(), It.IsAny<DateTime>()));
            cache.Setup(x => x.ReadFromCache(ContentTypes.Intranet_FatFooter)).Returns((List<ContentStyle>)null);
            var client = new StylesClient(service.Object, cache.Object);

            // Act
            client.Get(ContentTypes.Intranet_FatFooter);

            // Assert
            service.Verify(x => x.Get(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.ReadFromCache(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<List<ContentStyle>>(), It.IsAny<DateTime>()), Times.Never);
        }
    }
}


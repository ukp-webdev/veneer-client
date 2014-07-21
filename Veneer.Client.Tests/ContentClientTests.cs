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
    public class ContentClientTests
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
            ConfigurationManager.AppSettings["VeneerServiceUrl"] = "http://localhost:2222/api/Content/";
            var client = new ContentClient();
            
            // Act
            var content = client.Get(ContentTypes.Footer);
            
            // Assert
            Assert.That(content, Is.Not.Null);
        }

        [Test, Category("Integration")]
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

        [Test]
        public void Content_client_stores_last_known_good_response_to_local_cache()
        {   
            // Arrange
            var service = new Mock<IContentService>();
            var content = new Content
            {
                RefreshDate = DateTime.Now,
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.FatFooter.ToString(),
                        Html = "fat"
                    }
                }
            };
            service.Setup(x => x.Get(ContentTypes.FatFooter)).Returns(content);
            var cache = new Mock<ILocalCache>();
            cache.Setup(x => x.WriteToCache(ContentTypes.FatFooter, It.IsAny<Content>()));
            var client = new ContentClient(service.Object, cache.Object);

            // Act
            client.Get(ContentTypes.FatFooter);

            // Assert
            service.Verify(x => x.Get(ContentTypes.FatFooter), Times.Once);
            cache.Verify(x => x.WriteToCache(ContentTypes.FatFooter, It.IsAny<Content>()), Times.Once);
        }

        [Test]
        public void Content_client_retrieves_last_known_good_content_from_local_cache_if_service_throws_exception()
        {
            // Arrange
            var service = new Mock<IContentService>();
            
            service.Setup(x => x.Get(ContentTypes.FatFooter)).Throws(new Exception("Unit test exception"));
            var cache = new Mock<ILocalCache>();
            cache.Setup(x => x.WriteToCache(ContentTypes.FatFooter, It.IsAny<Content>()));
            var content = new Content
            { 
                RefreshDate = DateTime.Now, 
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.FatFooter.ToString()
                    }
                }
            };
            cache.Setup(x => x.ReadFromCache(ContentTypes.FatFooter)).Returns(content);
            var client = new ContentClient(service.Object, cache.Object);

            // Act
            client.Get(ContentTypes.FatFooter);

            // Assert
            service.Verify(x => x.Get(ContentTypes.FatFooter), Times.Once);
            cache.Verify(x => x.ReadFromCache(ContentTypes.FatFooter), Times.Once);
            cache.Verify(x => x.WriteToCache(ContentTypes.FatFooter, It.IsAny<Content>()), Times.Never);
        }

        [Test]
        public void Content_client_retrieves_last_known_good_content_from_local_cache_if_service_returns_no_data()
        {
            // Arrange
            var service = new Mock<IContentService>();

            service.Setup(x => x.Get(ContentTypes.FatFooter)).Throws(new Exception("Unit test exception"));
            var cache = new Mock<ILocalCache>();
            cache.Setup(x => x.WriteToCache(ContentTypes.FatFooter, It.IsAny<Content>()));            
            cache.Setup(x => x.ReadFromCache(ContentTypes.FatFooter)).Returns((Content)null);
            var client = new ContentClient(service.Object, cache.Object);

            // Act
            client.Get(ContentTypes.FatFooter);

            // Assert
            service.Verify(x => x.Get(ContentTypes.FatFooter), Times.Once);
            cache.Verify(x => x.ReadFromCache(ContentTypes.FatFooter), Times.Once);
            cache.Verify(x => x.WriteToCache(ContentTypes.FatFooter, It.IsAny<Content>()), Times.Never);
        }
    }
}


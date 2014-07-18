using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Veneer.Client.Caching;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Tests
{
    [TestFixture]
    public class LocalCacheTests
    {

        [Test]
        public void LocalCache_will_store_to_cache_if_content_type_not_already_added()
        {
            // Arrange
            var storageHandler = new Mock<IStorageHandler>();            
            storageHandler.Setup(x => x.WriteToStorage(It.IsAny<ContentTypes>(), It.IsAny<Content>()));
            var cache = new LocalCache(storageHandler.Object);
            var footerContent = new Content
            {
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.Footer.ToString(),
                        Html = "<div id='footer' />"
                    }
                }
            };

            // Act
            cache.WriteToCache(ContentTypes.Footer, footerContent);

            // Assert            
            storageHandler.Verify(x => x.WriteToStorage(It.IsAny<ContentTypes>(), It.IsAny<Content>()), Times.Once);
        }

        [Test]
        public void LocalCache_will_not_store_to_cache_if_content_type_already_added_with_same_refresh_time()
        {
            // Arrange
            var contentRefreshDate = new DateTime(2014, 07, 07, 12, 30, 0);
            var storageHandler = new Mock<IStorageHandler>();
            
            storageHandler.Setup(x => x.WriteToStorage(It.IsAny<ContentTypes>(), It.IsAny<Content>()));
            var cache = new LocalCache(storageHandler.Object);
            var footerContent = new Content
            {                
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.Footer.ToString(),
                        Html = "<div id='footer' />"
                    }
                },
                RefreshDateTime = contentRefreshDate
            };

            // Act
            cache.WriteToCache(ContentTypes.Footer, footerContent);
            cache.WriteToCache(ContentTypes.Footer, footerContent);
            
            // Assert            
            storageHandler.Verify(x => x.WriteToStorage(It.IsAny<ContentTypes>(), It.IsAny<Content>()), Times.Once);
        }

        [Test]
        public void LocalCache_will_store_to_cache_if_content_type_exists_with_older_refresh_time()
        {
            // Arrange            
            var storageHandler = new Mock<IStorageHandler>();

            storageHandler.Setup(x => x.WriteToStorage(It.IsAny<ContentTypes>(), It.IsAny<Content>()));
            var cache = new LocalCache(storageHandler.Object);

            var oldFooterContent = new Content
            {
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.Footer.ToString(),
                        Html = "<div id='footer' />"
                    }
                },
                RefreshDateTime = new DateTime(2014, 07, 07)
            };

            var newFooterContent = new Content
            {
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.Footer.ToString(),
                        Html = "<div id='footer'>footer</div>"
                    }
                },
                RefreshDateTime = new DateTime(2014, 07, 08)
            };

            // Act
            cache.WriteToCache(ContentTypes.Footer, oldFooterContent);
            cache.WriteToCache(ContentTypes.Footer, newFooterContent);

            // Assert            
            storageHandler.Verify(x => x.WriteToStorage(It.IsAny<ContentTypes>(), It.IsAny<Content>()), Times.Exactly(2));
        }

        [Test]
        public void LocalCache_will_retrieve_content_from_cache_if_it_exists()
        {
            // Arrange
            var cachedContent = new Content
            {
                RefreshDateTime = new DateTime(2014, 01, 01),
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.HeaderWithoutMegaNav.ToString(),
                        Html = "<div id='header' />"
                    }
                }
            };
            var storageHandler = new Mock<IStorageHandler>();
            storageHandler.Setup(x => x.ReadFromStorage(ContentTypes.HeaderWithoutMegaNav)).Returns(cachedContent);
            var cache = new LocalCache(storageHandler.Object);

            // Act
            var footerWithoutMegaNav = cache.ReadFromCache(ContentTypes.HeaderWithoutMegaNav);

            // Assert
            Assert.That(footerWithoutMegaNav, Is.Not.Null);
        }

        [Test]
        public void LocalCache_will_return_null_if_content_does_not_exist_in_cache()
        {
            // Arrange            
            var storageHandler = new Mock<IStorageHandler>();
            storageHandler.Setup(x => x.ReadFromStorage(ContentTypes.HeaderWithoutMegaNav)).Returns((Content)null);
            var cache = new LocalCache(storageHandler.Object);

            // Act
            var footerWithoutMegaNav = cache.ReadFromCache(ContentTypes.HeaderWithoutMegaNav);

            // Assert
            Assert.That(footerWithoutMegaNav, Is.Null);
        }        
    }
}

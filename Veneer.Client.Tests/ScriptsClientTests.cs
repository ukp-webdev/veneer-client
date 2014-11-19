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
    public class ScriptsClientTests
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
            ConfigurationManager.AppSettings["ScriptsServiceUrl"] = "http://localhost:2222/api/Scripts/";
            var client = new ScriptsClient();
            
            // Act
            var content = client.Get(ContentTypes.Intranet_FatFooter);
            
            // Assert
            Assert.That(content, Is.Not.Null);
            Assert.That(content, Is.InstanceOf<List<ContentScript>>());
        }

        [Test]
        public void Scripts_client_stores_last_known_good_response_to_local_cache()
        {
            // Arrange
            var service = new Mock<IScriptsService>();
            var scripts = new List<ContentScript>
            {
                new ContentScript
                {
                    Url = new Uri("http://scripts.com/1.js")
                }
            };
            service.Setup(x => x.Get(ContentTypes.Intranet_FatFooter)).Returns(scripts);
            var cache = new Mock<ILocalCache<List<ContentScript>>>();
            cache.Setup(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<List<ContentScript>>(), It.IsAny<DateTime>()));
            var client = new ScriptsClient(service.Object, cache.Object);

            // Act
            client.Get(ContentTypes.Intranet_FatFooter);

            // Assert
            service.Verify(x => x.Get(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<List<ContentScript>>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void Scripts_client_retrieves_last_known_good_content_from_local_cache_if_service_throws_exception()
        {
            // Arrange
            var service = new Mock<IScriptsService>();

            service.Setup(x => x.Get(ContentTypes.Intranet_FatFooter)).Throws(new Exception("Unit test exception"));
            var cache = new Mock<ILocalCache<List<ContentScript>>>();
            cache.Setup(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<List<ContentScript>>(), It.IsAny<DateTime>()));
            var scripts = new List<ContentScript>
            {
                new ContentScript
                {
                    Url = new Uri("http://scripts.com/1.js")
                }
            };
            cache.Setup(x => x.ReadFromCache(ContentTypes.Intranet_FatFooter)).Returns(scripts);
            var client = new ScriptsClient(service.Object, cache.Object);

            // Act
            client.Get(ContentTypes.Intranet_FatFooter);

            // Assert
            service.Verify(x => x.Get(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.ReadFromCache(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<List<ContentScript>>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void Scripts_client_retrieves_last_known_good_content_from_local_cache_if_service_returns_no_data()
        {
            // Arrange
            var service = new Mock<IScriptsService>();

            service.Setup(x => x.Get(ContentTypes.Intranet_FatFooter)).Returns((List<ContentScript>)null);
            var cache = new Mock<ILocalCache<List<ContentScript>>>();
            cache.Setup(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<List<ContentScript>>(), It.IsAny<DateTime>()));
            cache.Setup(x => x.ReadFromCache(ContentTypes.Intranet_FatFooter)).Returns((List<ContentScript>)null);
            var client = new ScriptsClient(service.Object, cache.Object);

            // Act
            client.Get(ContentTypes.Intranet_FatFooter);

            // Assert
            service.Verify(x => x.Get(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.ReadFromCache(ContentTypes.Intranet_FatFooter), Times.Once);
            cache.Verify(x => x.WriteToCache(ContentTypes.Intranet_FatFooter, It.IsAny<List<ContentScript>>(), It.IsAny<DateTime>()), Times.Never);
        }
    }
}


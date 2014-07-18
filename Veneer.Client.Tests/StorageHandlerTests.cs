using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Veneer.Client.Caching;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Tests
{
    [TestFixture]
    public class StorageHandlerTests
    {
        [SetUp]
        public void SetupConfiguration()
        {
            ConfigurationManager.AppSettings["LocalCacheFolder"] = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        [Test]
        public void Storage_handler_ignores_request_to_cache_if_no_app_setting_is_set_up()
        {
            try
            {
                // Arrange
                ConfigurationManager.AppSettings["LocalCacheFolder"] = string.Empty;
                var storageHandler = new StorageHandler();
                var content = new Content {RefreshDate = DateTime.Now.ToString(CultureInfo.InvariantCulture)};

                // Act
                storageHandler.WriteToStorage(ContentTypes.HeaderWithoutMegaNav, content);
            }
            catch (Exception)
            {
                // Assert
                Assert.Fail();
            }
        }

        [Test]
        public void Storage_handler_returns_null_from_local_cache_if_no_app_setting_is_set_up()
        {
            // Arrange
            ConfigurationManager.AppSettings["LocalCacheFolder"] = string.Empty;
            var storageHandler = new StorageHandler();

            // Act
            var content = storageHandler.ReadFromStorage(ContentTypes.Footer);

            // Assert
            Assert.That(content, Is.Null);
        }

        [Test, Category("Integration")]
        public void Storage_handler_writes_to_file_at_specified_location()
        {
            // Arrange
            var storageHandler = new StorageHandler();
            var content = new Content 
            {
                RefreshDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.HeaderWithMegaNav.ToString(),
                        Html = "<div id='meganav' />"
                    }
                }
            };

            // Act
            storageHandler.WriteToStorage(ContentTypes.HeaderWithMegaNav, content);

            var fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                           "\\ContentCache-HeaderWithMegaNav.cache";

            var fileContents = File.ReadAllText(fileName);

            var contentFromCache = storageHandler.ReadFromStorage(ContentTypes.HeaderWithMegaNav);

            // Assert
            Assert.That(fileContents, Is.StringContaining("meganav"));
            Assert.That(contentFromCache.Sections.FirstOrDefault(x => x.Id == ContentTypes.HeaderWithMegaNav.ToString()), Is.Not.Null);
        }        
    }
}

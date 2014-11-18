using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;

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
    }
}


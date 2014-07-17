using NUnit.Framework;
using Veneer.Client.Web.Models;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Web.Tests
{
    [TestFixture]
    public class SiteStructureTests
    {
        [Test]
        public void Model_initialises_with_empty_state()
        {
            // Arrange / Act
            var model = new SiteStructure();

            // Assert
            Assert.That(model, Is.Not.Null);
            Assert.That(model.ScriptUrls, Is.Empty);
            Assert.That(model.StyleUrls, Is.Empty);
            Assert.That(model.ContentItems, Is.Empty);
        }

        [Test]
        public void Item_provides_access_to_content_by_type()
        {
            // Arrange
            var model = new SiteStructure();
            model.ContentItems.Add(ContentTypes.HeaderWithMegaNav.ToString(), "<div id='header'></div>");

            // Act
            var html = model.Item(ContentTypes.HeaderWithMegaNav);

            // Assert
            Assert.That(html, Is.Not.Null);
        }

        [Test]
        public void Item_returns_empty_string_for_content_that_does_not_exist()
        {
            // Arrange
            var model = new SiteStructure();
            
            // Act
            var html = model.Item(ContentTypes.HeaderWithMegaNav);

            // Assert
            Assert.That(html, Is.Not.Null);
            Assert.That(html.Length, Is.EqualTo(0));
        }
    }
}

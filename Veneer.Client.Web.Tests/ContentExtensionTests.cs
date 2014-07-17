
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Veneer.Client.Web.Extensions;
using Veneer.Client.Web.Models;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Web.Tests
{
    [TestFixture]
    public class ContentExtensionTests
    {
        [Test]
        public void Extension_returns_html_for_content_type_that_exists_within_model()
        {
            // Arrange
            var model = new SiteStructure();
            model.ContentItems.Add(ContentTypes.Footer.ToString(), "<div id='footer'>footer</div>");
            var viewContext = new ViewContext();
            var viewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);

            // Act
            var html = htmlHelper.Content(model, ContentTypes.Footer);

            // Assert
            Assert.That(html, Is.Not.Null);
            Assert.That(html.ToHtmlString().Length, Is.GreaterThan(0));
        }

        [Test]
        public void Extension_returns_empty_string_for_content_type_that_does_not_exist_within_model()
        {
            // Arrange
            var model = new SiteStructure();            
            var viewContext = new ViewContext();
            var viewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);

            // Act
            var html = htmlHelper.Content(model, ContentTypes.Footer);

            // Assert
            Assert.That(html, Is.Not.Null);
            Assert.That(html.ToHtmlString().Length, Is.EqualTo(0));  
        }
    }
}

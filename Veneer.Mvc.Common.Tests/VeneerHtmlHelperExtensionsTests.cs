using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;
using Veneer.Mvc.Common.HtmlHelpers;
using Veneer.Mvc.Common.ViewModels;

namespace Veneer.Mvc.Common.Tests
{
    [TestFixture]
    public class VeneerHtmlHelperExtensionsTests
    {
        [Test]
        public void VeneerScripts_handles_content_types_with_no_scripts()
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
                        Id = "Footer", Html = "<div id='hello' />", Scripts = new List<ContentScript>()                                                     
                    }
                }
            };
            service.Setup(x => x.Get(It.IsAny<ContentTypes>())).Returns(content);
            var contentTypes = new List<ContentTypes> { ContentTypes.Footer };

            var model = new VeneerBaseViewModel(service.Object, contentTypes);

            var viewContext = new ViewContext();
            var viewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);

            // Act
            var result = htmlHelper.VeneerScripts(model).ToHtmlString();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void VeneerScripts_renders_script_tags_for_list_of_JS_files_in_model_for_single_content_type()
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
                        Id = "Footer", Html = "<div id='hello' />", Scripts = new List<ContentScript>
                        {
                             new ContentScript
                             {
                                 Url = new Uri("http://text-scripts.com/all.js")                                 
                             },
                             new ContentScript
                             {
                                 Url = new Uri("http://image-scripts.com/all.js")                                 
                             }
                        }
                    }
                }
            };
            service.Setup(x => x.Get(It.IsAny<ContentTypes>())).Returns(content);
            var contentTypes = new List<ContentTypes> {ContentTypes.Footer};

            var model = new VeneerBaseViewModel(service.Object, contentTypes);
            
            var viewContext = new ViewContext();
            var viewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);

            // Act
            var result = htmlHelper.VeneerScripts(model).ToHtmlString();

            // Assert
            Assert.That(result, Is.Not.Null.And.Not.Empty);
            Assert.That(result, Is.StringContaining(content.Sections[0].Scripts[0].Url.ToString()));
            Assert.That(result, Is.StringContaining(content.Sections[0].Scripts[1].Url.ToString()));
        }

        [Test]
        public void VeneerScripts_renders_script_tags_for_list_of_JS_files_in_model_for_multiple_content_types()
        {
            // Arrange
            var service = new Mock<IContentService>();
            var headerContent = new Content
            {
                RefreshDate = DateTime.Now,
                Sections = new List<ContentSection>
                {                   
                    new ContentSection
                    {
                        Id = "HeaderWithMegaNav", Html = "<div id='hello' />", Scripts = new List<ContentScript>
                        {
                            new ContentScript
                             {
                                 Url = new Uri("http://header-scripts.com/all.js")                                 
                             }
                        }
                    }
                }
            };

            var footerContent = new Content
            {
                RefreshDate = DateTime.Now,
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = "Footer", Html = "<div id='goodbye' />", Scripts = new List<ContentScript>
                        {                             
                             new ContentScript
                             {
                                 Url = new Uri("http://footer-scripts.com/all.js")                                 
                             }
                        }
                    }
                }
            };
            service.Setup(x => x.Get(ContentTypes.HeaderWithMegaNav)).Returns(headerContent);
            service.Setup(x => x.Get(ContentTypes.Footer)).Returns(footerContent);
            var contentTypes = new List<ContentTypes> { ContentTypes.HeaderWithMegaNav, ContentTypes.Footer };

            var model = new VeneerBaseViewModel(service.Object, contentTypes);

            var viewContext = new ViewContext();
            var viewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);

            // Act
            var result = htmlHelper.VeneerScripts(model).ToHtmlString();

            // Assert
            Assert.That(result, Is.Not.Null.And.Not.Empty);
            Assert.That(result, Is.StringContaining(headerContent.Sections[0].Scripts[0].Url.ToString()));
            Assert.That(result, Is.StringContaining(footerContent.Sections[0].Scripts[0].Url.ToString()));
        }

        [Test]
        public void VeneerStyles_handles_content_types_with_no_stylesheets()
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
                        Id = "Footer", Html = "<div id='hello' />", Styles = new List<ContentStyle>()                                                     
                    }
                }
            };
            service.Setup(x => x.Get(It.IsAny<ContentTypes>())).Returns(content);
            var contentTypes = new List<ContentTypes> { ContentTypes.Footer };

            var model = new VeneerBaseViewModel(service.Object, contentTypes);

            var viewContext = new ViewContext();
            var viewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);

            // Act
            var result = htmlHelper.VeneerStyles(model).ToHtmlString();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void VeneerStyles_renders_script_tags_for_list_of_CSS_files_in_model_for_single_content_type()
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
                        Id = "Footer", Html = "<div id='hello' />", Styles = new List<ContentStyle>
                        {
                             new ContentStyle
                             {
                                 Url = new Uri("http://text-styles.com/all.css")                                 
                             },
                             new ContentStyle
                             {
                                 Url = new Uri("http://image-styles.com/all.css")                                 
                             }
                        }
                    }
                }
            };
            service.Setup(x => x.Get(It.IsAny<ContentTypes>())).Returns(content);
            var contentTypes = new List<ContentTypes> { ContentTypes.Footer };

            var model = new VeneerBaseViewModel(service.Object, contentTypes);

            var viewContext = new ViewContext();
            var viewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);

            // Act
            var result = htmlHelper.VeneerStyles(model).ToHtmlString();

            // Assert
            Assert.That(result, Is.Not.Null.And.Not.Empty);
            Assert.That(result, Is.StringContaining(content.Sections[0].Styles[0].Url.ToString()));
            Assert.That(result, Is.StringContaining(content.Sections[0].Styles[1].Url.ToString()));
        }

        [Test]
        public void VeneerStyles_renders_script_tags_for_list_of_CSS_files_in_model_for_multiple_content_types()
        {
            // Arrange
            var service = new Mock<IContentService>();
            var headerContent = new Content
            {
                RefreshDate = DateTime.Now,
                Sections = new List<ContentSection>
                {                   
                    new ContentSection
                    {
                        Id = "HeaderWithMegaNav", Html = "<div id='hello' />", Styles = new List<ContentStyle>
                        {
                            new ContentStyle
                             {
                                 Url = new Uri("http://header-styles.com/all.css")                                 
                             }
                        }
                    }
                }
            };

            var footerContent = new Content
            {
                RefreshDate = DateTime.Now,
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = "Footer", Html = "<div id='goodbye' />", Styles = new List<ContentStyle>
                        {                             
                             new ContentStyle
                             {
                                 Url = new Uri("http://footer-styles.com/all.css")                                 
                             }
                        }
                    }
                }
            };
            service.Setup(x => x.Get(ContentTypes.HeaderWithMegaNav)).Returns(headerContent);
            service.Setup(x => x.Get(ContentTypes.Footer)).Returns(footerContent);
            var contentTypes = new List<ContentTypes> { ContentTypes.HeaderWithMegaNav, ContentTypes.Footer };

            var model = new VeneerBaseViewModel(service.Object, contentTypes);

            var viewContext = new ViewContext();
            var viewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);

            // Act
            var result = htmlHelper.VeneerStyles(model).ToHtmlString();

            // Assert
            Assert.That(result, Is.Not.Null.And.Not.Empty);
            Assert.That(result, Is.StringContaining(headerContent.Sections[0].Styles[0].Url.ToString()));
            Assert.That(result, Is.StringContaining(footerContent.Sections[0].Styles[0].Url.ToString()));
        }

        [Test]
        public void VeneerSection_handles_model_with_no_sections()
        {
            // Arrange
            var service = new Mock<IContentService>();
            service.Setup(x => x.Get(It.IsAny<ContentTypes>())).Returns(new Content());
            
            var contentTypes = new List<ContentTypes>();

            var model = new VeneerBaseViewModel(service.Object, contentTypes);

            var viewContext = new ViewContext();
            var viewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);

            // Act
            var result = htmlHelper.VeneerSection(model, ContentTypes.HeaderWithMegaNav).ToHtmlString();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void VeneerSection_handles_section_name_not_present_in_model()
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
                        Html = "<div id='home' />",
                        Id = "SectionNotUsedByThisModel"
                    }
                }
            };
            service.Setup(x => x.Get(It.IsAny<ContentTypes>())).Returns(content);

            var contentTypes = new List<ContentTypes>();

            var model = new VeneerBaseViewModel(service.Object, contentTypes);

            var viewContext = new ViewContext();
            var viewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);

            // Act
            var result = htmlHelper.VeneerSection(model, ContentTypes.HeaderWithMegaNav).ToHtmlString();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void VeneerSection_returns_markup_for_specified_content_section()
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
                        Html = "<div id='home' />",
                        Id = "HeaderWithMegaNav"
                    }
                }
            };
            service.Setup(x => x.Get(It.IsAny<ContentTypes>())).Returns(content);

            var contentTypes = new List<ContentTypes> { ContentTypes.HeaderWithMegaNav };

            var model = new VeneerBaseViewModel(service.Object, contentTypes);

            var viewContext = new ViewContext();
            var viewDataContainer = new Mock<IViewDataContainer>();
            var htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);

            // Act
            var result = htmlHelper.VeneerSection(model, ContentTypes.HeaderWithMegaNav).ToHtmlString();

            // Assert
            Assert.That(result, Is.EqualTo(content.Sections[0].Html));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Veneer.Client.Web.Controllers;
using Veneer.Client.Web.Models;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;

namespace Veneer.Client.Web.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test, Category("Integration")]
        public void Default_constructor_creates_service_client()
        {
            // Arrange
            ConfigurationManager.AppSettings["VeneerServiceUrl"] = "http://localhost:2222/api/Content";
            var controller = new HomeController();

            // Act 
            var view = controller.Index();

            // Assert
            Assert.That(view, Is.Not.Null);
        }

        [Test]
        public void Index_retrieves_content_items_from_service_client_and_merges_into_view() 
        {
            // Arrange
            var contentService = CreateMockService();
                                    
            var controller = new HomeController(contentService.Object);

            // Act
            var actionResult = controller.Index();
            var siteStructure = ((ViewResult)actionResult).Model as SiteStructure;

            // Assert
            Assert.IsNotNull(siteStructure);
            Assert.That(siteStructure.ContentItems.Count, Is.EqualTo(3));
            Assert.That(siteStructure.ContentItems.FirstOrDefault(x => x.Key == ContentTypes.FatFooter.ToString()), Is.Not.Null);
            Assert.That(siteStructure.ContentItems.FirstOrDefault(x => x.Key == ContentTypes.Footer.ToString()), Is.Not.Null);
            Assert.That(siteStructure.ContentItems.FirstOrDefault(x => x.Key == ContentTypes.HeaderWithMegaNav.ToString()), Is.Not.Null);
            Assert.That(siteStructure.ScriptUrls.Count, Is.EqualTo(3));
            Assert.That(siteStructure.ScriptUrls.FirstOrDefault(x => x == "http://scripts.com/footer"), Is.Not.Null);
            Assert.That(siteStructure.ScriptUrls.FirstOrDefault(x => x == "http://scripts.com/fat-footer"), Is.Not.Null);
            Assert.That(siteStructure.ScriptUrls.FirstOrDefault(x => x == "http://scripts.com/header"), Is.Not.Null);
            Assert.That(siteStructure.StyleUrls.Count, Is.EqualTo(3));
            Assert.That(siteStructure.StyleUrls.FirstOrDefault(x => x == "http://styles.com/footer"), Is.Not.Null);
            Assert.That(siteStructure.StyleUrls.FirstOrDefault(x => x == "http://styles.com/fat-footer"), Is.Not.Null);
            Assert.That(siteStructure.StyleUrls.FirstOrDefault(x => x == "http://styles.com/header"), Is.Not.Null);
        }

        [Test]
        public void WithMegaNav_retrieves_content_items_from_service_client_and_merges_into_view()
        {
            // Arrange
            var contentService = CreateMockService();

            var controller = new HomeController(contentService.Object);

            // Act
            var actionResult = controller.WithMegaNav();
            var siteStructure = ((ViewResult)actionResult).Model as SiteStructure;

            // Assert
            Assert.IsNotNull(siteStructure);
            Assert.That(siteStructure.ContentItems.Count, Is.EqualTo(3));
            Assert.That(siteStructure.ContentItems.FirstOrDefault(x => x.Key == ContentTypes.FatFooter.ToString()), Is.Not.Null);
            Assert.That(siteStructure.ContentItems.FirstOrDefault(x => x.Key == ContentTypes.Footer.ToString()), Is.Not.Null);
            Assert.That(siteStructure.ContentItems.FirstOrDefault(x => x.Key == ContentTypes.HeaderWithMegaNav.ToString()), Is.Not.Null);
            Assert.That(siteStructure.ScriptUrls.Count, Is.EqualTo(3));
            Assert.That(siteStructure.ScriptUrls.FirstOrDefault(x => x == "http://scripts.com/footer"), Is.Not.Null);
            Assert.That(siteStructure.ScriptUrls.FirstOrDefault(x => x == "http://scripts.com/fat-footer"), Is.Not.Null);
            Assert.That(siteStructure.ScriptUrls.FirstOrDefault(x => x == "http://scripts.com/header"), Is.Not.Null);
            Assert.That(siteStructure.StyleUrls.Count, Is.EqualTo(3));
            Assert.That(siteStructure.StyleUrls.FirstOrDefault(x => x == "http://styles.com/footer"), Is.Not.Null);
            Assert.That(siteStructure.StyleUrls.FirstOrDefault(x => x == "http://styles.com/fat-footer"), Is.Not.Null);
            Assert.That(siteStructure.StyleUrls.FirstOrDefault(x => x == "http://styles.com/header"), Is.Not.Null);
        }

        [Test]
        public void Index_deduplicates_scripts_and_styles_in_content_sections()
        {
            // Arrange
            var contentService = new Mock<IContentService>();

            var footerContent = new Content
            {
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.Footer.ToString(),
                        Html = "footer",
                        Scripts = new List<ContentScript>
                        {
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/footer")
                            },
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/common")
                            }
                        },
                        Styles = new List<ContentStyle>
                        {
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/footer")
                            },
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/common")
                            }
                        }
                    }
                }
            };

            var fatFooterContent = new Content
            {
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.FatFooter.ToString(),
                        Html = "fat footer",
                        Scripts = new List<ContentScript>
                        {
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/fat-footer")
                            },
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/common")
                            }
                        },
                        Styles = new List<ContentStyle>
                        {
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/fat-footer")
                            },
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/common")
                            }
                        }
                    }
                }
            };

            var headerWithMegaNavContent = new Content
            {
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.HeaderWithMegaNav.ToString(),
                        Html = "header",
                        Scripts = new List<ContentScript>
                        {
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/header")
                            },
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/common")
                            }
                        },
                        Styles = new List<ContentStyle>
                        {
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/header")
                            },
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/common")
                            }
                        }
                    }
                }
            };

            var headerWithoutMegaNavContent = new Content
            {
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.HeaderWithMegaNav.ToString(),
                        Html = "header",
                        Scripts = new List<ContentScript>
                        {
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/header-nomeganav")
                            },
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/common")
                            }
                        },
                        Styles = new List<ContentStyle>
                        {
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/header-nomeganav")
                            },
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/common")
                            }
                        }
                    }
                }
            };

            contentService.Setup(x => x.Get(ContentTypes.Footer)).Returns(footerContent);
            contentService.Setup(x => x.Get(ContentTypes.FatFooter)).Returns(fatFooterContent);
            contentService.Setup(x => x.Get(ContentTypes.HeaderWithMegaNav)).Returns(headerWithMegaNavContent);
            contentService.Setup(x => x.Get(ContentTypes.HeaderWithoutMegaNav)).Returns(headerWithoutMegaNavContent);

            var controller = new HomeController(contentService.Object);

            // Act
            var actionResult = controller.Index();
            var siteStructure = ((ViewResult)actionResult).Model as SiteStructure;

            // Assert
            Assert.IsNotNull(siteStructure);
            Assert.That(siteStructure.ContentItems.Count, Is.EqualTo(3));
            Assert.That(siteStructure.ContentItems.FirstOrDefault(x => x.Key == ContentTypes.FatFooter.ToString()), Is.Not.Null);
            Assert.That(siteStructure.ContentItems.FirstOrDefault(x => x.Key == ContentTypes.Footer.ToString()), Is.Not.Null);
            Assert.That(siteStructure.ContentItems.FirstOrDefault(x => x.Key == ContentTypes.HeaderWithMegaNav.ToString()), Is.Not.Null);
            Assert.That(siteStructure.ScriptUrls.Count, Is.EqualTo(4));
            Assert.That(siteStructure.ScriptUrls.FirstOrDefault(x => x == "http://scripts.com/footer"), Is.Not.Null);
            Assert.That(siteStructure.ScriptUrls.FirstOrDefault(x => x == "http://scripts.com/fat-footer"), Is.Not.Null);
            Assert.That(siteStructure.ScriptUrls.FirstOrDefault(x => x == "http://scripts.com/header"), Is.Not.Null);
            Assert.That(siteStructure.ScriptUrls.FirstOrDefault(x => x == "http://scripts.com/common"), Is.Not.Null);
            Assert.That(siteStructure.StyleUrls.Count, Is.EqualTo(4));
            Assert.That(siteStructure.StyleUrls.FirstOrDefault(x => x == "http://styles.com/footer"), Is.Not.Null);
            Assert.That(siteStructure.StyleUrls.FirstOrDefault(x => x == "http://styles.com/fat-footer"), Is.Not.Null);
            Assert.That(siteStructure.StyleUrls.FirstOrDefault(x => x == "http://styles.com/header"), Is.Not.Null);
            Assert.That(siteStructure.StyleUrls.FirstOrDefault(x => x == "http://styles.com/common"), Is.Not.Null);
        }


        private Mock<IContentService> CreateMockService()
        {
            var contentService = new Mock<IContentService>();

            var footerContent = new Content
            {
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.Footer.ToString(),
                        Html = "footer",
                        Scripts = new List<ContentScript>
                        {
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/footer")
                            }
                        },
                        Styles = new List<ContentStyle>
                        {
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/footer")
                            }
                        }
                    }
                }
            };

            var fatFooterContent = new Content
            {
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.FatFooter.ToString(),
                        Html = "fat footer",
                        Scripts = new List<ContentScript>
                        {
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/fat-footer")
                            }
                        },
                        Styles = new List<ContentStyle>
                        {
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/fat-footer")
                            }
                        }
                    }
                }
            };

            var headerWithMegaNavContent = new Content
            {
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.HeaderWithMegaNav.ToString(),
                        Html = "header",
                        Scripts = new List<ContentScript>
                        {
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/header")
                            }
                        },
                        Styles = new List<ContentStyle>
                        {
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/header")
                            }
                        }
                    }
                }
            };

            var headerWithoutMegaNavContent = new Content
            {
                Sections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        Id = ContentTypes.HeaderWithMegaNav.ToString(),
                        Html = "header",
                        Scripts = new List<ContentScript>
                        {
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/header-nomeganav")
                            },
                            new ContentScript
                            {
                                Url = new Uri("http://scripts.com/common")
                            }
                        },
                        Styles = new List<ContentStyle>
                        {
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/header-nomeganav")
                            },
                            new ContentStyle
                            {
                                Url = new Uri("http://styles.com/common")
                            }
                        }
                    }
                }
            };

            contentService.Setup(x => x.Get(ContentTypes.Footer)).Returns(footerContent);
            contentService.Setup(x => x.Get(ContentTypes.FatFooter)).Returns(fatFooterContent);
            contentService.Setup(x => x.Get(ContentTypes.HeaderWithMegaNav)).Returns(headerWithMegaNavContent);
            contentService.Setup(x => x.Get(ContentTypes.HeaderWithoutMegaNav)).Returns(headerWithoutMegaNavContent);

            return contentService;
        }
    }
}

using System;
using NUnit.Framework;
using Veneer.Contracts.DataContracts;

namespace Veneer.Contracts.Tests
{
    [TestFixture]
    public class ContractTests
    {
        [Test]
        public void Content_contains_list_of_content_sections()
        {
            // Arrange
            var content = new Content();
            
            // Act / Assert
            Assert.That(content.Sections, Is.Not.Null);
            Assert.That(content.Sections, Is.Empty);
        }

        [Test]
        public void ContentSection_contains_section_id()
        {
            // Arrange
            var contentSection = new ContentSection {Id = "testId"};

            // Act / Assert
            Assert.That(contentSection.Id, Is.EqualTo("testId"));
        }

        [Test]
        public void ContentSection_contains_list_of_styles()
        {
            // Arrange
            var contentSection = new ContentSection();

            // Act / Assert
            Assert.That(contentSection.Styles, Is.Not.Null);
            Assert.That(contentSection.Styles, Is.Empty);
        }

        [Test]
        public void ContentSection_contains_list_of_scripts()
        {
            // Arrange
            var contentSection = new ContentSection();

            // Act / Assert
            Assert.That(contentSection.Scripts, Is.Not.Null);
            Assert.That(contentSection.Scripts, Is.Empty);
        }

        [Test]
        public void ContentSection_contains_html_fragment()
        {
            // Arrange
            const string content = "<div id=\"test\">test</div>";
            var contentSection = new ContentSection { Html = content };

            // Act / Assert
            Assert.That(contentSection.Html, Is.EqualTo(content));
        }

        [Test]
        public void ContentScript_ToString_returns_html_if_set()
        {
            // Arrange
            const string scriptHtml = "<script type=\"text/javascript\">alert('hello!');</script>";
            var contentScript = new ContentScript {Html = scriptHtml};

            // Act
            var html = contentScript.ToString();

            // Assert
            Assert.That(html, Is.EqualTo(scriptHtml));
        }

        [Test]
        public void ContentScript_ToString_wraps_script_url_in_html_tag_if_no_html_set()
        {
            // Arrange
            const string scriptUrl = "http://wwww.google.com/script.js";
            var contentScript = new ContentScript {Url = new Uri(scriptUrl)};

            // Act
            var html = contentScript.ToString();

            // Assert
            Assert.That(html, Is.StringContaining(scriptUrl));
            Assert.That(html, Is.StringContaining("<script"));
        }

        [Test]
        public void ContentScript_ToString_returns_empty_string_if_no_html_or_url_set()
        {
            // Arrange
            var contentScript = new ContentScript();

            // Act
            var html = contentScript.ToString();

            // Assert
            Assert.That(html, Is.Empty);
        }

        [Test]
        public void ContentStyle_ToString_returns_html_if_set()
        {
            // Arrange
            const string styleHtml = "<style type=\"text/css\">.my-class { background: none; }</style>";
            var contentStyle = new ContentStyle { Html = styleHtml };

            // Act
            var html = contentStyle.ToString();

            // Assert
            Assert.That(html, Is.EqualTo(styleHtml));
        }

        [Test]
        public void ContentStyle_ToString_wraps_style_url_in_html_tag_if_no_html_set()
        {
            // Arrange
            const string styleUrl = "http://wwww.google.com/styles.css";
            var contentStyle = new ContentStyle { Url = new Uri(styleUrl) };

            // Act
            var html = contentStyle.ToString();

            // Assert
            Assert.That(html, Is.StringContaining(styleUrl));
            Assert.That(html, Is.StringContaining("<style"));
        }

        [Test]
        public void ContentStyle_ToString_returns_empty_string_if_no_html_or_url_set()
        {
            // Arrange
            var contentStyle = new ContentStyle();

            // Act
            var html = contentStyle.ToString();

            // Assert
            Assert.That(html, Is.Empty);
        }
    }
}

using System;
using System.Text;
using System.Web.Mvc;
using Veneer.Contracts.Enums;
using Veneer.Mvc.Common.ViewModels;

namespace Veneer.Mvc.Common.HtmlHelpers
{
    public static class VeneerHtmlHelperExtensions
    {
        public static MvcHtmlString VeneerScripts(this HtmlHelper helper, VeneerBaseViewModel model)
        {
            if (model.ScriptUrls == null || model.ScriptUrls.Count == 0)
                return MvcHtmlString.Empty;

            var response = new StringBuilder(string.Empty);
            foreach (var script in model.ScriptUrls)
            {
                var builder = new TagBuilder("script");
                builder.Attributes.Add("type", "text/javascript");
                builder.Attributes.Add("src", script);
                response.Append(builder.ToString(TagRenderMode.Normal));
                response.Append(Environment.NewLine);
            }

            return MvcHtmlString.Create(response.ToString());
        }
        public static MvcHtmlString VeneerStyles(this HtmlHelper helper, VeneerBaseViewModel model)
        {
            if (model.StyleUrls == null || model.StyleUrls.Count == 0)
                return MvcHtmlString.Empty;

            var response = new StringBuilder(string.Empty);
            foreach (var styleSheet in model.StyleUrls)
            {
                var builder = new TagBuilder("link");
                builder.Attributes.Add("rel", "stylesheet");
                builder.Attributes.Add("type", "text/css");
                builder.Attributes.Add("href", styleSheet);
                response.Append(builder.ToString(TagRenderMode.Normal));
                response.Append(Environment.NewLine);
            }

            return MvcHtmlString.Create(response.ToString());
        }

        public static MvcHtmlString VeneerSection(this HtmlHelper helper, VeneerBaseViewModel model, ContentTypes sectionName)
        {
            if (!model.ContentItems.ContainsKey(sectionName.ToString()))
                return MvcHtmlString.Empty;

            return MvcHtmlString.Create(model.ContentItems[sectionName.ToString()]);
        }
    }
}

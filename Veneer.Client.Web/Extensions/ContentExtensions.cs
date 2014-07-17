using System.Web;
using System.Web.Mvc;
using Veneer.Client.Web.Models;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Web.Extensions
{
    public static class ContentExtensions
    {
        public static IHtmlString Content(this HtmlHelper helper, SiteStructure model, ContentTypes contentType)
        {                        
            return new HtmlString(model.Item(contentType));            
        }
    }
}
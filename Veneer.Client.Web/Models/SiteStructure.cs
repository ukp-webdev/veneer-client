using System.Collections.Generic;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Web.Models
{
    public class SiteStructure
    {
        public List<string> StyleUrls { get; private set; }
        public List<string> ScriptUrls { get; private set; }

        public Dictionary<string, string> ContentItems { get; private set; }

        public SiteStructure()
        {
            ScriptUrls = new List<string>();
            StyleUrls = new List<string>();
            ContentItems = new Dictionary<string, string>();
        }

        public string Item(ContentTypes contentType)
        {
            try
            {
                return ContentItems[contentType.ToString()];
            }
            catch (KeyNotFoundException)
            {
                return string.Empty;
            }
        }
    }
}
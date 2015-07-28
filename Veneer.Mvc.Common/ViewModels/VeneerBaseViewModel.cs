using System.Collections.Generic;
using System.Linq;
using Veneer.Client;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;

namespace Veneer.Mvc.Common.ViewModels
{
    public class VeneerBaseViewModel
    {
        private readonly IContentService _veneerClient = new ContentClient();
        public List<string> StyleUrls { get; private set; }
        public List<string> ScriptUrls { get; private set; }

        public Dictionary<string, string> ContentItems { get; private set; }
        public VeneerBaseViewModel(IEnumerable<ContentTypes> contentTypes)
        {
            InitialiseModel(contentTypes);
        }

        public VeneerBaseViewModel(IContentService contentClient, IEnumerable<ContentTypes> contentTypes)
        {
            _veneerClient = contentClient;
            InitialiseModel(contentTypes);
        }

        private void InitialiseModel(IEnumerable<ContentTypes> contentTypes)
        {
            ScriptUrls = new List<string>();
            StyleUrls = new List<string>();
            ContentItems = new Dictionary<string, string>();

            PopulateSiteStructureFromContent(contentTypes);
        }

        private void PopulateSiteStructureFromContent(IEnumerable<ContentTypes> contentTypes)
        {            
            foreach (var contentType in contentTypes)
            {
                var content = _veneerClient.Get(contentType);

                var contentSection = content.Sections.FirstOrDefault(x => x.Id == contentType.ToString());

                if (contentSection != null)
                {
                    ContentItems.Add(contentType.ToString(), contentSection.Html);

                    foreach (var script in contentSection.Scripts)
                    {
                        if (ScriptUrls.FirstOrDefault(x => x == script.Url.ToString()) == null)
                        {
                            ScriptUrls.Add(script.Url.ToString());
                        }
                    }

                    foreach (var styleSheet in contentSection.Styles)
                    {
                        if (StyleUrls.FirstOrDefault(x => x == styleSheet.Url.ToString()) == null)
                        {
                            StyleUrls.Add(styleSheet.Url.ToString());
                        }
                    }
                }
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Veneer.Client.Web.Models;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;

namespace Veneer.Client.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContentService _veneerClient;        

        public HomeController()
        {
            _veneerClient = new ContentClient();            
        }

        public HomeController(IContentService clientService)
        {
            _veneerClient = clientService;            
        }

        public ActionResult Index()
        {
            var model = new SiteStructure();

            PopulateSiteStructureFromContent(model, new List<ContentTypes>
            {
                ContentTypes.Intranet_ThinFooter,
                ContentTypes.Intranet_FatFooter,
                ContentTypes.Intranet_Header_Commons 
            });            

            return View(model);
        }
        
        public ActionResult FullWidth()
        {
            var model = new SiteStructure();

            PopulateSiteStructureFromContent(model, new List<ContentTypes>
            {
                ContentTypes.Intranet_ThinFooter,
                ContentTypes.Intranet_FatFooter,
                ContentTypes.Intranet_Header
            });

            return View(model);
        }

        public ActionResult Lords()
        {
            var model = new SiteStructure();

            PopulateSiteStructureFromContent(model, new List<ContentTypes>
            {
                ContentTypes.Intranet_ThinFooter,
                ContentTypes.Intranet_FatFooter,
                ContentTypes.Intranet_Header_Lords
            });

            return View(model);
        }

        private void PopulateSiteStructureFromContent(SiteStructure model, IEnumerable<ContentTypes> contentTypes)
        {
            foreach (var contentType in contentTypes)
            {

                var content = _veneerClient.Get(contentType);

                var contentSection = content.Sections.FirstOrDefault(x => x.Id == contentType.ToString());
                
                if (contentSection != null) 
                {
                    model.ContentItems.Add(contentType.ToString(), contentSection.Html);

                    foreach (var script in contentSection.Scripts)
                    {
                        if (model.ScriptUrls.FirstOrDefault(x => x == script.Url.ToString()) == null)
                        {
                            model.ScriptUrls.Add(script.Url.ToString());
                        }
                    }

                    foreach (var styleSheet in contentSection.Styles)
                    {
                        if (model.StyleUrls.FirstOrDefault(x => x == styleSheet.Url.ToString()) == null)
                        {
                            model.StyleUrls.Add(styleSheet.Url.ToString());
                        }
                    }
                }
            }
        }
    }
}
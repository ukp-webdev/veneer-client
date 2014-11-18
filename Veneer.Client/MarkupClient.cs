using System;
using System.Configuration;
using System.ServiceModel.Web;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;

namespace Veneer.Client
{
       
    public class MarkupClient : IMarkupService
    {
        private IMarkupService _service;        

        private const string ServiceUrlKey = "MarkupServiceUrl";

        public MarkupClient()
        {
            
        }

        public MarkupClient(IMarkupService service)
        {
            _service = service;
        }

        public string Get(ContentTypes section)
        {
            return MarkupService.Get(section);
        }

        private IMarkupService MarkupService
        {
            get
            {
                if (_service != null)
                {
                    return _service;
                }

                var serviceUrl = new Uri(ConfigurationManager.AppSettings[ServiceUrlKey]);

                var channelFactory = new WebChannelFactory<IMarkupService>(serviceUrl);
                _service = channelFactory.CreateChannel();
                return _service;
            }
        }
    }
}

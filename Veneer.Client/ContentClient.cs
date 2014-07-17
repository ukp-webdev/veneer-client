using System;
using System.Configuration;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;
using System.ServiceModel.Web;

namespace Veneer.Client
{
       
    public class ContentClient : IContentService
    {
        private const string ServiceUrlKey = "VeneerServiceUrl";
        private IContentService _service;

        public IContentService ContentService
        {
            get
            {
                if (_service != null)
                {
                    return _service;
                }

                var serviceUrl = new Uri(ConfigurationManager.AppSettings[ServiceUrlKey]);

                var channelFactory = new WebChannelFactory<IContentService>(serviceUrl);
                _service = channelFactory.CreateChannel();
                return _service;
            }
        }

        public Content Get(ContentTypes section)
        {
            return ContentService.Get(section);
        }
    }
}

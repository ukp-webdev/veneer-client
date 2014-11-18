using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Web;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;

namespace Veneer.Client
{
       
    public class StylesClient : IStylesService
    {
        private IStylesService _service;        

        private const string ServiceUrlKey = "StylesServiceUrl";

        public StylesClient()
        {
            
        }

        public StylesClient(IStylesService service)
        {
            _service = service;
        }

        public List<ContentStyle> Get(ContentTypes section)
        {
            return StylesService.Get(section);
        }

        private IStylesService StylesService
        {
            get
            {
                if (_service != null)
                {
                    return _service;
                }

                var serviceUrl = new Uri(ConfigurationManager.AppSettings[ServiceUrlKey]);

                var channelFactory = new WebChannelFactory<IStylesService>(serviceUrl);
                _service = channelFactory.CreateChannel();
                return _service;
            }
        }
    }
}

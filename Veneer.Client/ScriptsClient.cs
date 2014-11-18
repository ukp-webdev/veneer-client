using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Web;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;

namespace Veneer.Client
{

    public class ScriptsClient : IScriptsService
    {
        private IScriptsService _service;        

        private const string ServiceUrlKey = "ScriptsServiceUrl";

        public ScriptsClient()
        {
            
        }

        public ScriptsClient(IScriptsService service)
        {
            _service = service;
        }

        public List<ContentScript> Get(ContentTypes section)
        {
            return ScriptsService.Get(section);
        }

        private IScriptsService ScriptsService
        {
            get
            {
                if (_service != null)
                {
                    return _service;
                }

                var serviceUrl = new Uri(ConfigurationManager.AppSettings[ServiceUrlKey]);

                var channelFactory = new WebChannelFactory<IScriptsService>(serviceUrl);
                _service = channelFactory.CreateChannel();
                return _service;
            }
        }
    }
}

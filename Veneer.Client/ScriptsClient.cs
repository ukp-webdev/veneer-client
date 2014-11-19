using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Web;
using Veneer.Client.Caching;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;

namespace Veneer.Client
{

    public class ScriptsClient : IScriptsService
    {
        private IScriptsService _service;
        private ILocalCache<List<ContentScript>> _localCache;

        private const string ServiceUrlKey = "ScriptsServiceUrl";

        public ScriptsClient()
        {
            _localCache = new LocalCache<List<ContentScript>>();
        }

        public ScriptsClient(IScriptsService service)
        {
            _service = service;
            _localCache = new LocalCache<List<ContentScript>>();
        }

        public ScriptsClient(IScriptsService service, ILocalCache<List<ContentScript>> localCache)
        {
            _service = service;
            _localCache = localCache;
        }

        public List<ContentScript> Get(ContentTypes section)
        {
            try
            {
                var content = ScriptsService.Get(section);
                if (content != null)
                {
                    _localCache.WriteToCache(section, content, DateTime.Now);
                    return content;
                }
                return _localCache.ReadFromCache(section);
            }
            catch (Exception)
            {
                return _localCache.ReadFromCache(section);
            }
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

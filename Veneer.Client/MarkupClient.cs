using System;
using System.Configuration;
using System.ServiceModel.Web;
using Veneer.Client.Caching;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;

namespace Veneer.Client
{
       
    public class MarkupClient : IMarkupService
    {
        private IMarkupService _service;
        private ILocalCache<string> _localCache;

        private const string ServiceUrlKey = "MarkupServiceUrl";

        public MarkupClient()
        {
            _localCache = new LocalCache<string>();
        }

        public MarkupClient(IMarkupService service)
        {
            _service = service;
            _localCache = new LocalCache<string>();
        }

        public MarkupClient(IMarkupService service, ILocalCache<string> localCache)
        {
            _service = service;
            _localCache = localCache;
        }

        public string Get(ContentTypes section)
        {
            try
            {
                var content = MarkupService.Get(section);
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

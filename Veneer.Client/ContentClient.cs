using System;
using System.Configuration;
using Veneer.Client.Caching;
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
        private ILocalCache _localCache;

        public ContentClient()
        {
            _localCache = new LocalCache();
        }

        public ContentClient(IContentService service, ILocalCache localCache)
        {
            _service = service;
            _localCache = localCache;
        }

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
            try
            {
                var content = ContentService.Get(section);
                if (content != null)
                {
                    _localCache.WriteToCache(section, content);
                    return content;
                }
                return _localCache.ReadFromCache(section);
            }
            catch (Exception)
            {
                return _localCache.ReadFromCache(section);
            }
        }
    }
}

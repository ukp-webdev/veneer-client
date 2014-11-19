using System;
using System.Configuration;
using System.ServiceModel.Web;
using Veneer.Client.Caching;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;

namespace Veneer.Client
{
       
    public class ContentClient : IContentService
    {
        private IContentService _service;
        private readonly ILocalCache<Content> _localCache;
        
        private const string ServiceUrlKey = "ContentServiceUrl"; 

        public ContentClient()
        {
            _localCache = new LocalCache<Content>();
        }

        public ContentClient(IContentService service, ILocalCache<Content> localCache)
        {
            _service = service;
            _localCache = localCache;
        }

        public Content Get(ContentTypes section)
        {
            try
            {
                var content = ContentService.Get(section);
                if (content != null)
                {
                    _localCache.WriteToCache(section, content, content.RefreshDate);
                    return content;
                }
                return _localCache.ReadFromCache(section);
            }
            catch (Exception)
            {
                return _localCache.ReadFromCache(section);
            }
        }

        private IContentService ContentService
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
    }
}

﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Web;
using Veneer.Client.Caching;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;
using Veneer.Contracts.ServiceContracts;

namespace Veneer.Client
{
       
    public class StylesClient : IStylesService
    {
        private IStylesService _service;
        private ILocalCache<List<ContentStyle>> _localCache;
        private const string ServiceUrlKey = "StylesServiceUrl";
        
        public StylesClient()
        {
            _localCache = new LocalCache<List<ContentStyle>>();
        }

        public StylesClient(IStylesService service)
        {
            _service = service;
            _localCache = new LocalCache<List<ContentStyle>>();
        }

        public StylesClient(IStylesService service, ILocalCache<List<ContentStyle>> localCache)
        {
            _service = service;
            _localCache = localCache;
        }

        public List<ContentStyle> Get(ContentTypes section)
        {
            try
            {
                var content = StylesService.Get(section);
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

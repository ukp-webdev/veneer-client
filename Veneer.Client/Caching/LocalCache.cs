using System;
using System.Collections.Generic;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Caching
{
    public class LocalCache : ILocalCache
    {        
        private readonly Dictionary<string, DateTime> _contentRefreshTimes = new Dictionary<string, DateTime>();
        private readonly IStorageHandler _storageHandler;

        public LocalCache()
        {
            _storageHandler = new StorageHandler();      
        }

        public LocalCache(IStorageHandler storageHandler)
        {
            _storageHandler = storageHandler;
        }

        public void WriteToCache(ContentTypes contentType, Content content)
        {
            DateTime? lastRefreshDate;
            try
            {
                lastRefreshDate = _contentRefreshTimes[contentType.ToString()];
            }
            catch (KeyNotFoundException)
            {
                lastRefreshDate = null;
            }

            if (!lastRefreshDate.HasValue || lastRefreshDate.Value < content.RefreshDateTime)
            {
                _storageHandler.WriteToStorage(contentType, content);
                _contentRefreshTimes[contentType.ToString()] = content.RefreshDateTime;
            }
        }

        public Content ReadFromCache(ContentTypes contentType)
        {
            return _storageHandler.ReadFromStorage(contentType);
        }
    }
}

using System;
using System.Collections.Generic;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Caching
{
    public class LocalCache<T> : ILocalCache<T>
    {        
        private readonly Dictionary<string, DateTime> _contentRefreshTimes = new Dictionary<string, DateTime>();
        private readonly IStorageHandler<T> _storageHandler;

        public LocalCache()
        {
            _storageHandler = new StorageHandler<T>();      
        }

        public LocalCache(IStorageHandler<T> storageHandler)
        {
            _storageHandler = storageHandler;
        }

        public void WriteToCache(ContentTypes contentType, T content, DateTime refreshDate)
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

            if (!lastRefreshDate.HasValue || lastRefreshDate.Value < refreshDate)
            {
                _storageHandler.WriteToStorage(contentType, content);
                _contentRefreshTimes[contentType.ToString()] = refreshDate;
            }
        }

        public T ReadFromCache(ContentTypes contentType)
        {
            return _storageHandler.ReadFromStorage(contentType);
        }
    }
}

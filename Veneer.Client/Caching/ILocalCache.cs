using System;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Caching
{
    public interface ILocalCache<T>
    {
        void WriteToCache(ContentTypes contentType, T content, DateTime refreshDate);
        T ReadFromCache(ContentTypes contentType);
    }
}

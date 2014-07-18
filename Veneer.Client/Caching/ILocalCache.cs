
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Caching
{
    public interface ILocalCache
    {
        void WriteToCache(ContentTypes contentType, Content content);
        Content ReadFromCache(ContentTypes contentType);
    }
}

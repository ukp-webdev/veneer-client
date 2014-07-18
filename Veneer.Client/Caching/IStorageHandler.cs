
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Caching
{
    public interface IStorageHandler
    {
        void WriteToStorage(ContentTypes contentType, Content content);
        Content ReadFromStorage(ContentTypes contentType);
    }
}

using Veneer.Contracts.Enums;

namespace Veneer.Client.Caching
{
    public interface IStorageHandler<T>
    {
        void WriteToStorage(ContentTypes contentType, T content);
        T ReadFromStorage(ContentTypes contentType);
    }
}

using System;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Veneer.Contracts.Enums;

namespace Veneer.Client.Caching
{
    public class StorageHandler<T> : IStorageHandler<T>
    {
        public void WriteToStorage(ContentTypes contentType, T content)
        {
            var fileName = GenerateFilenameAndPath(contentType.ToString());
            if (!String.IsNullOrEmpty(fileName))
            {                
                var serialisedContent = JsonConvert.SerializeObject(content);
                File.WriteAllText(fileName, serialisedContent);                
            }
        }

        public T ReadFromStorage(ContentTypes contentType)
        {
            var fileName = GenerateFilenameAndPath(contentType.ToString());
            if (!String.IsNullOrEmpty(fileName))
            {
                var serialisedContent = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<T>(serialisedContent);
            }
            return default(T);
        }

        private static string GenerateFilenameAndPath(string cacheKey)
        {
            var cacheFolder = ConfigurationManager.AppSettings["LocalCacheFolder"];
            if (!String.IsNullOrEmpty(cacheFolder))
            {
                return string.Format("{0}\\ContentCache-{1}.cache", cacheFolder, cacheKey);
            }
            return null;
        }
    }
}

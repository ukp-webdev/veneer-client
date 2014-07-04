using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Veneer.Contracts.DataContracts
{
    [DataContract]
    public class Content
    {
        public Content()
        {
            Sections = new List<ContentSection>();
        }

        [DataMember]
        public List<ContentSection> Sections { get; set; }
    }
}

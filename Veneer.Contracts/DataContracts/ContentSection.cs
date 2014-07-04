using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Veneer.Contracts.DataContracts
{
    [DataContract]
    public class ContentSection
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public List<ContentStyle> Styles { get; set; }

        [DataMember]
        public List<ContentScript> Scripts { get; set; }

        [DataMember]
        public string Html { get; set; }
    }
}

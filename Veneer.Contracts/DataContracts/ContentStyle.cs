
using System.Runtime.Serialization;

namespace Veneer.Contracts.DataContracts
{
    [DataContract]
    public class ContentStyle
    {
        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string Html { get; set; }
    }
}

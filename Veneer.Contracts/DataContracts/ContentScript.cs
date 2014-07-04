
using System;
using System.Runtime.Serialization;

namespace Veneer.Contracts.DataContracts
{
    [DataContract]
    public class ContentScript
    {
        [DataMember]
        public Uri Url { get; set; }

        [DataMember]
        public string Html { get; set; }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(Html))
            {
                if (Url == null)
                {
                    return string.Empty;
                }
                return string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", Url);
            }
            return Html;
        }
    }
}

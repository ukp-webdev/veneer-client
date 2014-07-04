
using System;
using System.Runtime.Serialization;

namespace Veneer.Contracts.DataContracts
{
    [DataContract]
    public class ContentStyle
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
                return string.Format("<style type=\"text/css\" src=\"{0}\"></style>", Url);
            }
            return Html;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
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

        [DataMember]
        public string RefreshDate { get; set; }

        // when Web API and WCF can agree how to serialize JSON dates, this property can be removed.
        public DateTime RefreshDateTime
        {
            get
            {
                if (String.IsNullOrEmpty(RefreshDate))
                    return default(DateTime);

                var dateTime = DateTime.Parse(RefreshDate);
                return dateTime;
            }
            set { RefreshDate = value.ToString(CultureInfo.InvariantCulture); }
        }
    }
}

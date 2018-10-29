using System;
using System.Xml.Serialization;

namespace SerkoTest.Models
{
    [XmlRoot("text_detail")]
    public class TextDetail
    {
        [XmlElement("expense")]
        public Expense Expense { get; set; }

        [XmlElement("vendor")]
        public string Vendor { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("date")]
        public string Date { get; set; }
    }
}

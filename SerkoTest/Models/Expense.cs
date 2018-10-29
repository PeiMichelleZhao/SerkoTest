using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SerkoTest.Models
{
    public class Expense
    {

        [XmlElement("cost_centre")]
        [DefaultValue("UNKNOWN")]
        public string CostCenter { get; set; }


        [XmlElement("total")]
        [Required(ErrorMessage = "Total is required")]
        public float Total { get; set; }

        public float GST { get; set; }

        public float Cost { get; set; }

        [XmlElement("payment_method")]
        public string PaymentMethod { get; set; }
    }
}

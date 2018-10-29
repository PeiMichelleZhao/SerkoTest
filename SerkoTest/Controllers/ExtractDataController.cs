using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using SerkoTest.Exceptions;
using SerkoTest.Models;
using SerkoTest.Services;

namespace SerkoTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtractDataController : ControllerBase
    {

        private readonly IExtractDataServices _services;

        public ExtractDataController(IExtractDataServices services){
            _services = services;
        }

        // POST api/values
        [HttpPost]
        public ActionResult<TextDetail> Post([FromBody] Message message)
        {
            // tax rate
            float taxRate = 0.15f;
            string data = "";

            // extract xml section
            try
            {
                data = _services.ExtractData(message.Text);
            }
            catch (NoCloseTagException ex) 
            {
                Console.Write(ex.ToString());
                return StatusCode(404, "item does not have close tag!");
            }

            // target object
            TextDetail textDetail;

            // add root for xml
            data = "<text_detail>" + data + "</text_detail>";

            // serialize xml to object
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(TextDetail));

                // use xml to check if total is exist
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(data);

                XmlNode totalNode = xml.SelectSingleNode("/text_detail/expense/total");
                if (totalNode == null)
                {
                    return StatusCode(404, "Total is requied!");
                }

                // convert xml to object
                using (TextReader reader = new StringReader(data))
                {
                    textDetail = (TextDetail)serializer.Deserialize(reader);
                }
            }
            catch (XmlException ex)
            {
                Console.Write(ex.ToString());
                return StatusCode(404, "Data block is not well formated!");
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return StatusCode(404, "Data block is not well formated!");
            }

            // set CostCenter default value
            if (textDetail.Expense.CostCenter == null)
            {
                textDetail.Expense.CostCenter = "UNKNOWN";
            }

            // calculate GST
            if (textDetail.Expense.Total > 0)
            {
                textDetail.Expense.GST = textDetail.Expense.Total * taxRate / 1 + taxRate;
                textDetail.Expense.Cost = textDetail.Expense.Total - textDetail.Expense.GST;
            }

            return textDetail;
        }
    }
}

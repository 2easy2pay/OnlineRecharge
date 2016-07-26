using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class InternationalRechargeAPIResponseDetails
    {
        public InternationalRechargeAPIResponseDetails()
        {

        }

        public int ID { get; set; }
        public string Response { get; set; }
        public string ResponseDescription { get; set; }
        public string PaymentID { get; set; }
        public string PaymentRef { get; set; }
        public DateTime Date { get; set; }
        public int InternationalRechargeID { get; set; }
        [ForeignKey("InternationalRechargeID")]
        public InternationalRecharges InternationalRecharge { get; set; }
    }
}
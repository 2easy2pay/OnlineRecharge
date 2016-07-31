using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class ShoppingCardPaymentDetails
    {
        public ShoppingCardPaymentDetails()
        {

        }
        public int ID { get; set; }
        public string PaymentID { get; set; }
        public string Result { get; set; }
        public string TrackID { get; set; }
        public string TransID { get; set; }
        public string Ref { get; set; }

        public int ShoppingCardID { get; set; }
        [ForeignKey("ShoppingCardID")]
        public ShoppingCards ShoppingCards { get; set; }
    }
}
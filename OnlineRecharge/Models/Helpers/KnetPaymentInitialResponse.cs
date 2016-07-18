using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Helpers
{
    public class KnetPaymentInitialResponse
    {
        public string Status { get; set; }
        public string message { get; set; }
        public string URL { get; set; }
        public string PaymentID { get; set; }
    }
}
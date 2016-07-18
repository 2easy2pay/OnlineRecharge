using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Helpers
{
    public class KnetPaymentInitiate
    {
        public string Amt { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string PaymentType { get; set; }
        public string Udf1 { get; set; }

        public string returnUrl { get; set; }
        public string errorUrl { get; set; }
    }
}
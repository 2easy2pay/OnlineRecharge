using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class NationalRecharges : BaseEntity
    {
        public NationalRecharges()
        {

        }
        public int CustomerID { get; set; }
        public string MobileNumber { get; set; }
        public decimal amount { get; set; }

        public NationalRechargePaymentDetails NationalRechargePaymentDetail { get; set; }

        public NationalRechargeAPIResponseDetails NationalRechargeAPIResponseDetail { get; set; }

        public virtual NationalRechargeTypes RechargeType { get; set; }
        public virtual ServiceProviders ServiceProvider { get; set; }
    }
}
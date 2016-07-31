using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class RechargeParameters : BaseEntity
    {
        public RechargeParameters()
        {

        }
        public string ServiceType { get; set; }
        public string RechargeType { get; set; }
        public string OperatorCode { get; set; }
        public string MobileNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
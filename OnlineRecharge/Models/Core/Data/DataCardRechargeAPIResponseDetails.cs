﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class DataCardRechargeAPIResponseDetails
    {
        public DataCardRechargeAPIResponseDetails()
        {

        }

        public int ID { get; set; }
        public string Response { get; set; }
        public string ResponseDescription { get; set; }
        public string PaymentID { get; set; }
        public string PaymentRef { get; set; }
        public DateTime Date { get; set; }

        //Voucher API Response Fields
        public decimal Denomination  { get; set; }
        public string OperatorName { get; set; }
        public string Password { get; set; }
        
        public string RechargeCode { get; set; }
        public string SerialNo { get; set; }
        public int DataCardRechargeID { get; set; }
        [ForeignKey("DataCardRechargeID")]
        public DataCardRecharges DataCardRecharge { get; set; }
    }
}
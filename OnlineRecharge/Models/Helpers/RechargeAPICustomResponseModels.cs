using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Helpers
{
    public class VoucherDetailsModel
    {
        public string OperatorCode { get; set; }
        public decimal Amount { get; set; }
    }
    public class VoucherResponseModel
    {
        public string ImageURL { get; set; }
        public string OperatorCode { get; set; }
        public decimal Amount { get; set; }
    }
    public class OperatorResponseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class RechargeParamModel
    {
        public string ServiceType { get; set; }
        public string RechargeType { get; set; }
        public string OperatorCode { get; set; }
        public string MobileNumber { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransferResponseDetailsModel
    {
        public string Response { get; set; }


        public string ResponseDescription { get; set; }


        public string PaymentID { get; set; }

        public string PaymentRef { get; set; }

        public DateTime Date { get; set; }
        public string ImageURL { get; set; }
        public string OperatorName { get; set; }
        public decimal  Amount { get; set; }
        //Voucher Transfer Response
        public string RechargeCode { get; set; }
        public decimal Denomination { get; set; }
        public string Password { get; set; }
        public string SerialNo { get; set; }
    }

    public class InternationalServiceProvidersModel
    {

        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class RechargeAPICustomResponseModels
    {
    }
}
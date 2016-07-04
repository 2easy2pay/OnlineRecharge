using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Helpers
{
    #region data contracts

    class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
    }
    public class Service
    {

        public string OperatorName { get; set; }

        public string OperatorType { get; set; }

        public bool Billpayment { get; set; }

        public bool Topup { get; set; }

        public bool Card { get; set; }

        public List<Denomination> DenomCollection { get; set; }
    }

    public class Denomination
    {

        public decimal Denom { get; set; }
    }


    public class VoucherValidation
    {

        public string Result { get; set; }
    }


    public class VoucherTransfer
    {

        public string Response { get; set; }


        public string OperatorName { get; set; }


        public decimal Denomination { get; set; }

        public string SerialNo { get; set; }

        public string RechargeCode { get; set; }

        public string Password { get; set; }
    }

    //    HISTORY_RES,Response,Card Count Status,Card details 1;                                                    card details 2;card details 3
    //HISTORY_RES,S,         Y,              90006855:EZ:20.000:14894220041:48291553740563:11/22/2015 4:44:23 
    //CARD DETAILS – ACCOUNT NO:CARD TYPE: DENOM: SERIAL NUMBER: RECHARGE CODE:   DATE
    //           90006855:   EZ:      20.000: 14894220041:   48291553740563: 11/22/2015 4:44:23

    public class HistoryData
    {

        public string Response { get; set; }


        public string CardCountStatus { get; set; }

        public List<CardDetails> CardDetails { get; set; }

    }

    public class CardDetails
    {

        public string AccountNo { get; set; }


        public string CardType { get; set; }

        public string Denomination { get; set; }

        public string SerialNumber { get; set; }

        public string RechargeCode { get; set; }

        public DateTime Date { get; set; }


    }





    public class TopupValidation
    {

        public string Response { get; set; }


        public string ResponseDescription { get; set; }


        public string PaymentID { get; set; }

        public string PaymentRef { get; set; }

        public DateTime Date { get; set; }

    }


    public class InternationalTopupValidation
    {

        public string Response { get; set; }


        public string ResponseDescription { get; set; }


        public string PaymentID { get; set; }

        public string PaymentRef { get; set; }

        public DateTime Date { get; set; }


        public string AmountType { get; set; }

        public string AmountValue { get; set; }

    }
    //    Response from the server
    // DENOM_RESPONSE,Response,Operator Name,Product Id 1:Product 1 Amount:Product 1 Name;Product Id 2:Product 2 Amount:Product 2 Name
    //Ex: 
    //DENOM_RESPONSE,S,BS,196:56.000:Sports Package- 1 year (Renew);197:69.000:Global Package 1 Year (Renew);193:70.000:Sports 1 Year + Decoder (New);192:83.000:Global Package + Decoder (New)


    public class DenomValidation
    {

        public string Response { get; set; }


        public string OperatorName { get; set; }




        public List<DenominationProduct> Products { get; set; }
    }

    public class DenominationProduct
    {

        public string ProductID { get; set; }

        public decimal ProductAmount { get; set; }

        public string ProductName { get; set; }
    }

    //    TOPUP_RES,Response,Response Description,Payment ID,Payment Ref,Date

    //EX: TOPUP_RES,S,1_SUCCESS,0013950672,105143065,3/17/2015 11:26:17 AM



    public class ServiceTopup
    {

        public string Response { get; set; }


        public string ResponseDescription { get; set; }


        public string PaymentID { get; set; }

        public string PaymentRef { get; set; }


        public DateTime Date { get; set; }


    }


    public class InternationalTopupCheck
    {

        public string Response { get; set; }


        public string ResponseDescription { get; set; }


        public string Amount { get; set; }

        public string LocalCountryAmount { get; set; }


        public string LocalCountryCurrecy { get; set; }


        public string LocalCountryAmountAfterText { get; set; }


    }


    public class TopupTransfer
    {

        public string Response { get; set; }


        public string ResponseDescription { get; set; }


        public string PaymentID { get; set; }

        public string PaymentRef { get; set; }

        public DateTime Date { get; set; }

    }


    public class InternationalTopupTransfer
    {

        public string Response { get; set; }


        public string ResponseDescription { get; set; }


        public string PaymentID { get; set; }

        public string PaymentRef { get; set; }

        public DateTime Date { get; set; }

    }


    public class BillPaymentValidation
    {

        public string Response { get; set; }


        public string ResponseDescription { get; set; }


        public string PaymentID { get; set; }

        public string PaymentRef { get; set; }

        public DateTime Date { get; set; }


        public decimal Total { get; set; }

        public decimal Billed { get; set; }

        public Decimal UnBilled { get; set; }

    }



    public class BillPaymentTransfer
    {

        public string Response { get; set; }


        public string ResponseDescription { get; set; }


        public string PaymentID { get; set; }

        public string PaymentRef { get; set; }

        public DateTime Date { get; set; }

    }

    public class ClientCredentials
    {

        public string UserName { get; set; }


        public string Password { get; set; }

    }
    public enum EnumInterkeySubscribeAuthorizeReturnCode : int
    {

        Subscriber_found_Success = 0,
        Subscriber_not_found = 1,
        Server_Side_Database_Failure_Or_Network_Failure = 9
    }

    //Ret_Code,Sb_Id,Sb_Status,Sb_Credit,Sb_Inuse,Sb_Type
    public class InterkeySubscribeAuthorize
    {

        public string Id { get; set; }

        public string Status { get; set; }

        public string Credit { get; set; }

        public string Inuse { get; set; }

        public string Type { get; set; }

        public EnumInterkeySubscribeAuthorizeReturnCode ReturnCodeEnum { get; set; }

        public int ReturnCode { get; set; }

    }
    //IK_SUBSCRIBER_RELEASE_RES,RET_CODE,SB_ID

    public class InterkeySubscriberRelease
    {


        public EnumInterkeySubscribeAuthorizeReturnCode ReturnCodeEnum { get; set; }

        public int ReturnCode { get; set; }

        public string Id { get; set; }
    }

    //    IK_SUBSCRIBER_UPDATE_RES,RET_CODE,SB_ID

    //E.g.: IK_SUBSCRIBER_UPDATE_RES,0,8682725934

    public class InterkeySubscriberUpdate
    {

        public string Id { get; set; }


        public EnumInterkeySubscribeAuthorizeReturnCode ReturnCodeEnum { get; set; }

        public int ReturnCode { get; set; }



        public string Credit { get; set; }
    }
    #endregion
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Helpers
{
    public class dn_knet_response
    {
        public string paymentID { get; set; }

        public string result { get; set; }

        public string postdate { get; set; }

        public string tranid { get; set; }

        public string auth { get; set; }

        public string trackid { get; set; }

        public string refr { get; set; }

        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
    }

    public class OrderPaymentDetails
    {
        public string tranid { get; set; }
        public string PaymentID { get; set; }
        public string trackid { get; set; }
        public string refr { get; set; }
    }
    public class KNetReponse
    {
        public string SucessURL = "https://staging.2easy2pay.com/orderdetails";
        public string ErrrorURL = "https://staging.2easy2pay.com/paymenterror";
        public dn_knet_response ReadResponse()
        {
            HttpRequest Request = HttpContext.Current.Request;
            dn_knet_response Response = new dn_knet_response();
            Response.paymentID = Request["PaymentID"]; // Reads the value of the Payment ID passed by GET request by the user.
            Response.result = Request["Result"]; // Reads the value of the Result passed by GET request by the user.
            Response.postdate = Request["PostDate"]; // Reads the value of the PostDate passed by GET request by the user.
            Response.tranid = Request["TranID"]; // Reads the value of the TranID passed by GET request by the user.
            Response.auth = Request["Auth"]; // Reads the value of the Auth passed by GET request by the user.
            Response.refr = Request["Ref"]; // Reads the value of the Ref passed by GET request by the user.
            Response.trackid = Request["TrackID"];  // Reads the value of the TrackID passed by GET request by the user.
            Response.udf1 = Request["UDF1"];  // Reads the value of the UDF1 passed by GET request by the user.

            return Response;
        }


        public dn_knet_response ReadErrorResponse()
        {
            HttpRequest Request = HttpContext.Current.Request;
            dn_knet_response res = new dn_knet_response();

            res.paymentID = Request["paymentID"];
            res.result = Request["result"];
            res.postdate = Request["postdate"];
            res.tranid = Request["tranid"];
            res.auth = Request["auth"];
            res.refr = Request["ref"];
            res.trackid = Request["trackid"];

            res.udf1 = Request["udf1"];
            res.udf2 = Request["udf2"];
            res.udf3 = Request["udf3"];
            res.udf4 = Request["udf4"];
            res.udf5 = Request["udf5"];

            return res;
        }



        public string GenerateResponse(out string result)
        {
            HttpRequest Request = HttpContext.Current.Request;
            string paymentID, postdate, tranid, auth, trackid, refr, udf1, udf2, udf3, udf4, udf5;

            paymentID = Request.Form["paymentID"];
            result = Request.Form["result"];
            postdate = Request.Form["postdate"];
            tranid = Request.Form["tranid"];
            auth = Request.Form["auth"];
            refr = Request.Form["ref"];
            trackid = Request.Form["trackid"];

            udf1 = Request.Form["udf1"];
            udf2 = Request.Form["udf2"];
            udf3 = Request.Form["udf3"];
            udf4 = Request.Form["udf4"];
            udf5 = Request.Form["udf5"];
            // ResponseObj = new dn_knet_response();
            /*
            ResponseObj.paymentID = paymentID;
            ResponseObj.auth = auth;
            ResponseObj.postdate = postdate;
            ResponseObj.refr = refr;
            ResponseObj.result = result;
            ResponseObj.trackid = trackid;
            ResponseObj.tranid = tranid;
            ResponseObj.udf1 = udf1;
            ResponseObj.udf2 = udf2;
            ResponseObj.udf3 = udf3;
            ResponseObj.udf4 = udf4;
            ResponseObj.udf5 = udf5;
            */


            string Response = "";
            if (result == "CAPTURED")
            {
                Response = SucessURL + "/" + udf1
                    + "/?PaymentID=" + paymentID
                    + "&Result=" + result
                    + "&PostDate=" + postdate
                    + "&TranID=" + tranid
                    + "&Auth=" + auth
                    + "&Ref=" + refr
                    + "&TrackID=" + trackid
                    + "&UDF1=" + udf1
                    + "&UDF2=" + udf2
                    + "&UDF3=" + udf3
                    + "&UDF4=" + udf4
                    + "&UDF5=" + udf5;
            }
            else if (result == "CANCELED")
            {
                Response = "REDIRECT=https://staging.2easy2pay.com/";
            }
            else
            {
                Response = ErrrorURL + "/" + udf1
                    + "/?PaymentID=" + paymentID
                    + "&Result=" + result
                    + "&PostDate=" + postdate
                    + "&TranID=" + tranid
                    + "&Auth=" + auth
                    + "&Ref=" + refr
                    + "&TrackID=" + trackid
                    + "&UDF1=" + udf1
                    + "&UDF2=" + udf2
                    + "&UDF3=" + udf3
                    + "&UDF4=" + udf4
                    + "&UDF5=" + udf5;
            }
            return Response;
        }
    }
}
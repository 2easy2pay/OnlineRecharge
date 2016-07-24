using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OnlineRecharge.Models.Core.Data;
using OnlineRecharge.Models.Core;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using OnlineRecharge.Models.Helpers;
using Newtonsoft.Json;
using System.Net;

namespace OnlineRecharge.Controllers
{
    public class HomeController : Controller
    {
        public const string BASEADDRESS = "https://grcweb.grckiosk.com:8443/";
        public const string USERNAME = "101";
        public const string PASSWORD = "000";
        #region Fields
        EFDbContext context = new EFDbContext();
        #endregion

        #region Action Methods
        public ActionResult Index()
        {
            try
            {
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetAllVouchers()
        {
            var resp = await this.GetAllOperatorVouchers();
            return this.Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProcessKnetPayment()
        {
            KnetPaymentInitiate request = new KnetPaymentInitiate();
            request.Amt = Request.Form["amount"];
            request.ContactNumber = Request.Form["contactNumber"];
            request.Email = Request.Form["email"];
            request.PaymentType = Request.Form["paymentType"];
            request.returnUrl = Request.Form["returnUrl"];
            request.errorUrl = Request.Form["errorUrl"];
            request.Udf1 = Request.Form["Udf1"];
            KnetPaymentInitialResponse response = new KnetPaymentInitialResponse();
            var httpClient = new HttpClient();
            var url = "https://api.2easy2pay.com/test/Gateway";
            var result = httpClient.PostAsJsonAsync(url, request).Result;
            if (result.IsSuccessStatusCode)
            {
                response = result.Content.ReadAsAsync<KnetPaymentInitialResponse>().Result;
            }
            return this.Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> TranferNationalTopup()
        {
            var resp = await this.TopupTransfer();
            return this.Json(resp, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Recharge API Service Methods

        [HttpPost]
        public async Task<List<VoucherDetailsModel>> GetAllOperatorVouchers()
        {
            try
            {
                List<VoucherDetailsModel> vouchers = new List<VoucherDetailsModel>();
                using (var client = new HttpClient())
                {

                    #region login and get token
                    var logindata = string.Format("grant_type=password&username={0}&password={1}", USERNAME, PASSWORD);//LOGIN DATA

                    var url = BASEADDRESS + "token";

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(logindata, Encoding.UTF8, "application/x-www-form-urlencoded");

                    var resp = await client.PostAsync(url, request.Content);
                    Token token = new Token();
                    if (resp.IsSuccessStatusCode)
                    {
                        token = await resp.Content.ReadAsAsync<Token>();
                    }
                    #endregion login and get token

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);
                        httpClient.BaseAddress = new Uri(BASEADDRESS);

                        // New code:
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/GetServiceList");
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<List<Service>>();

                            List<Service> operators = result.Where(x => x.OperatorType == "EZ" || x.OperatorType == "VV" || x.OperatorType == "XP").ToList();

                            foreach (var item in operators)
                            {
                                foreach (var card in item.DenomCollection)
                                {
                                    VoucherDetailsModel model = new VoucherDetailsModel();
                                    if (item.OperatorType == "EZ")
                                    {
                                        model.ImageURL = "/Content/img/Operators/zain.png";
                                    }
                                    else if (item.OperatorType == "VV")
                                    {
                                        model.ImageURL = "/Content/img/Operators/viva.png";
                                    }
                                    else if (item.OperatorType == "XP")
                                    {
                                        model.ImageURL = "/Content/img/Operators/ooreedo.png";
                                    }
                                    model.Amount = card.Denom;
                                    model.OperatorCode = item.OperatorType;
                                    vouchers.Add(model);
                                }
                            }

                        }

                    }
                }
                return vouchers;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            
        }


        [HttpPost]
        public async Task<TopupTransferResponseDetailsModel> TopupTransfer()
        {
            try
            {
                TopupTransferResponseDetailsModel model = new TopupTransferResponseDetailsModel();
                string rechargeType = Request.Form["rechargeType"];
                string operatorName = Request.Form["operatorCode"];
                string mobileNumber = Request.Form["mobileNumber"];
                string amount = Request.Form["amount"];
                string paymentID = Request.Form["paymentID"];
                string status = Request.Form["result"];
                string trackID = Request.Form["trackID"];
                string tranID = Request.Form["tranID"];
                string reference = Request.Form["ref"];
                using (var client = new HttpClient())
                {
                    #region login and get token
                    var logindata = string.Format("grant_type=password&username={0}&password={1}", USERNAME, PASSWORD);//LOGIN DATA

                    var url = BASEADDRESS + "token";

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(logindata, Encoding.UTF8, "application/x-www-form-urlencoded");

                    var resp = client.PostAsync(url, request.Content);
                    Token token = new Token();
                    if (resp.Result.IsSuccessStatusCode)
                    {
                        token = await resp.Result.Content.ReadAsAsync<Token>();
                    }
                    #endregion login and get token
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);
                        httpClient.BaseAddress = new Uri(BASEADDRESS);
                        string data = string.Format("?OperatorName={0}&AmtSelected={1}&MobileNumber={2}&PaymentType={3}",
                             operatorName, amount, mobileNumber, "CASH");
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/TopupTransfer" + data);

                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<TopupTransfer>();
                            model.Amount = amount;
                            model.Date = result.Date;
                            if (operatorName == "EZ")
                            {
                                model.ImageURL = "/Content/img/Operators/zain.png";
                            }
                            else if (operatorName == "VV")
                            {
                                model.ImageURL = "/Content/img/Operators/viva.png";
                            }
                            else if (operatorName == "XP")
                            {
                                model.ImageURL = "/Content/img/Operators/ooreedo.png";
                            }

                            model.OperatorName = GetOperatorNameByOperatorCode(operatorName);
                            model.PaymentID = result.PaymentID;
                            model.PaymentRef = result.PaymentRef;
                            model.Response = result.Response;
                            model.ResponseDescription = result.ResponseDescription;
                           int id= UpdateRechargeDetailsToDB(mobileNumber,Convert.ToDecimal(amount), rechargeType, operatorName,paymentID, status, trackID, tranID, reference,result);
                        }
                       

                    }
                }
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region DataAccess Methods
        public JsonResult GetServiceProviders()
        {
            List<ServiceProviders> model = new List<ServiceProviders>();
            try
            {
                model = context.ServiceProiders.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

        public string GetOperatorNameByOperatorCode(string operatorCode)
        {
            string operatorName = string.Empty;
            try
            {
                operatorName = context.ServiceProiders.Where(x => x.Code == operatorCode).Select(v => v.Name).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return operatorName;
        }

        public int UpdateRechargeDetailsToDB(string mobileNumber,decimal amount,string rechargeType,string operatorCode,string paymentID,string status,string trackID,string tranID,string reference, TopupTransfer result)
        {
            var model = context.NationalRecharges.Create();
            try
            {
                
                model.MobileNumber = mobileNumber;
                model.amount = amount;
                model.ServiceProvider = context.ServiceProiders.Where(x => x.Code == operatorCode).FirstOrDefault();
                model.RechargeType = context.NationalRechargeTypes.Where(x => x.Name.Equals(rechargeType, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                model.IsActive = true;
                model.IsDeleted = false;
                model.CreatedBy = 1;
                model.CreatedDate = DateTime.Now;
                model.CustomerID = 1;
                context.NationalRecharges.Add(model);
                context.SaveChanges();

                var paymentDetail = context.NationalRechargePaymentDetails.Create();
                paymentDetail.PaymentID = paymentID;
                paymentDetail.Result = status;
                paymentDetail.TrackID = trackID;
                paymentDetail.TransID = tranID;
                paymentDetail.Ref = reference;
                paymentDetail.NationalRecharge = model;
                context.NationalRechargePaymentDetails.Add(paymentDetail);

                var apiResponseDetail = context.NationalRechargeAPIResponseDetails.Create();
                apiResponseDetail.NationalRecharge = model;
                apiResponseDetail.PaymentID = result.PaymentID;
                apiResponseDetail.PaymentRef = result.PaymentRef;
                apiResponseDetail.Response = result.Response;
                apiResponseDetail.ResponseDescription = result.ResponseDescription;
                apiResponseDetail.Date = result.Date;
                context.NationalRechargeAPIResponseDetails.Add(apiResponseDetail);
                context.SaveChanges();
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return model.ID;
        }
        #endregion

        #region Helping Methods


       
      

        [HttpPost]
        public async Task<TopupValidation> TopupValidation()
        {
            try
            {
                string rechargeType = Request.Form["rechargeType"];
                string operatorCode = Request.Form["operatorCode"];
                string mobileNumber = Request.Form["mobileNumber"];
                string amount = Request.Form["amount"];
                string result=string.Empty;
                using (var client = new HttpClient())
                {

                    #region login and get token
                    var logindata = string.Format("grant_type=password&username={0}&password={1}", USERNAME, PASSWORD);//LOGIN DATA

                    var url = BASEADDRESS + "token";

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(logindata, Encoding.UTF8, "application/x-www-form-urlencoded");

                    var resp = await client.PostAsync(url, request.Content);
                    Token token = new Token();
                    if (resp.IsSuccessStatusCode)
                    {
                        token = await resp.Content.ReadAsAsync<Token>();
                    }
                    #endregion login and get token

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);
                        httpClient.BaseAddress = new Uri(BASEADDRESS);
                       var data = string.Format("?OperatorName={0}&Amount={1}&MobileNumber={2}", operatorCode, amount, mobileNumber);
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/TopupValidation" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            result = await response.Content.ReadAsStringAsync(); //response.Content.ReadAsAsync<TopupValidation>();
                        }
                        else
                        {

                        }

                    }
                }
                var test= JsonConvert.DeserializeObject<TopupValidation>(result) as TopupValidation;
                return test;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public async Task TopupTransfer()
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            #region login and get token
        //            var logindata = string.Format("grant_type=password&username={0}&password={1}", USERNAME, PASSWORD);//LOGIN DATA

        //            var url = BASEADDRESS + "token";

        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
        //            request.Content = new StringContent(logindata, Encoding.UTF8, "application/x-www-form-urlencoded");

        //            var resp = await client.PostAsync(url, request.Content);
        //            Token token = new Token();
        //            if (resp.IsSuccessStatusCode)
        //            {
        //                token = await resp.Content.ReadAsAsync<Token>();
        //            }
        //            #endregion login and get token
        //            using (var httpClient = new HttpClient())
        //            {
        //               string data = string.Format("?OperatorName={0}&AmtSelected={1}&MobileNumber={2}&PaymentType={3}",
        //                    "VV", "1.000", "55155445", "CASH");
        //                HttpResponseMessage response = await httpClient.GetAsync("api/Services/TopupTransfer" + data);

        //                if (response.IsSuccessStatusCode)
        //                {
        //                    var result = await response.Content.ReadAsAsync<TopupTransfer>();
        //                }
        //                else
        //                {
        //                    //MessageBox.Show(response.ReasonPhrase);
        //                }
                     
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public async Task ValidateTopup()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    #region login and get token
                    var logindata = string.Format("grant_type=password&username={0}&password={1}", USERNAME, PASSWORD);//LOGIN DATA

                    var url = BASEADDRESS + "token";

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(logindata, Encoding.UTF8, "application/x-www-form-urlencoded");

                    var resp = await client.PostAsync(url, request.Content);
                    Token token = new Token();
                    if (resp.IsSuccessStatusCode)
                    {
                        token = await resp.Content.ReadAsAsync<Token>();
                    }
                    #endregion login and get token
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);
                        httpClient.BaseAddress = new Uri(BASEADDRESS);
                        string data = string.Format("?OperatorName={0}&Amount={1}&MobileNumber={2}", "VV", "1.000", "55155445");
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/TopupValidation" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<TopupValidation>();
                        }
                        else
                        {
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        
        #endregion

        #region Test Api Service
        private static async Task TestWebApiAllServices()
        {

            // Start OWIN host 

            try
            {
                using (var client = new HttpClient())
                {

                    #region login and get token
                    var logindata = string.Format("grant_type=password&username={0}&password={1}", USERNAME, PASSWORD);//LOGIN DATA

                    var url = BASEADDRESS + "token";

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(logindata, Encoding.UTF8, "application/x-www-form-urlencoded");

                    var resp = await client.PostAsync(url, request.Content);
                    Token token = new Token();
                    if (resp.IsSuccessStatusCode)
                    {
                        token = await resp.Content.ReadAsAsync<Token>();
                    }
                    #endregion login and get token
                    #region call services test


                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);
                        httpClient.BaseAddress = new Uri(BASEADDRESS);

                        // New code:
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/GetServiceList");
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<List<Service>>();
                        }




                        //TestWebServiceClient.ServiceReference2.InternationalTopupValidation obj = (TestWebServiceClient.ServiceReference2.InternationalTopupValidation)WCTest.InternationalTopupValidation("IN/ID/917025834403");
                        string data = string.Format("?CountryCode={0}&OperatorCode={1}&MobileNumber={2}", "IN", "ID", "917025834403");
                        response = await httpClient.GetAsync("api/Services/InternationalTopupValidation" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<InternationalTopupValidation>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }
                        //TestWebServiceClient.ServiceReference2.InternationalTopupCheck obj1 = 
                        //(TestWebServiceClient.ServiceReference2.InternationalTopupCheck)WCTest.InternationalTopupCheck("IT/IN/VF/919645834359/2.000");
                        data = string.Format("?OperatorName={0}&CountryCode={1}&OperatorCode={2}&MobileNumber={3}&Amount={4}",
                            "IT", "IN", "VF", "919645834359", "2.000");
                        response = await httpClient.GetAsync("api/Services/InternationalTopupCheck" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<InternationalTopupCheck>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }

                        //TestWebServiceClient.ServiceReference2.InternationalTopupTransfer obj2 = 
                        //(TestWebServiceClient.ServiceReference2.InternationalTopupTransfer)WCTest.InternationalTopupTransfer("IT/1.250/919745632892/IN/VF/CASH");
                        data = string.Format("?OperatorName={0}&amount={1}&MobileNumber={2}&CountryCode={3}&OperatorCode={4}&PaymentType={5}",
                            "IT", "1.250", "919745632892", "IN", "VF", "CASH");
                        response = await httpClient.GetAsync("api/Services/InternationalTopupTransfer" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<InternationalTopupTransfer>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }


                        //TestWebServiceClient.ServiceReference2.TopupValidation obj3 = (TestWebServiceClient.ServiceReference2.TopupValidation)WCTest.TopupValidation("VV/1.000/55155445");
                        data = string.Format("?OperatorName={0}&Amount={1}&MobileNumber={2}", "VV", "1.000", "55155445");
                        response = await httpClient.GetAsync("api/Services/TopupValidation" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<TopupValidation>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }

                        //TestWebServiceClient.ServiceReference2.VoucherValidation obj4 = (TestWebServiceClient.ServiceReference2.VoucherValidation)WCTest.VoucherValidation("VV/1.000/1");
                        data = string.Format("?OperatorName={0}&Denomination={1}&CardCount={2}", "VV", "1.000", "1");
                        response = await httpClient.GetAsync("api/Services/VoucherValidation" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<VoucherValidation>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }



                        //TestWebServiceClient.ServiceReference2.BillPaymentValidation obj5 = (TestWebServiceClient.ServiceReference2.BillPaymentValidation)WCTest.BillPaymentValidation("IT/1.250/55155445");
                        data = string.Format("?OperatorName={0}&AmtSelected={1}&MobileNumber={2}", "IT", "1.250", "55155445");
                        response = await httpClient.GetAsync("api/Services/BillPaymentValidation" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<BillPaymentValidation>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }

                        //TestWebServiceClient.ServiceReference2.VoucherTransfer obj6 = (TestWebServiceClient.ServiceReference2.VoucherTransfer)WCTest.VoucherTransfer("VV/1.000/12345678901234");
                        data = string.Format("?OperatorName={0}&Denomination={1}&MobileNo={2}", "VV", "1.000", "12345678901234");
                        response = await httpClient.GetAsync("api/Services/VoucherTransfer" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<VoucherTransfer>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }

                        ////TestWebServiceClient.ServiceReference2.VoucherTransfer obj6_1 = (TestWebServiceClient.ServiceReference2.VoucherTransfer)WCTest.VoucherTransfer("VV/5.000/12345678901235");
                        data = string.Format("?OperatorName={0}&Denomination={1}&MobileNo={2}", "VV", "5.000", "12345678901235");
                        response = await httpClient.GetAsync("api/Services/VoucherTransfer" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<VoucherTransfer>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }

                        ////TestWebServiceClient.ServiceReference2.VoucherTransfer obj6_2 = (TestWebServiceClient.ServiceReference2.VoucherTransfer)WCTest.VoucherTransfer("VV/5.000/12345678901236");
                        data = string.Format("?OperatorName={0}&Denomination={1}&MobileNo={2}", "VV", "5.000", "12345678901236");
                        response = await httpClient.GetAsync("api/Services/VoucherTransfer" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<VoucherTransfer>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }

                        //TestWebServiceClient.ServiceReference2.TopupTransfer obj7 = (TestWebServiceClient.ServiceReference2.TopupTransfer)WCTest.TopupTransfer("VV/1.000/55155445/CASH");
                        data = string.Format("?OperatorName={0}&AmtSelected={1}&MobileNumber={2}&PaymentType={3}",
                            "VV", "1.000", "55155445", "CASH");
                        response = await httpClient.GetAsync("api/Services/TopupTransfer" + data);

                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<TopupTransfer>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }

                        //TestWebServiceClient.ServiceReference2.BillPaymentTransfer obj8 = (TestWebServiceClient.ServiceReference2.BillPaymentTransfer)WCTest.BillPaymentTransfer("VV/1.000/55155445/CASH");
                        data = string.Format("?OperatorName={0}&AmtSelected={1}&MobileNumber={2}&PaymentType={3}", "VV", "1.000", "55155445", "CASH");
                        response = await httpClient.GetAsync("api/Services/BillPaymentTransfer" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<BillPaymentTransfer>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }


                        ////ADD_SERVICE_TOPUP,BS,56.000,55155445,196;1234567890,CASH,1000$
                        //TestWebServiceClient.ServiceReference2.ServiceTopup obj10 = (TestWebServiceClient.ServiceReference2.ServiceTopup)WCTest.AddServiceTopup("BS", "56.000", "55155445", "196", "1234567890", "CASH");
                        data = string.Format("?operatorName={0}&amount={1}&mobileNumber={2}&productID={3}&userSubSmartCardNo={4}&PaymentType={5}",
                        "BS", "56.000", "55155445", "196", "1234567890", "CASH");
                        response = await httpClient.GetAsync("api/Services/AddServiceTopup" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<ServiceTopup>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }

                        ////            HISTORY,VV,10.000,5,74563874639872;55553874639872,101$
                        ////HISTORY,VV,,3,74563874639872;55553874639872,101$
                        ////         HISTORY,,,5,74563874639872;55553874639872,101$

                        //TestWebServiceClient.ServiceReference2.HistoryData obj11 = (TestWebServiceClient.ServiceReference2.HistoryData)WCTest.History("VV/10.000/5/12345678901234;12345678901235");
                        data = string.Format("?cardType={0}&denomination={1}&count={2}&accountNumbers={3}", "VV", "10.000", "5", "12345678901234;12345678901235");
                        response = await httpClient.GetAsync("api/Services/History" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<HistoryData>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }
                        #region inter key

                        //TestWebServiceClient.ServiceReference2.InterkeySubscribeAuthorize obj12 = (TestWebServiceClient.ServiceReference2.InterkeySubscribeAuthorize)WCTest.InterkeySubscribeAuthorize("7003726156");
                        data = string.Format("?sbID={0}", "7003726156");
                        response = await httpClient.GetAsync("api/Services/InterkeySubscribeAuthorize" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<InterkeySubscribeAuthorize>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }

                        ////IK_SUBSCRIBER_RELEASE,SB_ID
                        ////Eg: IK_SUBSCRIBER_RELEASE,7003726156

                        //TestWebServiceClient.ServiceReference2.InterkeySubscriberRelease obj13 = (TestWebServiceClient.ServiceReference2.InterkeySubscriberRelease)WCTest.InterkeySubscriberRelease("7003726156");
                        data = string.Format("?sbID={0}", "7003726156");
                        response = await httpClient.GetAsync("api/Services/InterkeySubscriberRelease" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<InterkeySubscriberRelease>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }



                        ////IK_SUBSCRIBER_UPDATE,7003726156,775$
                        //TestWebServiceClient.ServiceReference2.InterkeySubscriberUpdate obj14 = (TestWebServiceClient.ServiceReference2.InterkeySubscriberUpdate)WCTest.InterkeySubscriberUpdate("7003726156/775");
                        data = string.Format("?sbID={0}&sbCredit={1}", "7003726156", "775$");

                        response = await httpClient.GetAsync("api/Services/InterkeySubscriberUpdate" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<InterkeySubscriberUpdate>();
                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }

                        #endregion

                        //MessageBox.Show("All test successful");


                    }
                    #endregion call services test

                }
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.Message);

            }

        }
        #endregion

    }
}
﻿using System;
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
using System.Data.Entity.Validation;
using System.Configuration;

namespace OnlineRecharge.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        public const string BASEADDRESS = "https://grcweb.grckiosk.com:8443/";
        public const string USERNAME = "101";
        public const string PASSWORD = "000";

        #region Fields
        EFDbContext context = new EFDbContext();
        #endregion

        #region Action Methods

        #region Topup Action Methods
        [HttpPost]
        public async Task<JsonResult> TranferNationalTopup()
        {
            var resp = await this.TopupTransfer();
            return this.Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ValidateTopup()
        {
            var resp = await this.TopupVaalidation();
            return this.Json(resp, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BillPayment Action Methods
        [HttpPost]
        public async Task<JsonResult> NationalBillPayment()
        {
            var resp = await this.BillPayment();
            return this.Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> PaymentValidation()
        {
            var resp = await this.BillPaymentValidation();
            return this.Json(resp, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Voucher Action Methods 
        [HttpPost]
        public async Task<JsonResult> GetAllVouchers()
        {
            var resp = await this.GetAllOperatorVouchers();
            return this.Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> NationalVoucherTranfer()
        {
            var resp = await this.VoucherTranfer();
            return this.Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ValidateVoucher()
        {
            var resp = await this.VoucherValidation();
            return this.Json(resp, JsonRequestBehavior.AllowGet);
        }
        #endregion
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

       


        


        public JsonResult GetInternationalServiceProviders(string Code)
        {

            List<InternationalServiceProvidersModel> model = new List<InternationalServiceProvidersModel>();
            try
            {

                Code = Code.ToUpper();
                var countryId = context.Countries.Single(x => x.Code == Code).ID;
                model = context.internationalServiceProviders.Where(x => x.CountryID == countryId).Select(
                    x => new InternationalServiceProvidersModel
                    {
                        Code = x.Code,
                        Name = x.Name
                    }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

       

        [HttpPost]
        public async Task<JsonResult> GetAllDataCardVouchers()
        {
            var resp = await this.GetAllDataCardOperatorVouchers();
            return this.Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<List<VoucherDetailsModel>> GetAllDataCardOperatorVouchers()
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

                            List<Service> operators = result.Where(x => x.OperatorType == "VI" || x.OperatorType == "MY").ToList();

                            foreach (var item in operators)
                            {
                                foreach (var card in item.DenomCollection)
                                {
                                    VoucherDetailsModel model = new VoucherDetailsModel();
                                    if (item.OperatorType == "VI")
                                    {
                                        model.OperatorCode = "VV";
                                    }
                                    else if (item.OperatorType == "MY")
                                    {
                                        model.OperatorCode = "XP";
                                    }
                                    model.Amount = card.Denom;
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

        public JsonResult GetShoppingCardProviders()
        {
            List<ShoppingCardTypes> model = new List<ShoppingCardTypes>();
            try
            {

                //NP,IT,RX

                model = context.ShoppingCardTypes.Where(x => x.Code == "AZ" || x.Code == "CU" || x.Code == "FB" || x.Code == "GP"
                          || x.Code == "GK" || x.Code == "IK" || x.Code == "IG" || x.Code == "IT"

                          || x.Code == "IN" || x.Code == "IP" || x.Code == "OC" || x.Code == "PS" || x.Code == "PW"

                          || x.Code == "NP"

                          || x.Code == "PK" || x.Code == "TI" || x.Code == "NF" || x.Code == "MY" || x.Code == "RZ" || x.Code == "RX" || x.Code == "SM").ToList();

                model.Insert(0, new ShoppingCardTypes { Code = "-1", Name = "select" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dial(string Dialer)
        {
            if (!Dialer.EndsWith("#"))
            {
                ViewData["Dialer"] = Dialer + "#";
            }
            else
                ViewData["Dialer"] = Dialer;
            return View();
        }

        /// <summary>
        /// Call before submit the payment.
        /// </summary>
        /// <param name="OperatorName"></param>
        /// <param name="CountryCode"></param>
        /// <param name="OperatorCode"></param>
        /// <param name="MobileNumber"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        /// <summary>
        /// Call before submit the payment.
        /// </summary>
        /// <param name="OperatorName"></param>
        /// <param name="CountryCode"></param>
        /// <param name="OperatorCode"></param>
        /// <param name="MobileNumber"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public async Task<JsonResult> InternationalTopupCheck(string OperatorName, string CountryCode, string OperatorCode, string MobileNumber, string Amount)
        {
            InternationalTopupCheck model = new InternationalTopupCheck();
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


                        //TestWebServiceClient.ServiceReference2.InternationalTopupCheck obj1 = 
                        //(TestWebServiceClient.ServiceReference2.InternationalTopupCheck)WCTest.InternationalTopupCheck("IT/IN/VF/919645834359/2.000");
                        string data = string.Format("?OperatorName={0}&CountryCode={1}&OperatorCode={2}&MobileNumber={3}&Amount={4}",
                               OperatorName, CountryCode, OperatorCode, MobileNumber, Amount);
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/InternationalTopupCheck" + data);

                        //first call InternationalTopupCheck
                        //Internation topup transfer


                        if (response.IsSuccessStatusCode)
                        {
                            model = await response.Content.ReadAsAsync<InternationalTopupCheck>();
                        }

                    }
                    #endregion call services test

                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> InternationalTopupValidation(string CountryCode, string OperatorCode, string MobileNumber)
        {

            // Start OWIN host 
            InternationalTopupValidation model = new Models.Helpers.InternationalTopupValidation();
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

                        HttpResponseMessage response = new HttpResponseMessage();

                        string data = string.Format("?CountryCode={0}&OperatorCode={1}&MobileNumber={2}", "IN", "BL", "919959610594");
                        response = await httpClient.GetAsync("api/Services/InternationalTopupValidation" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            model = await response.Content.ReadAsAsync<InternationalTopupValidation>();
                        }

                        //string ApiUrl = string.Format("api/Services/InternationalTopupValidation?CountryCode={0}&OperatorCode={1}&MobileNumber={2}", CountryCode.ToUpper(), OperatorCode, MobileNumber);
                        //response = await httpClient.GetAsync(ApiUrl);
                        //if (response.IsSuccessStatusCode)
                        //{
                        //    model = await response.Content.ReadAsAsync<InternationalTopupValidation>();
                        //}

                    }

                }
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.Message);

            }

            return Json(model, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        ///  Save recharge details to db.
        /// </summary>
        /// <param name="OperatorName"></param>
        /// <param name="CountryCode"></param>
        /// <param name="OperatorCode"></param>
        /// <param name="MobileNumber"></param>
        /// <param name="Amount"></param>
        /// <param name="PaymentType"></param>
        /// <returns></returns>
        public async Task<JsonResult> InternationalTopupTransfer()
        {
            TransferResponseDetailsModel model = new TransferResponseDetailsModel();
            try
            {

                using (var client = new HttpClient())
                {

                    string rechargeType = Request.Form["rechargeType"];
                    string operatorName = Request.Form["operatorCode"];
                    string mobileNumber = Request.Form["mobileNumber"];
                    string amount = Request.Form["amount"];
                    string paymentID = Request.Form["paymentID"];
                    string status = Request.Form["result"];
                    string trackID = Request.Form["trackID"];
                    string tranID = Request.Form["tranID"];
                    string reference = Request.Form["ref"];
                    string countryCode = Request.Form["countryCode"];
                    string operatorCode = Request.Form["operatorCode"];


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


                        //TestWebServiceClient.ServiceReference2.InternationalTopupCheck obj1 = 
                        //(TestWebServiceClient.ServiceReference2.InternationalTopupCheck)WCTest.InternationalTopupCheck("IT/IN/VF/919645834359/2.000");
                        string data = string.Format("?OperatorName={0}&amount={1}&MobileNumber={2}&CountryCode={3}&OperatorCode={4}&PaymentType={5}",
                            "IT", amount, mobileNumber, countryCode, operatorCode, "CASH");
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/InternationalTopupTransfer" + data);

                        //first call InternationalTopupCheck
                        //Internation topup transfer


                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<TopupTransfer>();

                            model.PaymentID = result.PaymentID;
                            model.PaymentRef = result.PaymentRef;
                            model.Response = result.Response;
                            model.ResponseDescription = result.ResponseDescription;
                            model.Amount = Convert.ToDecimal(amount);
                            int id = UpdateInternationalRechargeDetailsToDB(mobileNumber, rechargeType, operatorName, paymentID, status, trackID, tranID, reference, model);

                        }
                        else
                        {
                            //MessageBox.Show(response.ReasonPhrase);
                        }

                    }


                    #endregion call services test

                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetAllShoppingCardVouchers()
        {
            var resp = await this.GetAllShoppingCardOperatorVouchers();
            return this.Json(resp, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Recharge API Service Methods
        #region Topup API Service Methods
        [HttpPost]
        public async Task<TransferResponseDetailsModel> TopupTransfer()
        {
            try
            {
                TransferResponseDetailsModel model = new TransferResponseDetailsModel();
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
                            model.Amount = Convert.ToDecimal(amount);
                            model.Date = result.Date;
                            model.OperatorName = GetOperatorNameByOperatorCode(operatorName);
                            model.PaymentID = result.PaymentID;
                            model.PaymentRef = result.PaymentRef;
                            model.Response = result.Response;
                            model.ResponseDescription = result.ResponseDescription;
                            int id = UpdateRechargeDetailsToDB(mobileNumber, rechargeType, operatorName, paymentID, status, trackID, tranID, reference, model);
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

        [HttpPost]
        public async Task<TopupValidation> TopupVaalidation()
        {
            try
            {
                TopupValidation model = new TopupValidation(); 
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

                       string data = string.Format("?OperatorName={0}&Amount={1}&MobileNumber={2}", operatorName, amount, mobileNumber);
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/TopupValidation" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            model = await response.Content.ReadAsAsync<TopupValidation>();
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

        #region BillPayment API Service Methods
        [HttpPost]
        public async Task<TransferResponseDetailsModel> BillPayment()
        {
            try
            {
                TransferResponseDetailsModel model = new TransferResponseDetailsModel();
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
                        string data = string.Format("?OperatorName={0}&AmtSelected={1}&MobileNumber={2}&PaymentType={3}", operatorName, amount, mobileNumber, "CASH");
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/BillPaymentTransfer" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<BillPaymentTransfer>();
                            model.Amount = Convert.ToDecimal(amount);
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
                            int id = UpdateRechargeDetailsToDB(mobileNumber, rechargeType, operatorName, paymentID, status, trackID, tranID, reference, model);
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

        [HttpPost]
        public async Task<BillPaymentValidation> BillPaymentValidation()
        {
            try
            {
                BillPaymentValidation model = new BillPaymentValidation();
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

                        string data = string.Format("?OperatorName={0}&AmtSelected={1}&MobileNumber={2}", operatorName, amount, mobileNumber);
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/BillPaymentValidation" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            model= await response.Content.ReadAsAsync<BillPaymentValidation>();
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

        #region Voucher Transfer API Service Methods
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
        public async Task<TransferResponseDetailsModel> VoucherTranfer()
        {
            try
            {
                WriteLog("VoucherTranfer", "VoucherTranfer");
                TransferResponseDetailsModel model = new TransferResponseDetailsModel();
                string apiUrl = string.Empty;
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ApiUrl")) // Key exists
                {
                    apiUrl = ConfigurationManager.AppSettings["ApiUrl"].ToString();
                    WriteLog("VoucherTranfer apiUrl", apiUrl);
                }
                else//Error log
                {
                }
                string rechargeType = Request.Form["rechargeType"];
                string serviceType = Request.Form["serviceType"];
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

                        string data = string.Format("?OperatorName={0}&Denomination={1}&MobileNo={2}", operatorName, amount, mobileNumber);
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/VoucherTransfer" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsAsync<VoucherTransfer>();
                            if (result.Response.ToUpper() == "S")
                            {
                                model.Amount = Convert.ToDecimal(amount);
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
                                model.Response = result.Response;
                                model.RechargeCode = result.RechargeCode;
                                model.Denomination = result.Denomination;
                                model.Password = result.Password;
                                model.SerialNo = result.SerialNo;
                                if (ServiceType.DataCards.ToString() == serviceType)
                                {
                                    int datacardresponse = UpdateDataCardRechargeDetailsToDB(mobileNumber, rechargeType, operatorName, paymentID, status, trackID, tranID, reference, model);
                                    if (ConfigurationManager.AppSettings.AllKeys.Contains("ApiUrl")) // Key exists
                                    {
                                        apiUrl += "api/MessageApi/DataCardsSendMessage?MobileNumber=" + mobileNumber + "&OperatorCode=" + operatorName + "&RechargeCode=" + result.RechargeCode + "&Amount=" + amount + "&TranID=" + tranID;
                                        WriteLog("DataCards apiUrl", apiUrl);
                                        using (var clients = new WebClient())
                                        {
                                            var resFrmApi = JsonConvert.DeserializeObject<string>(clients.DownloadString(apiUrl));
                                        }
                                    }
                                }
                                else if (ServiceType.ShoppingCards.ToString() == serviceType)
                                {
                                    model.OperatorName = GetShoppingCardsOperatorName(operatorName);
                                    int id = UpdateShoppingCardDetailsToDB(mobileNumber, rechargeType, operatorName, paymentID, status, trackID, tranID, reference, model);
                                    if (ConfigurationManager.AppSettings.AllKeys.Contains("ApiUrl")) // Key exists
                                    {
                                        apiUrl += "api/MessageApi/ShoppingCardsSendMessage?MobileNumber=" + mobileNumber + "&OperatorCode=" + operatorName + "&RechargeCode=" + result.RechargeCode + "&Amount=" + amount + "&TranID=" + tranID;
                                        WriteLog("DataCards apiUrl", apiUrl);
                                        using (var clients = new WebClient())
                                        {
                                            var resFrmApi = JsonConvert.DeserializeObject<string>(clients.DownloadString(apiUrl));
                                        }
                                    }
                                }
                                else
                                { int id = UpdateRechargeDetailsToDB(mobileNumber, rechargeType, operatorName, paymentID, status, trackID, tranID, reference, model); }
                                if (ConfigurationManager.AppSettings.AllKeys.Contains("ApiUrl")) // Key exists
                                {
                                    apiUrl += "api/MessageApi/SendMessage?MobileNumber=" + mobileNumber + "&OperatorCode=" + operatorName + "&RechargeCode=" + result.RechargeCode + "&Amount=" + amount + "&TranID=" + tranID;
                                    WriteLog("VoucherTranfer apiUrl", apiUrl);
                                    using (var clients = new WebClient())
                                    {
                                        var resFrmApi = JsonConvert.DeserializeObject<string>(clients.DownloadString(apiUrl));
                                    }
                                }
                            }
                            else//Failed
                            {
                                if (ServiceType.DataCards.ToString() == serviceType)
                                {
                                    apiUrl += "api/MessageApi/DataCardsFailedMessage?MobileNumber=" + mobileNumber + "&OperatorCode=" + operatorName + "&TrackID=" + trackID; ;
                                    WriteLog("DataCards apiUrl failed ", apiUrl);
                                    using (var clients = new WebClient())
                                    {
                                        var resFrmApi = JsonConvert.DeserializeObject<string>(clients.DownloadString(apiUrl));
                                    }
                                }
                                else if (ServiceType.ShoppingCards.ToString() == serviceType)
                                {
                                    apiUrl += "api/MessageApi/ShoppingCardsFailedMessage?MobileNumber=" + mobileNumber + "&OperatorCode=" + operatorName + "&TrackID=" + trackID; ;
                                    WriteLog("ShoppingCards apiUrl failed ", apiUrl);
                                    using (var clients = new WebClient())
                                    {
                                        var resFrmApi = JsonConvert.DeserializeObject<string>(clients.DownloadString(apiUrl));
                                    }
                                }
                                else//Mobile Vouchers
                                {
                                    apiUrl += "api/MessageApi/FailedMessage?MobileNumber=" + mobileNumber + "&OperatorCode=" + operatorName + "&TrackID=" + trackID; ;
                                    WriteLog("VoucherTranfer apiUrl failed ", apiUrl);
                                    using (var clients = new WebClient())
                                    {
                                        var resFrmApi = JsonConvert.DeserializeObject<string>(clients.DownloadString(apiUrl));
                                    }
                                }
                            }
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                WriteLog("VoucherTranfer Exception ", ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public async Task<VoucherValidation> VoucherValidation()
        {
            try
            {
                WriteLog("VoucherTranfer", "VoucherTranfer");
                VoucherValidation model = new VoucherValidation();
                string apiUrl = string.Empty;
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ApiUrl")) // Key exists
                {
                    apiUrl = ConfigurationManager.AppSettings["ApiUrl"].ToString();
                    WriteLog("VoucherTranfer apiUrl", apiUrl);
                }
                else//Error log
                {
                }
                string rechargeType = Request.Form["rechargeType"];
                string serviceType = Request.Form["serviceType"];
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

                       string data = string.Format("?OperatorName={0}&Denomination={1}&CardCount={2}",operatorName, amount, "1");
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/VoucherValidation" + data);
                        if (response.IsSuccessStatusCode)
                        {
                            model = await response.Content.ReadAsAsync<VoucherValidation>();
                        }
                      
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                WriteLog("VoucherTranfer Exception ", ex.Message);
                throw ex;
            }
        }
        #endregion
        [HttpPost]
        public async Task<List<VoucherResponseModel>> GetAllShoppingCardOperatorVouchers()
        {
            try
            {
                List<VoucherResponseModel> vouchers = new List<VoucherResponseModel>();
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

                            string[] OperatorTypes = new string[] { "AZ", "CU", "FB", "GB", "GK", "IK", "IG", "IN", "IP", "TI", "NF", "OC", "PS", "PW", "PK", "RZ", "RX", "SM" };


                            List<Service> operators = result.Where(x => x.OperatorType == "AZ" || x.OperatorType == "CU" || x.OperatorType == "FB" || x.OperatorType == "GP"
                            || x.OperatorType == "GK" || x.OperatorType == "IK" || x.OperatorType == "IG"

                            || x.OperatorType == "IN" || x.OperatorType == "IP" || x.OperatorType == "OC" || x.OperatorType == "PS" || x.OperatorType == "PW"
                           || x.OperatorType == "NP"

                             || x.OperatorType == "PK" || x.OperatorType == "TI" || x.OperatorType == "NF" || x.OperatorType == "MY" || x.OperatorType == "RZ" || x.OperatorType == "RX" || x.OperatorType == "SM").ToList();

                            //x.OperatorType == "IT"=null
                            foreach (var item in operators)
                            {
                                foreach (var card in item.DenomCollection)
                                {
                                    VoucherResponseModel model = new VoucherResponseModel();
                                    if (item.OperatorType == "AZ")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/amazon-small.png";
                                    }
                                    else if (item.OperatorType == "CU")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/cashu-small.png";
                                    }
                                    else if (item.OperatorType == "FB")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/facebook-small.png";
                                    }
                                    else if (item.OperatorType == "GP")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/google-play-small.png";
                                    }
                                    else if (item.OperatorType == "GK")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/google-play-small.png";
                                    }
                                    else if (item.OperatorType == "IK")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/inter-key.png";

                                    }
                                    else if (item.OperatorType == "IG")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/isbre-small.png";
                                    }
                                    else if (item.OperatorType == "IN")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards//isbre-small.png";
                                    }
                                    else if (item.OperatorType == "IP")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/isbre-small.png";
                                    }
                                    else if (item.OperatorType == "OC")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/onecard-small.png";
                                    }
                                    else if (item.OperatorType == "PS")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/playstation-small.png";
                                    }
                                    else if (item.OperatorType == "PW")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/playstation-small.png";
                                    }
                                    else if (item.OperatorType == "PK")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/playstation-small.png";
                                    }
                                    else if (item.OperatorType == "TI")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/iTunes-small.png";
                                    }
                                    else if (item.OperatorType == "IT")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/iTunes-small.png";
                                    }
                                    else if (item.OperatorType == "NF")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/netflix-small.png";
                                    }
                                    else if (item.OperatorType == "RZ")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/Rapplez-small.png";
                                    }
                                    else if (item.OperatorType == "RX")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/Rapplez.png";
                                    }
                                    else if (item.OperatorType == "SM")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/steam-small.png";
                                    }
                                    else if (item.OperatorType == "NF")
                                    {
                                        model.OperatorCode = item.OperatorType;
                                        model.ImageURL = "/Content/img/ShoppingCards/netflix-small.png";
                                    }
                                    model.Amount = card.Denom;
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

        #endregion

        #region DataAccess Methods
        public JsonResult GetServiceProviders()
        {
            List<OperatorResponseModel> model = new List<OperatorResponseModel>();
            try
            {
                model = context.ServiceProiders.Select(v=> new OperatorResponseModel()
                {
                    Code=v.Code,
                    Name=v.Name
                }).ToList();
                model.Insert(0,new OperatorResponseModel() { Code="-1",Name="select" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

        public string GetShoppingCardsOperatorName(string operatorCode)
        {
            string operatorName = string.Empty;
            try
            {
                operatorName = context.ShoppingCardTypes.Where(x => x.Code == operatorCode).Select(v => v.Name).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return operatorName;
        }

        /// <summary>
        /// Bind Card Type drop down list.
        /// </summary>
        /// <returns></returns>

        public JsonResult GetShoppingCardServiceProviders()
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

        public int UpdateRechargeDetailsToDB(string mobileNumber,string rechargeType,string operatorCode,string paymentID,string status,string trackID,string tranID,string reference, TransferResponseDetailsModel result)
        {
            var model = context.NationalRecharges.Create();
            try
            {
                
                model.MobileNumber = mobileNumber;
                model.amount = result.Amount;
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
                apiResponseDetail.Date = DateTime.Now;// result.Date;
                apiResponseDetail.Denomination = result.Denomination;
                apiResponseDetail.Password = result.Password;
                apiResponseDetail.RechargeCode = result.RechargeCode;
                apiResponseDetail.SerialNo = result.SerialNo;
                    context.NationalRechargeAPIResponseDetails.Add(apiResponseDetail);
                context.SaveChanges();
                
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return model.ID;
        }

        public int UpdateShoppingCardDetailsToDB(string mobileNumber, string rechargeType, string operatorCode, string paymentID, string status, string trackID, string tranID, string reference, TransferResponseDetailsModel result)
        {
            var model = context.ShoppingCards.Create();
            try
            {


                model.amount = result.Amount;
                model.ServiceProvider = context.ShoppingCardTypes.Where(x => x.Code == operatorCode).FirstOrDefault();
                //Set shopping Type card Id.
                model.ShoppingCardTypesID = model.ServiceProvider.ID;
                model.IsActive = true;
                model.IsDeleted = false;
                model.CreatedBy = 1;
                model.CreatedDate = DateTime.Now;
                model.CustomerID = 1;
                context.ShoppingCards.Add(model);
                context.SaveChanges();

                var paymentDetail = context.ShoppingCardPaymentDetails.Create();
                paymentDetail.PaymentID = paymentID;
                paymentDetail.Result = status;
                paymentDetail.TrackID = trackID;
                paymentDetail.TransID = tranID;
                paymentDetail.Ref = reference;
                paymentDetail.ShoppingCards = model;
                context.ShoppingCardPaymentDetails.Add(paymentDetail);

                var apiResponseDetail = context.ShoppingCardAPIResponseDetails.Create();
                apiResponseDetail.NationalRecharge = model;
                apiResponseDetail.Response = result.Response;
                apiResponseDetail.Denomination = result.Denomination;
                apiResponseDetail.Password = result.Password;
                apiResponseDetail.RechargeCode = result.RechargeCode;
                apiResponseDetail.SerialNo = result.SerialNo;
                context.ShoppingCardAPIResponseDetails.Add(apiResponseDetail);
                context.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return model.ID;
        }


        public int UpdateDataCardRechargeDetailsToDB(string mobileNumber, string rechargeType, string operatorCode, string paymentID, string status, string trackID, string tranID, string reference, TransferResponseDetailsModel result)
        {
            var model = context.DataCardRecharge.Create();
            try
            {

                model.MobileNumber = mobileNumber;
                model.amount = result.Amount;
                model.ServiceProvider = context.ServiceProiders.Where(x => x.Code == operatorCode).FirstOrDefault();
                model.IsActive = true;
                model.IsDeleted = false;
                model.CreatedBy = 1;
                model.CreatedDate = DateTime.Now;
                model.CustomerID = 1;
                context.DataCardRecharge.Add(model);
                context.SaveChanges();

                var paymentDetail = context.DataCardRechargePaymentDetail.Create();
                paymentDetail.PaymentID = paymentID;
                paymentDetail.Result = status;
                paymentDetail.TrackID = trackID;
                paymentDetail.TransID = tranID;
                paymentDetail.Ref = reference;
                paymentDetail.DataCardRecharge = model;
                context.DataCardRechargePaymentDetail.Add(paymentDetail);

                var apiResponseDetail = context.DataCardRechargeApiDetail.Create();
                apiResponseDetail.DataCardRecharge = model;
                apiResponseDetail.PaymentID = result.PaymentID;
                apiResponseDetail.PaymentRef = result.PaymentRef;
                apiResponseDetail.Response = result.Response;
                apiResponseDetail.ResponseDescription = result.ResponseDescription;
                apiResponseDetail.Date = DateTime.Now;// result.Date;
                apiResponseDetail.Denomination = result.Denomination;
                apiResponseDetail.Password = result.Password;
                apiResponseDetail.RechargeCode = result.RechargeCode;
                apiResponseDetail.SerialNo = result.SerialNo;
                context.DataCardRechargeApiDetail.Add(apiResponseDetail);
                context.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return model.ID;
        }

        public int UpdateInternationalRechargeDetailsToDB(string mobileNumber, string rechargeType, string operatorCode, string paymentID, string status, string trackID, string tranID, string reference, TransferResponseDetailsModel result)
        {
            var model = context.InternationalRecharges.Create();
            try
            {

                model.MobileNumber = mobileNumber;
                model.amount = result.Amount;
                model.ServiceProvider = context.internationalServiceProviders.Where(x => x.Code == operatorCode).FirstOrDefault();
                //model.RechargeType = context.NationalRechargeTypes.Where(x => x.Name.Equals(rechargeType, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                model.IsActive = true;
                model.IsDeleted = false;
                model.CreatedBy = 1;
                model.CreatedDate = DateTime.Now;
                model.CustomerID = 1;
                context.InternationalRecharges.Add(model);
                context.SaveChanges();

                var paymentDetail = context.InternationalRechargePaymentDetails.Create();
                paymentDetail.PaymentID = paymentID;
                paymentDetail.Result = status;
                paymentDetail.TrackID = trackID;
                paymentDetail.TransID = tranID;
                paymentDetail.Ref = reference;
                paymentDetail.InternationalRecharge = model;
                context.InternationalRechargePaymentDetails.Add(paymentDetail);

                var apiResponseDetail = context.InternationalRechargeAPIResponseDetails.Create();
                apiResponseDetail.InternationalRecharge = model;
                apiResponseDetail.PaymentID = result.PaymentID;
                apiResponseDetail.PaymentRef = result.PaymentRef;
                apiResponseDetail.Response = result.Response;
                apiResponseDetail.ResponseDescription = result.ResponseDescription;
                apiResponseDetail.Date = result.Date == DateTime.MinValue ? DateTime.Now : result.Date;
                context.InternationalRechargeAPIResponseDetails.Add(apiResponseDetail);
                context.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return model.ID;
        }
        #endregion

        #region Kamal
        //Internatinal Recharnges Update

        public JsonResult GetServiceDataCardProviders()
        {
            List<InternationalServiceProvidersModel> model = new List<InternationalServiceProvidersModel>();
            try
            {
                model = context.ServiceProiders.Select(x => new InternationalServiceProvidersModel
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToList();

                model.Insert(0, new InternationalServiceProvidersModel() { Code = "-1", Name = "select" });


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return this.Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Wallet API Methods
        [HttpPost]
        public JsonResult GetRechargeParameter(int id)
        {
            RechargeParamModel result = new RechargeParamModel();
            try
            {
                string url = string.Empty;
                using (var client = new HttpClient())
                {
                    string param = "id=" + id;
                    url = "https://api.2easy2pay.com/Wallet/api/parameter/get?";
                    HttpResponseMessage response = client.GetAsync(url + param, HttpCompletionOption.ResponseContentRead).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsAsync<RechargeParamModel>().Result;
                    }

                    url = string.Empty;
                    url = "https://api.2easy2pay.com/Wallet/api/parameter/delete?";
                    response = client.DeleteAsync(url + param).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        bool isDeleted = response.Content.ReadAsAsync<bool>().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return this.Json(result, JsonRequestBehavior.AllowGet);
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
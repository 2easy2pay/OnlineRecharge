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

namespace OnlineRecharge.Controllers
{
    public class HomeController : Controller
    {
        public const string BASEADDRESS = "https://grcweb.grckiosk.com:8443/";
        public const string USERNAME = "101";
        public const string PASSWORD = "000";
        public ActionResult Index()
        {
            try
            {
                TestWebApiAllServices();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View();
        }

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
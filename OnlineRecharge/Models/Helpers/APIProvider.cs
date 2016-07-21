using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OnlineRecharge.Models.Helpers
{
    public class APIProvider
    {
        public const string BASEADDRESS = "https://grcweb.grckiosk.com:8443/";
        public const string USERNAME = "101";
        public const string PASSWORD = "000";
        public async Task<TopupTransfer> TopupTransfer()
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

                    var resp =  client.PostAsync(url, request.Content);
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
                             "VV", "1.000", "55155445", "CASH");
                        HttpResponseMessage response = await httpClient.GetAsync("api/Services/TopupTransfer" + data);

                        //if (response.IsSuccessStatusCode)
                        //{

                        //}
                        //else
                        //{
                        //    //MessageBox.Show(response.ReasonPhrase);
                        //}
                        return (await response.Content.ReadAsAsync<TopupTransfer>());

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
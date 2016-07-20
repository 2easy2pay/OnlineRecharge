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
        public async Task<TopupTransfer> TopupTransfer(string USERNAME, string PASSWORD, string BASEADDRESS)
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
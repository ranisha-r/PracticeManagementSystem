using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Core
{
   public class ApiCall
    
    {
        public async void HttpPostToApi(string json, string apiUrl)
        {

            try
            {
                string result = "";
                using (HttpClient client = new HttpClient())
                {

                    var response = await client.PostAsync(apiUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                    using (HttpContent content = response.Content)
                    {
                         result = content.ReadAsStringAsync().Result;
                 
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        public async Task<string> HttpGetfromApi(string apiURL,string token)
        {
            try
            {


                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await client.GetAsync(apiURL);
                    string res = "";
                        res= await response.Content.ReadAsStringAsync();
                        return res;

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }




        }      

    }
}

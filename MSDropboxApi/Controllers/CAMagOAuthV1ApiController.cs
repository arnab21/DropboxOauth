using MSDropboxApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Collections.Concurrent;

namespace MSDropboxApi.Controllers
{


    /// <summary>
    /// This Controller receives a OAuth token from a MAG gateway and manages further interactions with Dropbox internally
    /// </summary>
    [RoutePrefix("camagoauthapi/v1")]
    public class CAMagOAuthV1ApiController : ApiController
    {
        HttpClient client = new HttpClient();
        const string USER_HEADER = "X-MS-USERID";
        const string DEFAULT_USERID = "default";
        string sessionID = string.Empty;
        string bearerToken = "HgUYyC8K3HAAAAAAAAAAH28My8Um95RHbe9s3J2-CSSVvwUfBB-TPeoRsxzQLsA";
        static ConcurrentDictionary<string, string> userTokens = new ConcurrentDictionary<string, string>();

        public CAMagOAuthV1ApiController()
        {
            userTokens.TryAdd(DEFAULT_USERID, bearerToken);
            

            client.BaseAddress = new Uri("https://api.dropboxapi.com/2/");
            client.DefaultRequestHeaders.Add(HttpRequestHeader.ContentType.ToString(), "application/json");


            //fi5worhr5jAAAAAAAAAAHavgkmE11nvRLaAsaqexnvKA37xzFgxY_I5vW5XVpStn

        }

        private string GetUserTokenFromHeader()
        {
            string userToken;
            var o = string.IsNullOrEmpty(Helpers.GetSingleHeaderValue(Request.Headers, USER_HEADER)) ?
                 userTokens.TryGetValue(DEFAULT_USERID, out userToken) :
                 userTokens.TryGetValue(Helpers.GetSingleHeaderValue(Request.Headers, USER_HEADER), out userToken);

            return userToken;
        }



        // GET: camagoauthapi/v1?throwFault=true
        [Route("get")]
        [HttpGet]
        public IEnumerable<string> Get([FromUri] bool throwFault=false)
        {
            if (throwFault)
                throw new ApplicationException("Unexpected error in CA MAG Api Business application");

            return new string[] { "dropboxFolder1", "dropboxFolder2" };
        }




        // GET: camagoauthapi/v1/search/MSPlan?searchFileName=ssm
        [Route("search/{folderPath}")]
        [HttpGet]
        public string SearchFiles(string folderPath, [FromUri] string searchFileName)
        {

            var param = Newtonsoft.Json.JsonConvert.SerializeObject(new { path = "/" + folderPath, query = searchFileName });
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
                   
            client.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), "Bearer " + GetUserTokenFromHeader());
            var response = client.PostAsync("files/search", contentPost).Result;
            //return "{\"access_token\": \"HgUYyC8K3HAAAAAAAAAAI_nWVK7ACWAXIitrSS0Nfe7P1GsV9FcmrknlpXP-DaUV\", \"token_type\": \"bearer\"}"; 
            if (response.StatusCode == HttpStatusCode.OK)
                return response.Content.ReadAsStringAsync().Result;
            else
                return "Error: " + response.StatusCode;
        }





        // POST: camagoauthapi/v1/setToken?token=abc123&sessionId=123456
        [Route("setToken")]
        [HttpPost]
        public void SetToken([FromUri]string token, [FromUri]string sessionId = "")
        {
            if (string.IsNullOrEmpty(sessionId))
                userTokens.AddOrUpdate(DEFAULT_USERID, token, (key, oldValue) => token);
            else
                userTokens.AddOrUpdate(sessionId, token, (key, oldValue) => token);
        }


        // POST: camagoauthapi/getToken?sessionId=123456
        [Route("getToken")]
        [HttpGet]
        public string GetToken([FromUri]string sessionId = "")
        {
            string tokenValue;
            if (string.IsNullOrEmpty(sessionId))
            {
                userTokens.TryGetValue(DEFAULT_USERID, out tokenValue);
                tokenValue = string.Format("{0}:{1}", DEFAULT_USERID, tokenValue);
            }

            else
            {
                userTokens.TryGetValue(sessionId, out tokenValue);
                tokenValue = string.Format("{0}:{1}", sessionId, tokenValue);
            }

            return tokenValue;
        }

  
    }
}

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
    /// This Controller manages interactions with Dropbox using CA MAG. It has no knowledge of OAuth tokens
    /// </summary>
    [RoutePrefix("camagoauthapi/v2")]
    public class CAMagOAuthV2ApiController : ApiController
    {
        HttpClient client = new HttpClient();
        const string USER_HEADER = "X-MS-USERID";
        const string DEFAULT_USERID = "default";
        string sessionID = string.Empty;


        public CAMagOAuthV2ApiController()
        {
            client.BaseAddress = new Uri("https://mag.poc.ms.apim.ca.com:8443/ms-sandbox/dropbox/api/");
            client.DefaultRequestHeaders.Add(HttpRequestHeader.ContentType.ToString(), "application/json");

        }




        // GET: camagoauthapi/v2
        [Route("get")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "dropboxFolder1", "dropboxFolder2" };
        }




        // GET: camagoauthapi/v2/search/MSPlan?searchFileName=ssm
        [Route("search/{folderPath}")]
        [HttpGet]
        public string SearchFiles(string folderPath, [FromUri] string searchFileName)
        {
            try
            {
                var param = Newtonsoft.Json.JsonConvert.SerializeObject(new { path = "/" + folderPath, query = searchFileName });
                HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Add(USER_HEADER, Helpers.GetSingleHeaderValue(Request.Headers, USER_HEADER));

                var response = client.PostAsync("files/search", contentPost).Result;
                //return "{\"access_token\": \"HgUYyC8K3HAAAAAAAAAAI_nWVK7ACWAXIitrSS0Nfe7P1GsV9FcmrknlpXP-DaUV\", \"token_type\": \"bearer\"}"; 
                if (response.StatusCode == HttpStatusCode.OK)
                    return response.Content.ReadAsStringAsync().Result;
                else
                    return "{\"Error\" : \"" + response.StatusCode + "\"}";
            }
            catch (Exception ex)
            {

                string message = ex.ToString();
  
                return "{\"Error\" : \"" + message + " -- " + ex.StackTrace + "\"}";
            }

        }




    }
}

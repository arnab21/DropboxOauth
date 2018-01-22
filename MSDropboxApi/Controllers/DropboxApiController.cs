using MSDropboxApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace MSDropboxApi.Controllers
{
    [RoutePrefix("dropboxapi")]
    public class DropboxApiController : ApiController
    {
        HttpClient client = new HttpClient();
        static string bearerToken = "HgUYyC8K3HAAAAAAAAAAOozF6NDZXvo1Fnoxbsgo_dkrRbgZudfp-vyns7UKHo9-";

        public DropboxApiController()
        {
            client.BaseAddress = new Uri("https://api.dropboxapi.com/2/");
            client.DefaultRequestHeaders.Add(HttpRequestHeader.ContentType.ToString(), "application/json");
            client.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), "Bearer " + bearerToken);
            //fi5worhr5jAAAAAAAAAAHavgkmE11nvRLaAsaqexnvKA37xzFgxY_I5vW5XVpStn
            
        }



        // GET: dropboxapi
        [Route("get")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "dropboxFolder1", "dropboxFolder2" };
        }


        //http://localhost:53565/dropboxapi/acceptToken?state=123456&code=HgUYyC8K3HAAAAAAAAAAF_MOLYw3NLx-PtNkq9HQJc8
        // GET: dropboxapi/acceptToken
        [Route("acceptToken")]
        [HttpGet]
        public HttpResponseMessage AcceptToken([FromUri] string state = "", [FromUri] string code = "")
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(" https://api.dropboxapi.com/");
            client.DefaultRequestHeaders.Add(HttpRequestHeader.ContentType.ToString(), "application/json");

            string redirectUri = Request.RequestUri.Host.Contains("localhost") ? "http%3A%2F%2Flocalhost%3A53565%2Fdropboxapi%2FacceptToken" : "https%3A%2F%2Fmsdropboxapi.azurewebsites.net%2Fdropboxapi%2FacceptToken";

            string tokenParameters = "grant_type=authorization_code&client_id=vj36hmzj1c0ggsm&client_secret=mo2sj6i3qk11z4g&redirect_uri=" + redirectUri + "&code=" + code;
            var param = string.Empty;
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
            var response = client.PostAsync("oauth2/token?"+ tokenParameters, contentPost).Result;
                         
            string responseFragment;
            if (response.StatusCode == HttpStatusCode.OK)
            {

                var responseContent = response.Content.ReadAsStringAsync().Result;
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<DropboxOauthTokenResponse>(responseContent);
                bearerToken = obj.access_token;
                responseFragment = bearerToken;
            }
            else
                responseFragment = "Error: " + response.ReasonPhrase;

            

            var htmlResponse = new HttpResponseMessage();
            var htmlResponseTemplate = @"<html><h1>Dropbox Auth Response</h1><body><div class=""row""><div class=""col - md - 4""><h2>Oauth Token {0}</h2><p> <a class=""btn btn-default"" href=""/DropboxUI/Authorize""> Back</p></div></div></body></html>";
            htmlResponse.Content = new StringContent(string.Format(htmlResponseTemplate, responseFragment));
            htmlResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

                    

            return htmlResponse;             

        }


        // GET: dropboxapi/search/MSPlan?searchFileName=ssm
        [Route("search/{folderPath}")]
        [HttpGet]
        public string Get(string folderPath, [FromUri] string searchFileName)
        {

            var param = Newtonsoft.Json.JsonConvert.SerializeObject(new {path = "/"+folderPath, query = searchFileName });
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");
                   
            var response = client.PostAsync("files/search", contentPost).Result;
            //return "{\"access_token\": \"HgUYyC8K3HAAAAAAAAAAI_nWVK7ACWAXIitrSS0Nfe7P1GsV9FcmrknlpXP-DaUV\", \"token_type\": \"bearer\"}"; 
            if (response.StatusCode == HttpStatusCode.OK)
                return response.Content.ReadAsStringAsync().Result;
            else
                return "Error: " + response.StatusCode;
        }

        // POST: dropboxapi/token
        //public void Post([FromBody]string value)
        //{
        //}

        // POST: dropboxapi/setToken?token=123
        [Route("setToken")]
        [HttpPost]
        public void Post([FromUri]string token)
        {
            bearerToken = token;
        }


    }
}

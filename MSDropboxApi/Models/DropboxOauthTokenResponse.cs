using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSDropboxApi.Models
{
    [Serializable]
    public class DropboxOauthTokenResponse
    {

        public string access_token { get; set; }
        public string token_type { get; set; }
        public string account_id { get; set; }
        public string uid { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace MSDropboxApi.Models
{
    public class Helpers
    {

        public static string GetSingleHeaderValue(HttpRequestHeaders headers, string headerKey)
        {
            IEnumerable<string> headerValues;
            if (headers.TryGetValues(headerKey, out headerValues))
            {
                return headerValues.FirstOrDefault();
            }
            else
                return string.Empty;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSDropboxApi.Controllers
{
    public class CAMagUIController : Controller
    {

        //GET camagui/authorize
        public ActionResult Authorize()
        {
            ViewBag.Title = "CA MAG Dropbox Auth Page";
            //TODO:  We should get UserMsUserId from SSO/Kerberos tokens or X-MS-UserId header.
            ViewBag.MsUserId = "P12ABC";
            ViewBag.Nonce = Guid.NewGuid().ToString();
            return View();
        }
    }
}

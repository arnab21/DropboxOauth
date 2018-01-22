using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSDropboxApi.Controllers
{
    public class DropboxUIController : Controller
    {

        //GET dropboxui/authorize
        public ActionResult Authorize()
        {
            ViewBag.Title = "Dropbox Auth Page";
            //TODO:  We should get UserMsUserId from SSO/Kerberos tokens or X-MS-UserId header.
            ViewBag.MsUserId = "P12ABC";
            ViewBag.Nonce = Guid.NewGuid().ToString();
            return View();
        }
    }
}

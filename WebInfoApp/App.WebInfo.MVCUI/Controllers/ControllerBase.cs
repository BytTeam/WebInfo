using App.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.WebInfo.Business;
using App.WebInfo.MVCUI.Helpers;
using Microsoft.AspNetCore.Http;

namespace App.WebInfo.MVCUI.Controllers
{
    public class ControllerBase : Controller
    {

        public AlertUi alertUi { get; set; }
        public virtual IHttpContextAccessor HttpContextAccessor { get; set; }
        public ControllerBase()
        {

            alertUi = new AlertUi();
        }

        public void AlertUiMessage()
        {
            TempData["alertUi"] = alertUi.ToString();
        }

        public SessionUser GetLoginUser()
        {
            return HttpContextAccessor.HttpContext.Session.GetObject<SessionUser>(AppConst.UserSessionName);
        }
    }
}

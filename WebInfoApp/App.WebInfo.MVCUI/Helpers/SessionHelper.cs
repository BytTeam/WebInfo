using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace App.WebInfo.MVCUI.Helpers
{
    public class SessionHelper
    {
        private const string SessionUserKey = "session_user_key";
        private IHttpContextAccessor _httpContext;
        public SessionHelper(HttpContext httpContext)
        {
            _httpContext.HttpContext = httpContext;
        }

        public SessionUser GetUser()
        {
            return _httpContext.HttpContext.Session.GetObject<SessionUser>(SessionUserKey);
        }

        public void SetUser(SessionUser sessionUser)
        {
            _httpContext.HttpContext.Session.SetObject(SessionUserKey, sessionUser);
        }
    }

    public class SessionUser
    {
        public string UserName { get; set; }
        public bool IsGeneral { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsHelp { get; set; }
        public bool IsEducation { get; set; }
        public bool IsCreate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsReader { get; set; }

    }
}

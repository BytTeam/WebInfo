using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.WebInfo.Business;
using App.WebInfo.MVCUI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.WebInfo.MVCUI.ViewComponents
{
    public class PageActionViewComponet : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PageActionViewComponet(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = new SessionUser();
            if (_httpContextAccessor.HttpContext.Session != null)
            {
                user = _httpContextAccessor.HttpContext.Session.GetObject<SessionUser>(AppConst.UserSessionName);
            }
            return View(user.IsAdmin);
        }
    }
}

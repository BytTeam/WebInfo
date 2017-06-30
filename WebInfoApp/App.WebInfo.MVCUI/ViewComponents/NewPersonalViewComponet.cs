using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.WebInfo.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace App.WebInfo.MVCUI.ViewComponents
{
    public class NewPersonalViewComponet : ViewComponent
    {
        private IPersonalService _personalService;

        public NewPersonalViewComponet(IPersonalService personalService)
        {
            _personalService = personalService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var count =await _personalService.Count(x => !x.IsDelete && x.IsNewItem);

            return View(count);
        }
    }
};

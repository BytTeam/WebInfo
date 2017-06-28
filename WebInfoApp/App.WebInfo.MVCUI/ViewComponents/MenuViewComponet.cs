using App.WebInfo.Business.Abstract;
using App.WebInfo.MVCUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.WebInfo.MVCUI.ViewComponents
{
    public class MenuViewComponet: ViewComponent
    {
        private readonly IMenuService _menuService;
        public MenuViewComponet(IMenuService menuService)
        {
            _menuService = menuService;
        }
        public async Task<IViewComponentResult> InvokeAsync() {

            var list = await _menuService.GetList(x => x.ParentId == 0);
            foreach (var item in list)
            {
                item.SubMenu = await _menuService.GetList(x => x.ParentId == item.MenuId);
            }

            MenuViewModel view = new MenuViewModel()
            {
                list = list
            };
            return View(view);
        }
    }
}

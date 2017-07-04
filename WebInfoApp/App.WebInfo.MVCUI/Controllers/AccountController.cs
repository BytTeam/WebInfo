using App.Core.Utilities;
using App.WebInfo.Business;
using App.WebInfo.Business.Abstract;
using App.WebInfo.Entities.Concrete;
using App.WebInfo.MVCUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.WebInfo.MVCUI.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly RoleManager<CustomIdentityRole> _roleManager;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly IUserRoleService _userRoleManager;

        public AccountController(UserManager<CustomIdentityUser> userManager, RoleManager<CustomIdentityRole> roleManager, SignInManager<CustomIdentityUser> signInManager, IUserRoleService userRoleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userRoleManager = userRoleManager;
        }

        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel
            {
                RoleList = new SelectList(_userRoleManager.GetRoles().Where(x=>!x.Name.Equals("Admin"))
                    .Select(x => new {Id = x.Name, Value = x.Name}), "Id", "Value")
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                CustomIdentityUser user = new CustomIdentityUser
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email
                };

                IdentityResult result =
                    _userManager.CreateAsync(user, registerViewModel.Password).Result;

                if (result.Succeeded)
                {
                   // string[] roleSplit = registerViewModel.Roles.Split(',');
                    foreach (string rol in registerViewModel.Roles)
                    {
                        _userManager.AddToRoleAsync(user, rol).Wait();
                    }
                    
                    return RedirectToAction("Login", "Account");
                }
            }

            return View(registerViewModel);
        }

        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel
            {
                UserName = "yonetici",
                Password = "201408As."
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel, string returnUrl="/")
        {
            if (ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignInAsync(loginViewModel.UserName,
                    loginViewModel.Password, loginViewModel.RememberMe, false).Result;

                if (result.Succeeded)
                {
                    
                    return LocalRedirect(returnUrl);   
                }

                ModelState.AddModelError("", "Invalid login!");
            }

            return View(loginViewModel);
        }
        
        public ActionResult LogOff()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Login");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            List<CustomIdentityUser> userList = await _userManager.Users.ToListAsync();
            return View(userList);
        }

        public async Task<IActionResult> Role()
        {

            CustomIdentityRole generalRole = new CustomIdentityRole
            {
                Name = AppConst.UserRole.General
            };
            await _roleManager.CreateAsync(generalRole);
            CustomIdentityRole privateRole = new CustomIdentityRole
            {
                Name = AppConst.UserRole.Private
            };
            await _roleManager.CreateAsync(privateRole);
            CustomIdentityRole helpRole = new CustomIdentityRole
            {
                Name = AppConst.UserRole.Help
            };
            await _roleManager.CreateAsync(helpRole);
            CustomIdentityRole educationRole = new CustomIdentityRole
            {
                Name = AppConst.UserRole.Education
            };
            await _roleManager.CreateAsync(educationRole);
            CustomIdentityRole createRole = new CustomIdentityRole
            {
                Name = AppConst.UserRole.Create
            };
            await _roleManager.CreateAsync(createRole);
            CustomIdentityRole deleteRole = new CustomIdentityRole
            {
                Name = AppConst.UserRole.Delete
            };
            await _roleManager.CreateAsync(deleteRole);

            alertUi.AlertUiType = AlertUiType.success;
            AlertUiMessage();

            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

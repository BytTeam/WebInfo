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

                RoleList = new SelectList(_userRoleManager.GetRoles().Where(x => !x.Name.Equals("Admin"))
                    .Select(x => new { Id = x.Name, Value = x.Name }), "Id", "Value")
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!string.IsNullOrEmpty(registerViewModel.Id))
            {
                if (string.IsNullOrEmpty(registerViewModel.Password))
                {
                    registerViewModel.Password = "Emty";
                }
            }
            if (ModelState.IsValid)
            {

                IdentityResult result;
                IdentityResult passwordResult;
                CustomIdentityUser user;
                if (!string.IsNullOrEmpty(registerViewModel.Id))
                {
                    user = _userManager.FindByNameAsync(registerViewModel.UserName).Result;

                    user.Email = registerViewModel.Email;
                    result =
                        _userManager.UpdateAsync(user).Result;
                    if (registerViewModel.Password != "Emty")
                    {
                        string tokenAsync = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                        passwordResult = _userManager.ResetPasswordAsync(user, tokenAsync, registerViewModel.Password)
                            .Result;
                    }
                    else
                    {
                        passwordResult = result;
                    }
                }
                else
                {
                    user = new CustomIdentityUser
                    {
                        UserName = registerViewModel.UserName,
                        Email = registerViewModel.Email
                    };
                    result = _userManager.CreateAsync(user, registerViewModel.Password).Result;
                    passwordResult = result;
                }
                if (result.Succeeded && passwordResult.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    if (userRoles != null)
                    {
                        _userManager.RemoveFromRolesAsync(user, userRoles).Wait();
                    }
                    // string[] roleSplit = registerViewModel.Roles.Split(',');
                    foreach (string rol in registerViewModel.Roles)
                    {
                        _userManager.AddToRoleAsync(user, rol.ToLower()).Wait();

                    }

                    return RedirectToAction("List");
                }
            }

            return View(registerViewModel);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return View("Register", new RegisterViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                CurrentPassword = user.PasswordHash,
                Email = user.Email,
                Roles = userRoles.ToArray(),
                RoleList = new SelectList(_userRoleManager.GetRoles().Where(x => !x.Name.Equals("Admin"))
                    .Select(x => new { Id = x.Name, Value = x.Name }), "Id", "Value")
            });

        }
        public async Task<JsonResult> Delete(string id)
        {
            if (id == null)
            {
                return Json(new { isSuccess = false });
            }

            var user = await _userManager.FindByNameAsync(id);
            if (user == null)
            {
                return Json(new { isSuccess = false });
            }
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return Json(new { isSuccess = false });
            }

            return Json(new { isSuccess = true });
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
        public ActionResult Login(LoginViewModel loginViewModel, string returnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignInAsync(loginViewModel.UserName,
                    loginViewModel.Password, loginViewModel.RememberMe, false).Result;

                if (result.Succeeded)
                {

                    return LocalRedirect("/");
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
            List<UserListViewModel> model = userList.Select(x => new UserListViewModel()
            {
                UserName = x.UserName,
                UserRoles = _userManager.GetRolesAsync(x).Result.ToList()
            }).Where(x => !x.UserRoles.Contains("Admin")).ToList();

            return View(model);
        }

        public async Task<IActionResult> Role()
        {

            //CustomIdentityRole generalRole = new CustomIdentityRole
            //{
            //    Name = AppConst.UserRole.General
            //};
            //await _roleManager.CreateAsync(generalRole);
            //CustomIdentityRole privateRole = new CustomIdentityRole
            //{
            //    Name = AppConst.UserRole.Private
            //};
            //await _roleManager.CreateAsync(privateRole);
            //CustomIdentityRole helpRole = new CustomIdentityRole
            //{
            //    Name = AppConst.UserRole.Help
            //};
            //await _roleManager.CreateAsync(helpRole);
            //CustomIdentityRole educationRole = new CustomIdentityRole
            //{
            //    Name = AppConst.UserRole.Education
            //};
            //await _roleManager.CreateAsync(educationRole);
            //CustomIdentityRole createRole = new CustomIdentityRole
            //{
            //    Name = AppConst.UserRole.Create
            //};
            //await _roleManager.CreateAsync(createRole);
            //CustomIdentityRole deleteRole = new CustomIdentityRole
            //{
            //    Name = AppConst.UserRole.Delete
            //};
            //await _roleManager.CreateAsync(deleteRole);

            CustomIdentityRole readerRole = new CustomIdentityRole
            {
                Name = AppConst.UserRole.Reader
            };
            await _roleManager.CreateAsync(readerRole);

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

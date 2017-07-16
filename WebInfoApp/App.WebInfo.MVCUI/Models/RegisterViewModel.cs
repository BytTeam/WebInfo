using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.WebInfo.MVCUI.Models
{
    public class RegisterViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CurrentPassword { get; set; }
        public string[] Roles { get; set; }
        public SelectList RoleList { get; set; }
    }
}
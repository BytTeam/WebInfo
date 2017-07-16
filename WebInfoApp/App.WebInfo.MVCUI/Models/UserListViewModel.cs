using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.WebInfo.MVCUI.Models
{
    public class UserListViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> UserRoles { get; set; }
    }
}

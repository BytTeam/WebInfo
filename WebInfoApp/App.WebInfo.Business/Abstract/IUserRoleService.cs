using App.WebInfo.Entities.Concrete;
using System.Collections.Generic;

namespace App.WebInfo.Business.Abstract
{
    public interface IUserRoleService
    {
        List<CustomIdentityRole> GetRoles();
    }
}

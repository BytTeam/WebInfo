using System;
using System.Collections.Generic;
using System.Text;
using App.WebInfo.Entities.Concrete;

namespace App.WebInfo.DataAccess.Abstract
{
    public interface IUserRoleDal
    {
        List<CustomIdentityRole> GetRoleList();
    }
}

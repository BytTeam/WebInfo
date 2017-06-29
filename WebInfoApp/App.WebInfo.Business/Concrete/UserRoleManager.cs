using App.WebInfo.Business.Abstract;
using App.WebInfo.DataAccess.Abstract;
using App.WebInfo.Entities.Concrete;
using System.Collections.Generic;

namespace App.WebInfo.Business.Concrete
{
    public class UserRoleManager : IUserRoleService
    {
        private readonly IUserRoleDal _userRoleDal;

        public UserRoleManager(IUserRoleDal userRoleDal)
        {
            _userRoleDal = userRoleDal;
        }

        public List<CustomIdentityRole> GetRoles()
        {
            return _userRoleDal.GetRoleList();
        }
    }
}

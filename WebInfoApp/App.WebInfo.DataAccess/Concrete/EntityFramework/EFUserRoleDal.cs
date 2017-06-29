using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.WebInfo.DataAccess.Abstract;
using App.WebInfo.Entities.Concrete;

namespace App.WebInfo.DataAccess.Concrete.EntityFramework
{
    public class EfUserRoleDal: IUserRoleDal
    {
        public List<CustomIdentityRole> GetRoleList()
        {
            using (var context=new CustomIdentityDbContext())
            {
                return context.Roles.ToList();
            }
        }
    }
}

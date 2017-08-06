using System;
using App.WebInfo.DataAccess.Abstract;
using App.WebInfo.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Core.Entites;

namespace App.WebInfo.DataAccess.Concrete.EntityFramework
{
    public class EfUtileDal:IUtileDal

    {
        public async Task<List<T>> GetList<T>(Expression<Func<T, bool>> filter = null) where T : class, IEntity, new()
        {
            using (var context = new WebInfoContext())
            {
                return filter == null ? await context.Set<T>().ToListAsync() : await context.Set<T>().Where(filter).ToListAsync();
            }
        }
    }
}

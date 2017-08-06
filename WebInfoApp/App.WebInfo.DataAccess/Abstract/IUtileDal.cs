using App.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace App.WebInfo.DataAccess.Abstract
{
    public interface IUtileDal
    {
        Task<List<T>> GetList<T>(Expression<Func<T, bool>> filter = null) where T : class, IEntity, new();
    }
}

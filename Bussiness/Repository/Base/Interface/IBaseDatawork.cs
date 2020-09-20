using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models.Entity;

namespace Business.Repository.Interface
{
    public interface IBaseDatawork
    {
        Task<int> SaveChangesAsync();
        void Update<TEntity>(TEntity model);

    }
}

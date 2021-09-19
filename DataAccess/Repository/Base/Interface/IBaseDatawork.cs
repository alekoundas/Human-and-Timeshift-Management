using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository.Interface
{
    public interface IBaseDatawork :IDisposable
    {
        Task<int> SaveChangesAsync();
        void Update<TEntity>(TEntity model);
        void UpdateRange<TEntity>(List<TEntity> models);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Repository.Interface
{
    public interface IBaseDatawork
    {
        Task<int> SaveChangesAsync();
        void Update<TEntity>(TEntity model);
        void UpdateRange<TEntity>(List<TEntity> models);
    }
}

using System;
using System.Threading.Tasks;

namespace Data
{
    public interface IBaseRepository<TEntity> 
    {
        Task<TEntity> GetOne(Guid id);
        Task Create(TEntity item);
        Task Delete(TEntity item);
    }
}
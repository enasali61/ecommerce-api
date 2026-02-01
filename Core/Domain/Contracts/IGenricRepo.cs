using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface IGenricRepo<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
       Task <TEntity?> GetByIdAsync(Tkey id);
        Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);
        Task<TEntity?> GetByIdAsync(Specefication<TEntity> specefication);
        Task<IEnumerable<TEntity>> GetAllAsync(Specefication<TEntity> specefication);
        Task<int> CountAsync(Specefication<TEntity> specefication);
        Task AddAsync(TEntity entity);
        void DeleteAsync(TEntity entity);
        void UpdateAsync(TEntity entity);

    }
}

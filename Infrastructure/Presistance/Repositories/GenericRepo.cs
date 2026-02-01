using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Contracts;
using Presistance.Data;

namespace Presistance.Repositories
{
    public class GenericRepo<TEntity, Tkey> : IGenricRepo<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
        {
            if (asNoTracking == true)
                return await _dbContext.Set<TEntity>().ToListAsync();
            return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync(); // else


        }
        public async Task<TEntity?> GetByIdAsync(Tkey id) => await _dbContext.Set<TEntity>().FindAsync(id);
        
        public async Task AddAsync(TEntity entity) =>  await _dbContext.Set<TEntity>().AddAsync(entity);

        public void DeleteAsync(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);       
        public async void UpdateAsync(TEntity entity) =>_dbContext.Set<TEntity>().Update(entity);

        public async Task<TEntity?> GetByIdAsync(Specefication<TEntity> specefication)
        => await applySpicificaion(specefication).FirstOrDefaultAsync();


        public async Task<IEnumerable<TEntity>> GetAllAsync(Specefication<TEntity> specefication)
       => await applySpicificaion(specefication).ToListAsync();

        private IQueryable<TEntity> applySpicificaion(Specefication<TEntity> specefication)
                   => SpeceficationEvaluator.GetQuery<TEntity>(_dbContext.Set<TEntity>(), specefication);

        public async Task<int> CountAsync(Specefication<TEntity> specefication)
        => await applySpicificaion(specefication).CountAsync();
            
        
    }
}

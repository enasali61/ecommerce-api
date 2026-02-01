using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presistance.Data;

namespace Presistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ConcurrentDictionary<string, object> _repositories;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new();
        }
        public IGenricRepo<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            //return new genric repo<TEntity, Tkey>(_dbcontext)
            // dictionary 
            // key : value
            // product : new genricrepo<product,int>

            //using dictionary but it is too complecated so using an easier function get or add 
            //var typeName = typeof(TEntity).Name; // key
            //if (_repositories.ContainsKey(typeName))
            //{
            //    return (IGenricRepo<TEntity, Tkey>)_repositories(typeName);
            //}
            //else {
            //    var repo = new GenericRepo<TEntity, Tkey>(_dbContext);
            //    return repo;
            //}

           return (IGenricRepo<TEntity, Tkey>)_repositories.GetOrAdd(typeof(TEntity).Name,_ => new GenericRepo<TEntity, Tkey>(_dbContext));
        }

        public async Task<int> SaveChangesAsync()  => await _dbContext.SaveChangesAsync();
        
    }
}

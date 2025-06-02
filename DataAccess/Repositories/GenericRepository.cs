using System.Linq.Expressions;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DomainData.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        //protected readonly LibraryContext _libraryContext;
        protected readonly DbSet<TModel> _dbSet;

        public GenericRepository(ClinicContext context)
        {
            _dbSet = context.Set<TModel>();
        }

        public TModel GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public TModel Create(TModel model)
        {
            var res = _dbSet.Add(model);
            return res.Entity;
        }
        

        public void Update(TModel model)
        {
            _dbSet.Update(model);
        }

        public TModel[] GetAll(
            Expression<Func<TModel, bool>>? search=null,
            Expression<Func<TModel, object>>? orderBy=null,
            Expression<Func<TModel, bool>>? filter= null,
            params string[]? includeFields)
        {
            if (includeFields == null) return _dbSet.ToArray();

            IQueryable<TModel> query = _dbSet;

            foreach (var include in includeFields)
            {
                query = query.Include(include);
            }

            if (search != null)
            {
                query = query.Where(search);
            }

            if (orderBy != null)
                query = query.OrderBy(orderBy);
            
            if (filter != null)
                query = query.Where(filter);

            return query.ToArray();
        }

        public void RemoeAll()
        {
            _dbSet.RemoveRange(_dbSet);
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public bool Exists(Func<TModel, bool> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public TModel? Find(Func<TModel, bool> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }
    }
}
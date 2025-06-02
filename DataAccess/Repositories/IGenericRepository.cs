using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        public TModel GetById(int id);

        public TModel Create(TModel model);

        public TModel[] GetAll(
            Expression<Func<TModel, bool>>? search,
            Expression<Func<TModel, object>>? orderBy,
            Expression<Func<TModel, bool>>? filter= null,
            params string[]? includeFields
        );

        public void Update(TModel model);

        public void Delete(int id);
    }
}
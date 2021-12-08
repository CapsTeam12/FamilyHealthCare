using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }

        Task<T> GetById(Guid id);

        Task<T> GetByAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "");

        Task<IEnumerable<T>> GetAll();

        Task<T> Create(T entity);

        Task<T> Update(T entity);

        Task<T> Delete(T entity);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Linq.Expressions;

namespace API_DataAccess.DataAccess.Internal
{
    public interface IRepository<TEntity> where TEntity : class
    {
		TEntity Get(int id);
		List<TEntity> GetAll();
		//List<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

		long Add(TEntity entity);
		long AddRange(IEnumerable<TEntity> entities);

		bool Update(TEntity entity);

		bool UpdateRange(IEnumerable<TEntity> entities);

		bool Delete(TEntity entity);
		bool DeleteRange(IEnumerable<TEntity> entities);

	}
}

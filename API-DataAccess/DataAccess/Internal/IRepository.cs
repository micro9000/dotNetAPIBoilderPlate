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
		long Add(TEntity entity);
		long AddRange(IEnumerable<TEntity> entities);



		TEntity Get(long id);
		TEntity GetFirstOrDefault(string query, object param);
		
		List<TEntity> GetAll();
		List<TEntity> GetAll(string query, CommandType cmdType = CommandType.Text);
		List<TEntity> GetAll(string query, object p, CommandType cmdType = CommandType.Text);

		Task<List<TEntity>> GetAllAsync(string query, CommandType cmdType = CommandType.Text);
		Task<List<TEntity>> GetAllAsync(string query, object p, CommandType cmdType = CommandType.Text);


		List<T> GetAll<T>() where T : class;
		List<T> GetAll<T>(string query, CommandType cmdType = CommandType.Text) where T : class;
		List<T> GetAll<T>(string query, object p, CommandType cmdType = CommandType.Text) where T : class;

		Task<List<T>> GetAllAsync<T>(string query, CommandType cmdType = CommandType.Text) where T : class;
		Task<List<T>> GetAllAsync<T>(string query, object p, CommandType cmdType = CommandType.Text) where T : class;

		//List<TEntity> Find(Expression<Func<TEntity, bool>> predicate);



		bool Update(TEntity entity);

		bool UpdateRange(IEnumerable<TEntity> entities);

		bool Delete(TEntity entity);
		bool DeleteRange(IEnumerable<TEntity> entities);


	}
}

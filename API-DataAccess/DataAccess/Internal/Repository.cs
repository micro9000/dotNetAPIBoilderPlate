using API_DataAccess.DataAccess;
using API_DataAccess.Internal;
using API_DataAccess.SettingModel;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API_DataAccess.DataAccess.Internal
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        private DatabaseSettings _settings;
        private string _connectionString;
        private Enums.Adapter _dbAdapter;

        public Repository(IOptions<DatabaseSettings> dbOptions)
        {
            this._settings = dbOptions.Value;
            this._connectionString = this._settings.Main.ConnectionString;
            this._dbAdapter = this._settings.Main.Adapter;

            SqlMapperExtensions.GetDatabaseType = conn => this._settings.Main.Adapter.ToString();

        }

        public long Add(TEntity entity)
        {
            long id = 0;
            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                id = conn.Insert(entity);
                conn.Close();
            }
            return id;
        }

        public long AddRange(IEnumerable<TEntity> entities)
        {
            long id = 0;
            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                id = conn.Insert(entities);
                conn.Close();
            }
            return id;
        }

        public bool Delete(TEntity entity)
        {
            bool res = false;
            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                res = conn.Delete(entity);
                conn.Close();
            }
            return res;
        }

        public bool DeleteRange(IEnumerable<TEntity> entities)
        {
            bool res = false;
            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                res = conn.Delete(entities);
                conn.Close();
            }
            return res;
        }

        //public List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        public TEntity Get(long id)
        {
            TEntity res;
            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                res = conn.Get<TEntity>(id);
                conn.Close();
            }
            return res;
        }

        public List<TEntity> GetAll()
        {
            List<TEntity> res = new List<TEntity>();
            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                res = conn.GetAll<TEntity>().ToList();
                conn.Close();
            }
            return res;
        }

        public bool Update(TEntity entity)
        {
            bool res;
            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                res = conn.Update(entity);
                conn.Close();
            }
            return res;
        }

        public bool UpdateRange(IEnumerable<TEntity> entities)
        {
            bool res;
            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                res = conn.Update(entities);
                conn.Close();
            }
            return res;
        }
    }
}

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
using Dapper;
using System.Data;

namespace API_DataAccess.DataAccess.Internal
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        private DatabaseSettings _settings;
        private string _connectionString;
        private Enums.DatabaseAdapter _dbAdapter;

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


        public long Add<T>(T entity) where T : class
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

        // 
        // Get methods
        //

        public T GetValue<T>(string query)
        {
            T res;
            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                res = conn.ExecuteScalar<T>(query);
                conn.Close();
            }
            return res;
        }

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

        public TEntity GetFirstOrDefault(string query, object param, CommandType cmdType = CommandType.Text)
        {
            TEntity result;
            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                result = conn.QueryFirstOrDefault<TEntity>(query, param, commandType: cmdType);
                conn.Close();
            }
            return result;
        }


        public T GetFirstOrDefault<T>(string query, object param, CommandType cmdType = CommandType.Text) where T : class
        {
            T result;
            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                result = conn.QueryFirstOrDefault<T>(query, param, commandType: cmdType);
                conn.Close();
            }
            return result;
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


        public List<TEntity> GetAll(string query, CommandType cmdType = CommandType.Text)
        {
            List<TEntity> results = new List<TEntity>();

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                results = conn.Query<TEntity>(query, commandType: cmdType).ToList();
                conn.Close();
            }

            return results;
        }
  
        public List<TEntity> GetAll(string query, object p, CommandType cmdType = CommandType.Text)
        {
            List<TEntity> results = new List<TEntity>();

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                results = conn.Query<TEntity>(query, p, commandType: cmdType).ToList();
                conn.Close();
            }

            return results;
        }


        public async Task<List<TEntity>> GetAllAsync(string query, CommandType cmdType = CommandType.Text)
        {
            List<TEntity> results = new List<TEntity>();

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                var res = await conn.QueryAsync<TEntity>(query, commandType: cmdType);
                results = res.ToList();
                conn.Close();
            }

            return results;
        }

        public async Task<List<TEntity>> GetAllAsync(string query, object p, CommandType cmdType = CommandType.Text)
        {
            List<TEntity> results = new List<TEntity>();

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                var res = await conn.QueryAsync<TEntity>(query, p, commandType: cmdType);
                results = res.ToList();
                conn.Close();
            }

            return results;
        }


        public List<T> GetAll<T>() where T : class
        {
            List<T> res = new List<T>();
            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                res = conn.GetAll<T>().ToList();
                conn.Close();
            }
            return res;
        }

        public List<T> GetAll<T>(string query, CommandType cmdType = CommandType.Text) where T : class
        {
            List<T> results = new List<T>();

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                results = conn.Query<T>(query, commandType: cmdType).ToList();
                conn.Close();
            }

            return results;
        }

        public List<T> GetAll<T>(string query, object p, CommandType cmdType = CommandType.Text) where T : class
        {
            List<T> results = new List<T>();

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                results = conn.Query<T>(query, p, commandType: cmdType).ToList();
                conn.Close();
            }

            return results;
        }

        public async Task<List<T>> GetAllAsync<T>(string query, CommandType cmdType = CommandType.Text) where T : class
        {
            List<T> results = new List<T>();

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                var res = await conn.QueryAsync<T>(query, commandType: cmdType);
                results = res.ToList();
                conn.Close();
            }

            return results;
        }

        public async Task<List<T>> GetAllAsync<T>(string query, object p, CommandType cmdType = CommandType.Text) where T : class
        {
            List<T> results = new List<T>();

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                var res = await conn.QueryAsync<T>(query, p, commandType: cmdType);
                results = res.ToList();
                conn.Close();
            }

            return results;
        }




        //
        // UPDATING methods
        //


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


        public bool Update<T>(T entity) where T : class
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

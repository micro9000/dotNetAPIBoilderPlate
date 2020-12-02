using API_DataAccess.DataAccess.Internal;
using API_DataAccess.Internal;
using API_DataAccess.Model;
using Microsoft.Extensions.Options;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_DataAccess.SettingModel;
using API_DataAccess.DataAccess.Contracts;

namespace API_DataAccess.DataAccess.Core
{
    public class UserData : Repository<User>, IUserData
    {
        private DatabaseSettings _settings;
        private string _connectionString;
        private Enums.Adapter _dbAdapter;

        public UserData(IOptions<DatabaseSettings> dbOptions) : base (dbOptions)
        {
            this._settings = dbOptions.Value;
            this._connectionString = this._settings.Main.ConnectionString;
            this._dbAdapter = this._settings.Main.Adapter;
        }



        public User Login(string userName, string password)
        {
            User results = new User();
            string query = "SELECT * FROM Users WHERE isDeleted=false AND userName=@Username AND password=@Password";

            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                results = conn.QueryFirstOrDefault<User>(query,
                    new
                    {
                        Username = userName,
                        Password = password
                    });
                conn.Close();
            }

            this.GetUserRoles(results);

            //if (results != null)
            //{
            //    var roles = this.GetRoles(results.Id);

            //    foreach(var role in roles)
            //    {
            //        results.Roles.Add(role);
            //    }
            //}

            return results;
        }


        public async Task<User> GetById (long id)
        {
            User results = new User();
            string query = "SELECT * FROM Users WHERE isDeleted=false AND id=@Id";

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                results = await conn.QueryFirstOrDefaultAsync<User>(query,
                    new
                    {
                        Id = id
                    });
                conn.Close();
            }

            this.GetUserRoles(results);

            return results;
        }



        public List<User> GetAll_exclude_deleted()
        {
            List<User> results = new List<User>();

            string query = @"SELECT * FROM Users WHERE isDeleted=false";

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                results = conn.Query<User>(query).ToList();

                conn.Close();
            }

            return results;
        }


        public List<Role> GetRoles(long userId)
        {
            List<Role> roles = new List<Role>();

            string query = @"SELECT R.*
                            FROM UserRoles AS UR
                            JOIN Users AS U ON UR.userId=U.id
                            JOIN Roles AS R ON UR.roleId=R.id
                            WHERE UR.isDeleted=false AND R.isDeleted=false AND UR.userID=@UserId";

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                roles = conn.Query<Role>(query, 
                new { 
                    UserId = userId
                }).ToList();

                conn.Close();
            }

            return roles;
        }

        private void GetUserRoles(User user)
        {
            if (user != null)
            {
                var roles = this.GetRolesAsync(user.Id);

                foreach (var role in roles)
                {
                    user.Roles.Add(role);
                }
            }
        }

        public List<Role> GetRolesAsync(long userId)
        {
            List<Role> roles = new List<Role>();

            string query = @"SELECT R.*
                            FROM UserRoles AS UR
                            JOIN Users AS U ON UR.userId=U.id
                            JOIN Roles AS R ON UR.roleId=R.id
                            WHERE UR.isDeleted=false AND R.isDeleted=false AND UR.userID=@UserId";

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                roles = conn.QueryAsync<Role>(query,
                new
                {
                    UserId = userId
                }).Result.ToList();

                conn.Close();
            }

            return roles;
        }

    }
}

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
using System.Data;

namespace API_DataAccess.DataAccess.Core
{
    public class UserData : Repository<User>, IUserData
    {
        private readonly IUserRoleData _userRoleData;
        private DatabaseSettings _settings;
        private string _connectionString;
        private Enums.DatabaseAdapter _dbAdapter;

        public UserData(IOptions<DatabaseSettings> dbOptions, IUserRoleData userRoleData) : base (dbOptions)
        {
            this._settings = dbOptions.Value;
            this._connectionString = this._settings.Main.ConnectionString;
            this._dbAdapter = this._settings.Main.Adapter;
            this._userRoleData = userRoleData;
        }


        public long GetCount()
        {
            string query = "SELECT COUNT(*) FROM Users WHERE isDeleted=false";
            long count = this.GetValue<long>(query);
            return count;
        }


        public User GetById (long id)
        {
            User results = new User();
            string query = "SELECT * FROM Users WHERE isDeleted=false AND id=@Id";

            results = this.GetFirstOrDefault(query, new
            {
                Id = id
            });

            this.GetUserRoles(results);

            return results;
        }

        public User GetByEmail(string email)
        {
            User results = new User();
            string query = "SELECT * FROM Users WHERE isDeleted=false AND email=@Email";

            results = this.GetFirstOrDefault(query, new
            {
                Email = email
            });

            this.GetUserRoles(results);

            return results;
        }


        public User GetByUsername(string username)
        {
            User results = new User();
            string query = "SELECT * FROM Users WHERE isDeleted=false AND userName=@Username";

            results = this.GetFirstOrDefault(query, new
            {
                Username = username
            });

            this.GetUserRoles(results);

            return results;
        }

        public User GetByResetToken (string token)
        {
            User results = new User();
            string query = "SELECT * FROM Users WHERE isDeleted=false AND resetToken=@ResetToken";

            results = this.GetFirstOrDefault(query, new
            {
                ResetToken = token
            });
            return results;
        }

        public User GetByVerificationToken(string token)
        {
            User results = new User();
            string query = "SELECT * FROM Users WHERE isDeleted=false AND verificationToken=@Token";

            results = this.GetFirstOrDefault(query, new
            {
                Token = token
            });
            return results;
        }

        public List<User> GetAll_exclude_deleted()
        {
            string query = @"SELECT * FROM Users WHERE isDeleted=false";
            return this.GetAll(query);
        }

        private void GetUserRoles(User user)
        {
            if (user != null)
            {
                var roles = this._userRoleData.GetRolesByUser(user.Id);

                foreach (var role in roles)
                {
                    user.Roles.Add(role);
                }
            }
        }

        //public List<Role> GetRolesAsync(long userId)
        //{
        //    List<Role> roles = new List<Role>();

        //    string query = "spGetUserRoles";

        //    using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
        //    {
        //        roles = conn.QueryAsync<Role>(query,
        //        new
        //        {
        //            UserId = userId
        //        }, commandType: CommandType.StoredProcedure).Result.ToList();

        //        conn.Close();
        //    }

        //    return roles;
        //}

    }
}

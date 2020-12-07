using API_DataAccess.DataAccess.Contracts;
using API_DataAccess.DataAccess.Internal;
using API_DataAccess.Model;
using API_DataAccess.SettingModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_DataAccess.DataAccess.Core
{
    public class UserRoleData : Repository<UserRole>,IUserRoleData
    {
        private DatabaseSettings _settings;
        private string _connectionString;
        private Enums.Adapter _dbAdapter;

        public UserRoleData(IOptions<DatabaseSettings> dbOptions) : base(dbOptions)
        {
            this._settings = dbOptions.Value;
            this._connectionString = this._settings.Main.ConnectionString;
            this._dbAdapter = this._settings.Main.Adapter;
        }

        public bool Delete (long userId, long userRoleId)
        {
            var role = this.Get(userRoleId, userId);
            role.IsDeleted = true;
            role.DeletedAt = DateTime.UtcNow;
            return this.Update(role);
        }

        public UserRole Get(long userRoleId, long userId)
        {
            string query = "SELECT * FROM UserRoles WHERE id=@UserRoleId AND userId=@UserId";
            var userRole = this.GetFirstOrDefault(query, new { UserRoleId = userRoleId, UserId = userId });
            return userRole;
        }

        public Role GetRole(long userRoleId, long userId)
        {
            string query = "spGetUserRole";
            var role = this.GetFirstOrDefault<Role>(query, new { UserId= userId, UserRoleId=userRoleId }, CommandType.StoredProcedure);
            return role;
        }


        public List<Role> GetRolesByUser(long userId)
        {
            string query = "spGetUserRoles";
            var results = this.GetAllAsync<Role>(query, new { UserId = userId }, CommandType.StoredProcedure).Result;
            return results.ToList();
        }

        public List<User> GetUsersByRole(RoleKey role)
        {
            throw new NotImplementedException();
        }
    }
}

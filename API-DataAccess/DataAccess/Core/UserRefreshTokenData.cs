using API_DataAccess.DataAccess.Internal;
using API_DataAccess.Internal;
using API_DataAccess.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using API_DataAccess.SettingModel;
using API_DataAccess.DataAccess.Contracts;

namespace API_DataAccess.DataAccess.Core
{
    public class UserRefreshTokenData : Repository<UserRefreshToken>, IUserRefreshTokenData
    {

        private DatabaseSettings _settings;
        private string _connectionString;
        private Enums.DatabaseAdapter _dbAdapter;
        private readonly IUserRoleData _userRoleData;

        public UserRefreshTokenData(IOptions<DatabaseSettings> dbOptions, IUserRoleData userRoleData) : base(dbOptions)
        {
            this._settings = dbOptions.Value;
            this._connectionString = this._settings.Main.ConnectionString;
            this._dbAdapter = this._settings.Main.Adapter;
            _userRoleData = userRoleData;
        }


        public UserRefreshToken GetByToken(string token)
        {
            UserRefreshToken results = new UserRefreshToken();

            string query = @"SELECT * FROM UserRefreshTokens AS URT
                             INNER JOIN Users AS U ON U.id = URT.userId
                             WHERE U.isDeleted=false AND URT.isDeleted=false AND URT.token = @Token";

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                var tokens = conn.Query<UserRefreshToken, User, UserRefreshToken>(query,
                        (URT, U) =>
                        {
                            if (U != null)
                            {
                                var roles = _userRoleData.GetRolesByUser(U.Id);

                                foreach (var role in roles)
                                {
                                    U.Roles.Add(role);
                                }
                            }

                            URT.UserData = U;
                            return URT;
                        }, new
                        {
                            Token = token
                        }).ToList();

                results = tokens.SingleOrDefault();

                conn.Close();
            }

            return results;

        }

        public List<UserRefreshToken> GetAllByUser(long userId)
        {
            string query = @"SELECT * FROM UserRefreshTokens WHERE isDeleted=false AND userId=@UserId";
            return this.GetAll(query, new { UserId = userId });
        }

        public UserRefreshToken GetByUser(long userId)
        {
            return new UserRefreshToken
            {
                Id = 100,
                UserId = 1,
                Token = "Test",
                Expires = DateTime.Now,
                Created = DateTime.Now,
                RevokedByIp = "testing",
                ReplacedByToken = "test"
            };
        }
    }
}

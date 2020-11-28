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

namespace API_DataAccess.DataAccess.Core
{
    public class UserRefreshTokenData : Repository<UserRefreshToken>, IUserRefreshTokenData
    {

        private DatabaseSettings _settings;
        private string _connectionString;
        private Enums.Adapter _dbAdapter;

        public UserRefreshTokenData(IOptions<DatabaseSettings> dbOptions) : base(dbOptions)
        {
            this._settings = dbOptions.Value;
            this._connectionString = this._settings.Main.ConnectionString;
            this._dbAdapter = this._settings.Main.Adapter;
        }


        public UserRefreshToken GetByToken(string token)
        {
            UserRefreshToken results = new UserRefreshToken();

            string query = @"SELECT * FROM UserRefreshTokens AS URT
                             INNER JOIN Users AS U ON U.id = URT.userId
                             WHERE U.isDeleted = 0 AND URT.isDeleted=0 AND URT.token = @Token";

            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                var tokens = conn.Query<UserRefreshToken, User, UserRefreshToken>(query,
                        (URT, U) =>
                        {
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
            List< UserRefreshToken> results = new List<UserRefreshToken>();

            string query = @"SELECT * FROM UserRefreshTokens WHERE userId=@UserId";

            using (var conn = new WrappedDbConnection(ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter)))
            {
                var tokens = conn.Query<UserRefreshToken>(query, new
                        {
                            UserId = userId
                        }).ToList();

                conn.Close();
            }

            return results;
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

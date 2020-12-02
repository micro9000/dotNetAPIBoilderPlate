﻿using API_DataAccess.DataAccess.Contracts;
using API_DataAccess.DataAccess.Internal;
using API_DataAccess.Internal;
using API_DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using API_DataAccess.SettingModel;
using Microsoft.Extensions.Options;

namespace API_DataAccess.DataAccess.Core
{
    public class RoleData : Repository<Role>, IRoleData
    {
        private DatabaseSettings _settings;
        private string _connectionString;
        private Enums.Adapter _dbAdapter;

        public RoleData(IOptions<DatabaseSettings> dbOptions) : base(dbOptions)
        {
            this._settings = dbOptions.Value;
            this._connectionString = this._settings.Main.ConnectionString;
            this._dbAdapter = this._settings.Main.Adapter;
        }

        public List<Role> GetAll_exclude_deleted()
        {
            List<Role> results = new List<Role>();

            string query = @"SELECT * FROM Roles WHERE isDeleted=false";

            using (var conn = ConnectionFactory.GetDBConnecton(this._connectionString, this._dbAdapter))
            {
                results = conn.Query<Role>(query).ToList();

                conn.Close();
            }

            return results;
        }
    }
}

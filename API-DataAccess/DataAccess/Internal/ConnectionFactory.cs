using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using API_DataAccess.SettingModel;

namespace API_DataAccess.Internal
{
    public static class ConnectionFactory
    {

        public static DbConnection GetDBConnecton (string connectionString, Enums.DatabaseAdapter adapter)
        {
            DbConnection connection = new MySqlConnection(connectionString);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Invalid connection string");
            }

            switch (adapter) {
                case Enums.DatabaseAdapter.sqlconnection:
                    connection = new SqlConnection(connectionString);
                    break;

                case Enums.DatabaseAdapter.npgsqlconnection:
                    connection = new OracleConnection(connectionString);
                    break;
                default:
                    connection = new MySqlConnection(connectionString);
                    break;
            }

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while connecting to the database. See innerException for details.", ex);
            }

            return connection;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_DataAccess.SettingModel
{
    public static class Enums
    {
        public enum Adapter
        {
            sqlconnection,
            sqlceconnection,
            npgsqlconnection,
            sqliteconnection,
            mysqlconnection,
            fbconnection
        }
    }
}

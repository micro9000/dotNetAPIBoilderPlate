using API_DataAccess.SettingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_DataAccess.SettingModel
{
    public class ApplicationDBConnectionSettings
    {
        public string ConnectionString { get; set; }
        public Enums.DatabaseAdapter Adapter { get; set; }
    }

    public class DatabaseSettings
    {
        //public string ConnectionString { get; set; }

        public ApplicationDBConnectionSettings Main { get; set; }

    }
}

using Dapper.Contrib.Extensions;
using System;

namespace API_DataAccess.Model
{
    [Table("Roles")]
    public class Role
    {
        private long id;

        [Key]
        public long Id
        {
            get { return id; }
            set { id = value; }
        }


        private string roleKey;

        public string RoleKey
        {
            get { return roleKey; }
            set { roleKey = value; }
        }

        public int IsDeleted { get; set; }

    }
}

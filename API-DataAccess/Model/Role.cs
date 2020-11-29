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


        private RoleKey roleKey;

        public RoleKey RoleKey
        {
            get { return roleKey; }
            set { roleKey = value; }
        }

        private DateTime createdAt = DateTime.UtcNow;

        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }

        private DateTime updatedAt;

        public DateTime UpdatedAt
        {
            get { return updatedAt; }
            set { updatedAt = value; }
        }

        public DateTime? DeletedAt
        {
            get; set;
        }


        [Write(false)]
        [Computed]
        public bool IsDeleted => this.DeletedAt != null;


    }
}

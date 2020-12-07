using Dapper.Contrib.Extensions;
using System;

namespace API_DataAccess.Model
{
    [Table("Roles")]
    public class Role
    {
        private int id;

        [Key]
        public int Id
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

        private DateTime deletedAt = DateTime.MinValue;

        public DateTime DeletedAt
        {
            get { return deletedAt; }
            set { deletedAt = value; }
        }


        private bool isDeleted;

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }


    }
}

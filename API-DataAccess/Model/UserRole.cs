using Dapper.Contrib.Extensions;
using System;

namespace API_DataAccess.Model
{
    [Table("UserRoles")]
    public class UserRole
    {
        private long id;

        [Key]
        public long Id
        {
            get { return id; }
            set { id = value; }
        }


        private int roleId;

        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }


        private long userId;

        public long UserId
        {
            get { return userId; }
            set { userId = value; }
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

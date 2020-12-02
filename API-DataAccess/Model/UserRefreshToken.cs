using System;
using Dapper.Contrib.Extensions;

namespace API_DataAccess.Model
{
    [Table("UserRefreshTokens")]
    public class UserRefreshToken
    {
        [Key]
        public int Id { get; set; }

        public long UserId { get; set; }

        public string Token { get; set; }
        public DateTime Expires { get; set; }

        [Write(false)]
        [Computed]
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }


        private DateTime revoked = DateTime.MinValue;

        public DateTime Revoked
        {
            get { return revoked; }
            set { revoked = value; }
        }

        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }

        [Write(false)]
        [Computed]
        public bool IsActive => Revoked == DateTime.MinValue && !IsExpired;


        private DateTime createdAt = DateTime.UtcNow;

        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }

        private DateTime updatedAt = DateTime.UtcNow;

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


        [Write(false)]
        [Computed]
        public User UserData { get; set; }

    }
}

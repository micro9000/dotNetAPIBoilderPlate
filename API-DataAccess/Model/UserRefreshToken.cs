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
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }

        [Write(false)]
        [Computed]
        public bool IsActive => Revoked == null && !IsExpired;


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


        [Write(false)]
        [Computed]
        public User UserData { get; set; }

    }
}

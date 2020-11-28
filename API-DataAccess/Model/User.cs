using Dapper.Contrib.Extensions;
using System;

namespace API_DataAccess.Model
{
    [Table("Users")]
    public class User
    {
        private long id;

        [Key]
        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        private string userName;

        public string Username
        {
            get { return userName; }
            set { userName = value; }
        }


        private string fullName;

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }


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

        private DateTime deletedAt = DateTime.UtcNow;

        public DateTime DeletedAt
        {
            get { return deletedAt; }
            set { deletedAt = value; }
        }


        public int IsDeleted { get; set; }

        //[Write(false)]
        //[Computed]
        //public string Projects { get; set; }

    }
}

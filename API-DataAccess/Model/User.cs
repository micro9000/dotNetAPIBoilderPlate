using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

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

        private DateTime updatedAt;

        public DateTime UpdatedAt
        {
            get { return updatedAt; }
            set { updatedAt = value; }
        }

        public DateTime? DeletedAt
        {
            get;set;
        }


        [Write(false)]
        [Computed]
        public bool IsDeleted => this.DeletedAt != null;


        private List<Role> roles = new List<Role>();

        [Write(false)]
        [Computed]
        public List<Role> Roles
        {
            get { return roles; }
            set { roles = value; }
        }
    }
}

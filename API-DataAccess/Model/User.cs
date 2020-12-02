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


        private bool acceptTerms;

        public bool AcceptTerms
        {
            get { return acceptTerms; }
            set { acceptTerms = value; }
        }


        private string verificationToken;

        public string VerificationToken
        {
            get { return verificationToken; }
            set { verificationToken = value; }
        }

        private bool isVerified;

        public bool IsVerified
        {
            get { return isVerified; }
            set { isVerified = value; }
        }

        private string resetToken;

        public string ResetToken
        {
            get { return resetToken; }
            set { resetToken = value; }
        }


        private DateTime resetTokenExpiresAt;

        public DateTime ResetTokenExpiresAt
        {
            get { return resetTokenExpiresAt; }
            set { resetTokenExpiresAt = value; }
        }

        private DateTime passwordResetAt;

        public DateTime PasswordResetAt
        {
            get { return passwordResetAt; }
            set { passwordResetAt = value; }
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

using API_DataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.User
{
    public class UpdateUserRequestDTO
    {
        private string userName;

        public string Username
        {
            get { return userName; }
            set { userName = this.replaceEmptyWithNull(value); }
        }


        private string fullName;

        public string FullName
        {
            get { return fullName; }
            set { fullName = this.replaceEmptyWithNull(value); }
        }


        private string email;

        [EmailAddress]
        public string Email
        {
            get { return email; }
            set { email = this.replaceEmptyWithNull(value); }
        }


        private string password;

        [MinLength(6)]
        public string Password
        {
            get { return password; }
            set { password = this.replaceEmptyWithNull(value);}
        }


        private string confirmPassword;

        [Compare("Password")]
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = this.replaceEmptyWithNull(value); }
        }


        private string replaceEmptyWithNull(string value)
        {
            // replace empty string with null to make field optional
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}

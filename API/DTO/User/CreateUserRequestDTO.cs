using API.CustomAttributes;
using API_DataAccess.Model;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.User
{
    public class CreateUserRequestDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        [EnumDataTypeArray(typeof(RoleKey))]
        public RoleKey[] Roles { get; set; }

    }
}

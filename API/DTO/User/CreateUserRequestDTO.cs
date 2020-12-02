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
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string[] Roles { get; set; }

    }
}

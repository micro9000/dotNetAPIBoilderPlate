using System.ComponentModel.DataAnnotations;

namespace API.DTO.User
{
    public class ForgotPasswordRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

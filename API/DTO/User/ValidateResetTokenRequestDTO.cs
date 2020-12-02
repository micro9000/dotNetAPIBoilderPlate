using System.ComponentModel.DataAnnotations;

namespace API.DTO.User
{
    public class ValidateResetTokenRequestDTO
    {
        [Required]
        public string Token { get; set; }
    }
}

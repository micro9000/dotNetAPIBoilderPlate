using System.ComponentModel.DataAnnotations;

namespace API.DTO.User
{
    public class VerifyEmailRequestDTO
    {
        [Required]
        public string Token { get; set; }
    }
}

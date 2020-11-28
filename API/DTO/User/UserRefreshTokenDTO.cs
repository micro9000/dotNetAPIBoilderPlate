using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.User
{
    public class UserRefreshTokenDTO
    {
        public int Id { get; set; }

        public long UserId { get; set; }

        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive { get; set; }

        public ReadUserDTO UserData { get; set; }
    }
}

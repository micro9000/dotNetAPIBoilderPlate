using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.User
{
    public class UserBaseDTO
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public List<RoleDTO> Roles { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime DeletedAt { get; set; }

    }
}

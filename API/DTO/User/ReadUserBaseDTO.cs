using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.User
{
    public class ReadUserBaseDTO
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

    }
}

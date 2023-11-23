using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.DTO
{
    public class LoginDto
    {
        public bool IsLoggedIn { get; set; }
        public UserDto? User { get; set; }
    }
}

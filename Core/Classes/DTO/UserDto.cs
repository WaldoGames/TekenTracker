using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.DTO
{
    public class UserDto
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string Token { get; set; }
    }
}

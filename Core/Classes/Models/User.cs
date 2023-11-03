using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Models
{
    public class User
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string Token { get; set; }
        public DateTime TokenValidUntil { get; set; }
    }
}

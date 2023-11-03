using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.DTO
{
    public class CheckAccountTokenDTO
    {
        public DateTime ValidUntil { get; set; }
        public string Token {  get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.DTO
{
    //an simplfied post for the main page
    public class PostSimpleDto
    {
        public string Title { get; set; }
        public string MainImageUrl { get; set; }
        public DateTime dateTime { get; set; }
    }
}

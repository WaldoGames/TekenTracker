using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.DTO
{
    internal class SubImageDto
    {
        public int subimageId { get; set; }
        public string imageUrl { get; set; }
        public DateTime uploadDate { get; set; }
    }
}

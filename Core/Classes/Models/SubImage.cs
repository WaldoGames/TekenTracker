using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Models
{
    public class SubImage
    {
        public int? subimageId { get; set; }
        public string imageUrl { get; set; }
        public DateTime uploadDate { get; set; }
        public int postId { get; set; }
    }
}

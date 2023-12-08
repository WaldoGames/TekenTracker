using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Models
{
    public class SubImage
    {
        public int? SubimageId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UploadDate { get; set; }
        public int PostId { get; set; }
    }
}

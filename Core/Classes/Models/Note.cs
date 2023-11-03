using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Models
{
    public class Note
    {
        public int? noteId { get; set; }
        public string text { get; set; }
        public DateTime uploadDate { get; set; }
        public int postId { get; set; }
    }
}

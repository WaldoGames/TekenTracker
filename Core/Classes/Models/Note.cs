using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Models
{
    public class Note
    {
        public int? NoteId { get; set; }
        public string Text { get; set; }
        public DateTime UploadDate { get; set; }
        public int PostId { get; set; }
    }
}

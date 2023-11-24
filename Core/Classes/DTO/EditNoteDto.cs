using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.DTO
{
    public class EditNoteDto
    {
        public int NoteId { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; }
    }
}

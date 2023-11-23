using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{
    public interface INoteRepository
    {
        public Result<NotesDto> GetNotesFromPost(int PostId);

        public Result<NotesDto> GetNoteById(int NoteId);
        public SimpleResult AddNewNote(int PostId, string NewNote);
        public SimpleResult RemoveNewNote(int NoteId);
        public SimpleResult UpdateNote(int NoteId, string NewText);
        public Result<bool> DoesNoteExist(int NoteId);
    }
}

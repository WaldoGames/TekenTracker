using Core.Classes.DTO;
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
        public bool TryGetNotesFromPost(int PostId, out NotesDto? notes);
        public bool TryAddNewNote(int PostId, string NewNote);
        public bool TryRemoveNewNote(int NoteId);
        public bool TryUpdateNote(int NoteId, string NewText);
        public bool DoesNoteExist(int NoteId);
    }
}

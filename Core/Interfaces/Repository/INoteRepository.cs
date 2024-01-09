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
        public Result<NotesDto> GetNotesFromPost(int postId);

        public Result<Note> GetNoteById(int noteId);
        public SimpleResult AddNewNote(int postId, string newNote);
        public SimpleResult RemoveNewNote(int noteId);
        public SimpleResult UpdateNote(int noteId, string newText);
        public Result<bool> DoesNoteExist(int noteId);
    }
}

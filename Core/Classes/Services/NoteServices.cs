using Core.Classes.DTO;
using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Services
{
    internal class NoteServices
    {
        INoteRepository noteRepository;

        public NoteServices(INoteRepository noteRepository)
        {
            this.noteRepository = noteRepository;
        }
        public bool TryAddNewNote(NewNoteDto newNote)
        {
            return noteRepository.TryAddNewNote(newNote.PostId, newNote.Text);
        }
        public bool TryAddManyNotesNewPost(List<NewNoteDto> newNotes, int postId)
        {
            foreach (NewNoteDto newNote in newNotes)
            {
                newNote.PostId = postId;
                if (!TryAddNewNote(newNote))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

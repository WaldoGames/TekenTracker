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
        public SimpleResult AddNewNote(NewNoteDto newNote)
        {
            return noteRepository.AddNewNote(newNote.PostId, newNote.Text);
        }
        public SimpleResult AddManyNotesNewPost(List<NewNoteDto> newNotes, int postId)
        {
            foreach (NewNoteDto newNote in newNotes)
            {
                newNote.PostId = postId;
                if (AddNewNote(newNote).IsFailed)
                {
                    return new SimpleResult { ErrorMessage = "NoteServices->AddManyNotesNewPost: Error passed from AddNewNote" };
                }
            }
            return new SimpleResult { };
        }
    }
}

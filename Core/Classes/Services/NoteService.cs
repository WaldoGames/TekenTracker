using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes.Services
{
    public class NoteService
    {
        INoteRepository noteRepository;

        public NoteService(INoteRepository noteRepository)
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

        public SimpleResult UpdateNote(EditNoteDto note)
        {
            return noteRepository.UpdateNote(note.NoteId, note.Text);
        }

        public NullableResult<Note> GetNoteById(int noteId)
        {
            Result<bool> resultExist = noteRepository.DoesNoteExist(noteId);

            if (resultExist.IsFailed) {
                return new NullableResult<Note> { ErrorMessage = "NoteService->GetNoteById: error passed from noteRepository->doesNoteExist" };
            }

            if(resultExist.Data == false)
            {
                return new NullableResult<Note> { IsEmpty = true };
            }

            Result<Note> result= noteRepository.GetNoteById(noteId);

            if (resultExist.IsFailed)
            {
                return new NullableResult<Note> { ErrorMessage = "NoteService->GetNoteById: error passed from noteRepository->GetNoteById" };
            }
            return new NullableResult<Note>(result);
        }
    }
}

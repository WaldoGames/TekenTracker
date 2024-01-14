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
        INoteRepository NoteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            NoteRepository = noteRepository;
        }
        public SimpleResult AddNewNote(NewNoteDto newNote)
        {
            return NoteRepository.AddNewNote(newNote.PostId, newNote.Text);
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
            return NoteRepository.UpdateNote(note.NoteId, note.Text);
        }

        public NullableResult<Note> GetNoteById(int noteId)
        {
            Result<bool> resultExist = NoteRepository.DoesNoteExist(noteId);

            if (resultExist.IsFailed) {
                return new NullableResult<Note> { ErrorMessage = "NoteService->GetNoteById: error passed from noteRepository->doesNoteExist" };
            }

            if(resultExist.Data == false)
            {
                return new NullableResult<Note> { IsEmpty = true };
            }

            Result<Note> result= NoteRepository.GetNoteById(noteId);

            if (resultExist.IsFailed)
            {
                return new NullableResult<Note> { ErrorMessage = "NoteService->GetNoteById: error passed from noteRepository->GetNoteById" };
            }
            return new NullableResult<Note>(result);
        }
        public SimpleResult DeleteNote(int noteId)
        {
            Result<bool> result = NoteRepository.DoesNoteExist(noteId);

            if (result.IsFailed)
            {
                return new SimpleResult { ErrorMessage = "NoteService->DeleteNote could not confirm exisitance of post" };
            }

            return NoteRepository.RemoveNote(noteId);
        }
    }
}

using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Interfaces.Repository;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Classes.RepositoryImplementations
{
    public class NoteRepository: INoteRepository
    {
        string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=tekentrackerdb";

        public Result<bool> DoesNoteExist(int NoteId)
        {
            throw new NotImplementedException();
        }

        public SimpleResult AddNewNote(int PostId, string NewNote)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO note(text, upload_date, post_id) VALUES(@newNote, @date, @postId)", con);
                    cmd.Parameters.AddWithValue("@postId", PostId);
                    cmd.Parameters.AddWithValue("@newNote", NewNote);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader rdr = cmd.ExecuteReader();               
                    con.Close();
                    return new SimpleResult();
                }
            }
            catch (Exception e)
            {
                return new SimpleResult { ErrorMessage = "NoteRepository->AddNewNote" + e.Message };
                throw;
            }
        }

        public Result<NotesDto> GetNotesFromPost(int PostId)
        {
            NotesDto notes = new NotesDto();
            notes.Notes = new List<Note>();
            using (MySqlConnection con = new MySqlConnection(CS))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM note WHERE post_id = @id", con);
                    cmd.Parameters.AddWithValue("@id", PostId);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (notes == null)
                        {
                            notes = new NotesDto();
                            notes.Notes = new List<Note>();
                        }


                        Note note = new Note();
                        note.text = Convert.ToString(rdr["text"]);
                        note.uploadDate = Convert.ToDateTime(rdr["upload_date"]);
                        note.noteId = Convert.ToInt32(rdr["note_id"]);
                        notes.Notes.Add(note);

                    }

                    con.Close();
                    return new Result<NotesDto> { Data = notes };
                }
                catch (Exception e)
                {
                    con.Close();
                    return new Result<NotesDto> { ErrorMessage = "NoteRepository->GetNotesFromPost: " + e.Message };
                }
            }
            return new Result<NotesDto> { ErrorMessage = "NoteRepository->GetNotesFromPost: unkown error" };
        }

        public SimpleResult RemoveNewNote(int NoteId)
        {
            throw new NotImplementedException();
        }

        public SimpleResult UpdateNote(int NoteId, string NewText)
        {
            throw new NotImplementedException();
        }
    }
}

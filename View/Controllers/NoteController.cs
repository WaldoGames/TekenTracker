using Core.Classes.DTO;
using Core.Classes.Services;
using Core.Classes;
using Microsoft.AspNetCore.Mvc;
using View.Models;
using Dal.Classes.RepositoryImplementations;
using Microsoft.Extensions.Caching.Memory;
using Core.Classes.Models;

namespace View.Controllers
{
    public class NoteController : LoggedinControllerBase
    {
        PostService postService;
        TagService tagService;
        NoteService noteService;
        SessionController sessionController;
        IWebHostEnvironment webHost;

        public NoteController(IMemoryCache cache, IWebHostEnvironment webHost) : base(cache)
        {
            postService = new PostService(new PostRepository(), new NoteRepository(), new SubimageRepository(), new TagRepository());
            tagService = new TagService(new TagRepository());
            noteService = new NoteService(new NoteRepository());
            sessionController = new SessionController(cache);
            this.webHost = webHost;
        }
        public ActionResult Create(int id)
        {
            NewNoteDto newNote = new NewNoteDto();

            newNote.PostId = id;

            return View(newNote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, NewNoteDto newNote)
        {
            newNote.PostId = id;
            //create the post (make sure to return the id)
            if (!CheckLogin(out ActionResult LoginView))
            {
                return LoginView;
            }

            SimpleResult result = noteService.AddNewNote(newNote);
            if (result.IsFailed)
            {
                return View("error");
            }

            return RedirectToAction("Details", "Post", new { id = id });//return tagedit window(make sure id is in link)
        }

        public ActionResult Edit(int id)
        {
            EditNoteDto newNote = new EditNoteDto();


            NullableResult<Note> note = noteService.GetNoteById(id);

            if (note.IsEmpty)
            {
                //not found
            }
            if (note.IsFailed)
            {
                //error
            }


            newNote.NoteId = id;
            newNote.Text = note.Data.Text;
            newNote.PostId = note.Data.PostId;

            return View(newNote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditNoteDto note)
        {
            note.NoteId = id;
            SimpleResult result = noteService.UpdateNote(note);

            if (result.IsFailed)
            {
                return View("Error");
            }
            NullableResult<Note> ReturnNote = noteService.GetNoteById(id);
            if (ReturnNote.IsFailed || ReturnNote.IsEmpty)
            {
                return View("Error");
            }

            return RedirectToAction("Details", "Post", new { id = ReturnNote.Data.PostId });//return tagedit window(make sure id is in link)

        }


        public ActionResult Delete(int id)
        {
            Result<Note> tmpNote = noteService.GetNoteById(id);

            if(tmpNote.IsFailed)
            {
                return View("error");
            }

            SimpleResult sr = noteService.DeleteNote(id);

            if (sr.IsFailed)
            {
                return View("error");
            }

            return RedirectToAction("Details", "Post", new { id = tmpNote.Data.PostId });
        }
    }
}

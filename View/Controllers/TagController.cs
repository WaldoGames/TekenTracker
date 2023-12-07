using Core.Classes.DTO;
using Core.Classes.Services;
using Core.Classes;
using Microsoft.AspNetCore.Mvc;
using Dal.Classes.RepositoryImplementations;
using Microsoft.Extensions.Caching.Memory;
using View.Models;
using Core.Classes.Enums;

namespace View.Controllers
{
    public class TagController : LoggedinControllerBase
    {
        TagService tagService;
        SessionController sessionController;
        IWebHostEnvironment webHost;

        public TagController(IMemoryCache cache, IWebHostEnvironment webHost) : base(cache)
        {
            tagService = new TagService(new TagRepository());
            sessionController = new SessionController(cache);
            this.webHost = webHost;
        }

        public ActionResult Create(int id)
        {
            if (!CheckLogin(out ActionResult LoginView))
            {
                return LoginView;
            }

            NewTagViewModel vm = new NewTagViewModel();
          
            vm.TagsList = new List<TagTypes>
            {
                TagTypes.Search,
                TagTypes.Improvement,
            };

            ViewBag.Types = vm.TagsList;
            vm.ReturnPostId = id;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, NewTagViewModel NewTag)
        {
            //create the post (make sure to return the id)
            if (!CheckLogin(out ActionResult LoginView))
            {
                return LoginView;
            }

            SimpleResult result = tagService.CreateNewTag(NewTag.Title,(int)NewTag.TagType);
            if (result.IsFailed)
            {
                return View("error");
            }

            return RedirectToAction("EditTagsFromPost", "Post", new { id = id });//return tagedit window(make sure id is in link)
        }
    }
}

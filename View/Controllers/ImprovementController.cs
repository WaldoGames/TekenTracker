using Core.Classes;
using Core.Classes.DTO;
using Core.Classes.Services;
using Dal.Classes.RepositoryImplementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MySqlX.XDevAPI;
using View.Models;

namespace View.Controllers
{
    public class ImprovementController : LoggedinControllerBase
    {
        TagService tagService;
        SessionController sessionController;
        IWebHostEnvironment webHost;

        ImprovementSearchLimit Standard = new ImprovementSearchLimit
        {
            timeOrAmount = TimeOrAmount.time,
            Reach = 365 //1 year default i guess
        };
        public ImprovementController(IMemoryCache cache, IWebHostEnvironment webHost) : base(cache)
        {
            tagService = new TagService(new TagRepository());
            sessionController = new SessionController(cache);
            this.webHost = webHost;
        }



        // GET: ImprovementController
        public ActionResult Index()
        {
            if (!CheckLogin(out ActionResult LoginView))
            {
                return LoginView;
            }

            sessionController.GetUserFromSession(out UserDto user);
            //user.userId

            Result<List<TagAndAmount>> tags = tagService.GetTagsForImprovementWindow(Standard, user.userId);

            if (tags.IsFailed)
            {
                //return error
            }
            ImprovementViewModel improvementViewModel = new ImprovementViewModel();
            improvementViewModel.Returned = tags.Data;
            improvementViewModel.SearchLimits = Standard;

            return View(improvementViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: ImprovementController/Create
        public ActionResult Index(ImprovementViewModel improvementViewModel)
        {
            if (!CheckLogin(out ActionResult LoginView))
            {
                return LoginView;
            }

            sessionController.GetUserFromSession(out UserDto user);
            //user.userId

            Result<List<TagAndAmount>> tags = tagService.GetTagsForImprovementWindow(improvementViewModel.SearchLimits, user.userId);

            if (tags.IsFailed)
            {
                //return error
            }
            improvementViewModel.Returned = tags.Data;

            return View(improvementViewModel);
        }

    }
}

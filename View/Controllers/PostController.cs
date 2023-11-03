using Core.Classes.DTO;
using Core.Classes.Services;
using Dal.Classes.RepositoryImplementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace View.Controllers
{
    public class PostController : Controller
    {
        // GET: PostController
        UserService userService = new UserService(new UserRepository());
        SessionController sessionController;

        public PostController(IMemoryCache cache)
        {
            sessionController = new SessionController(cache);
        }
        public ActionResult Index()
        {
            
            if (sessionController.GetUserFromSession(out UserDto dto) && userService.ChecKLoginStatus(dto))
            {
                return View();
            }

            return RedirectToAction("Index", "User", new { area = "" });


        }

        // GET: PostController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Classes.Services;
using Core.Classes;
using Dal.Classes.RepositoryImplementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MySqlX.XDevAPI.Common;
using System.Text;
using View.Models;

namespace View.Controllers
{
    public class UserController : LoggedinControllerBase
    {
        //return View("~/Views/Wherever/SomeDir/MyView.aspx")
        // GET: UserController
        UserService userService = new UserService(new UserRepository());
        //PostService
        SessionController sessionController;
        public UserController(IMemoryCache cache):base(cache) 
        {
            sessionController = new SessionController(cache);
        }
        public ActionResult Index()
        {
            if (!CheckLogin(out ActionResult LoginView))
            {
                return View();
            }

            return RedirectToAction("LogedIn");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginObject LoginObject)
        {

            Result<LoginDto> loginDto = userService.TryLogin(LoginObject.username, LoginObject.password);

            if (loginDto.IsFailed || loginDto.Data.IsLoggedIn == false || loginDto.Data.User == null)
            {
                return View();
            }

            sessionController.AddUserToSession(loginDto.Data.User);

            return RedirectToAction("LogedIn");
        }
        public ActionResult LogedIn()
        {
            UserObject user = new UserObject();
            sessionController.GetUserFromSession(out UserDto? userDto);

            UserLogedinModel model = new UserLogedinModel();
            PostService ps = new PostService(new PostRepository(), new NoteRepository(), new SubimageRepository(), new TagRepository());

            if (userDto != null)
            {
                user.username = userDto.UserName;
                user.email = userDto.Email;
                model.username = userDto.UserName;
                Result<List<string>> sl = ps.GetRandomImagesFromUser(userDto.UserId);
                if (!sl.IsFailed)
                {
                    model.Images = sl.Data;
                }
                

                return View(model);
            }
            else
            {
                return RedirectToAction("error");
                //error?
            }
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateNewUser nu)
        {


            NewUserDto newUserDto = new NewUserDto();
            newUserDto.UserName = nu.username;
            newUserDto.Email = nu.email;
            newUserDto.Password = nu.password;

            userService.CreateNewUser(newUserDto);

            return RedirectToAction("Index");
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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

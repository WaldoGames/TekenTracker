using Core.Classes.DTO;
using Core.Classes.Models;
using Core.Classes.Services;
using Dal.Classes.RepositoryImplementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using View.Models;

namespace View.Controllers
{
    public class UserController : LoggedinControllerBase
    {
        //return View("~/Views/Wherever/SomeDir/MyView.aspx")
        // GET: UserController
        UserService userService = new UserService(new UserRepository());
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

            return View("LogedIn");
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginObject LoginObject)
        {
            
            if (!userService.TryLogin(LoginObject.username, LoginObject.password, out UserDto user))
            {
                return View();
            }

            sessionController.AddUserToSession(user);

            return RedirectToAction("LogedIn");
        }
        public ActionResult LogedIn()
        {
            UserObject user = new UserObject();
            sessionController.GetUserFromSession(out UserDto? userDto);
            if (userDto != null)
            {
                user.username = userDto.userName;
                user.email = userDto.email;
                return View(user);
            }
            else
            {
                return View();
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
            newUserDto.userName = nu.username;
            newUserDto.email = nu.email;
            newUserDto.password = nu.password;

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

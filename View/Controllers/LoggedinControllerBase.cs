using Core.Classes.DTO;
using Core.Classes.Services;
using Dal.Classes.RepositoryImplementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace View.Controllers
{
    public class LoggedinControllerBase : Controller
    {
        UserService BaseUserService = new UserService(new UserRepository());
        SessionController BaseSessionController;

        public LoggedinControllerBase(IMemoryCache cache)
        {
            BaseSessionController = new SessionController(cache);
        }
        public bool CheckLogin(out ActionResult LoginView)
        {
            LoginView = RedirectToAction("Index", "User", new { area = "" });

            if (BaseSessionController.GetUserFromSession(out UserDto dto) && BaseUserService.ChecKLoginStatus(dto))
            {
                return true;
            }
            return false;
        }
    }
}

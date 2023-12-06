using Core.Classes.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using View.Models;

namespace View.Controllers
{
    public class ImprovementController : Controller
    {
        ImprovementSearchLimit Standard = new ImprovementSearchLimit
        {
            timeOrAmount = TimeOrAmount.time,
            Reach = 365 //1 year default i guess
        };

        // GET: ImprovementController
        public ActionResult Index()
        {
            return View();
        }

        // POST: ImprovementController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ImprovementViewModel improvementViewModel)
        {
            return View(improvementViewModel);
        }

    }
}

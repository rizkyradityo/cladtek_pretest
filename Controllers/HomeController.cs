using System;
using System.Web.Mvc;

namespace PTXYZ_OvertimeApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home/Index - Main Menu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}

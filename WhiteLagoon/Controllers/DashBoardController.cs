using Microsoft.AspNetCore.Mvc;

namespace WhiteLagoon.Web.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

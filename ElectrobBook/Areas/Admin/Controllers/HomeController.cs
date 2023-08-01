using Microsoft.AspNetCore.Mvc;

namespace OnlineBook.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area(nameof(Admin))]
        public IActionResult Index()
        {
            return View();
        }

        
             public IActionResult Denied()
        {
            return View();
        }
    }
}

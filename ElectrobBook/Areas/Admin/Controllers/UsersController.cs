using Microsoft.AspNetCore.Mvc;

namespace OnlineBook.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        [Area(nameof(Admin))]

        public IActionResult Index()
        {
            return View();
        }
    }
}

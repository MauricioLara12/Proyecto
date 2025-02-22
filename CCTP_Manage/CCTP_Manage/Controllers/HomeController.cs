using Microsoft.AspNetCore.Mvc;

namespace CCTP_Manage.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CCTP_Manage.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace DoneTool.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}

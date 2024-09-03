using Microsoft.AspNetCore.Mvc;

namespace PPKBeverageManagement.Controllers
{
    public class NguoiDungController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PPKBeverageManagement.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHangController
        public ActionResult Index()
        {
            return View();
        }
        public IActionResult PaymentSuccess()
        {
            return View();
        }
        public IActionResult PaymentFailure()
        {
            return View();
        }
    // GET: GioHangController/Details/5
    public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GioHangController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GioHangController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GioHangController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GioHangController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GioHangController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GioHangController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PPKBeverageManagement.Models;
using System.Linq;
using PPKBeverageManagement.Extensions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace PPKBeverageManagement.Controllers
{
    public class KhachHangController : Controller
    {
        QUANLYCAPHEContext da = new QUANLYCAPHEContext();
        public readonly IHttpContextAccessor _httpContextAccessor;
        private string HoKh, TenKh, UserName, Pass, DiaChi, SoDienThoai;
        public KhachHangController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
		// GET: KhachHangController
		public ActionResult ListBeverage(string? kw, string? page, string? size, int? gia)
		{
			string check = _httpContextAccessor.HttpContext.Session.GetString("UserName");
			if (check != null)
			{
				ViewData["SuccessMessage"] = _httpContextAccessor.HttpContext.Session.GetString("SuccessMessage");

				// Lấy tên người dùng từ session
				var nameH = _httpContextAccessor.HttpContext.Session.GetString("HoKh");
				var name = _httpContextAccessor.HttpContext.Session.GetString("TenKh");

				if (!string.IsNullOrEmpty(nameH) && !string.IsNullOrEmpty(name))
				{
					ViewData["FullName"] = $"{nameH} {name}";
				}
				else
				{
					ViewData["FullName"] = "Khách hàng";
				}

				// Xóa thông báo từ session
				var ds = da.SanPhams.Include(c => c.Size).ToList();
				_httpContextAccessor.HttpContext.Session.Remove("SuccessMessage");
				if (!string.IsNullOrEmpty(kw))
				{
					ds = ds.Where(s => s.Ten.Contains(kw)).ToList();
				}
				if (!string.IsNullOrEmpty(size))
				{
					int sizeid = int.Parse(size);
					ds = ds.Where(s => s.SizeId == sizeid).ToList();
				}
				if (gia != null)
				{
					ds = ds.Where(s => s.Tien <= (decimal)gia).ToList();
				}
				int pageSize = 4;
				if (!string.IsNullOrEmpty(page))
				{
					int pageNumber = int.Parse(page);

					ds = da.SanPhams
							  .OrderBy(item => item.Id)
							  .Skip((pageNumber - 1) * pageSize)//Lấy từ vị trí
							  .Take(pageSize)//Lấy bao nhiêu
							  .ToList();
				}

				return View(ds);
			}
			else
				return RedirectToAction("DangNhap");



		}

		// GET: KhachHangController/Details/5
		public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KhachHangController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KhachHangController/Create
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

        // GET: KhachHangController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: KhachHangController/Edit/5
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

        // GET: KhachHangController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KhachHangController/Delete/5
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
        public ActionResult DangKy()
        {
            //KhachHang kh = new KhachHang();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DangKyAsync(IFormCollection collection)
        {
            // Gán các giá tị người dùng nhập liệu cho các biến
            HoKh = collection["HoKh"];
            TenKh = collection["TenKh"];
            UserName = collection["UserName"];
            Pass = collection["Pass"];
            DiaChi = collection["DiaChi"];
            SoDienThoai = collection["SoDienThoai"];
            _httpContextAccessor.HttpContext.Session.SetString("Pass", Pass);
            _httpContextAccessor.HttpContext.Session.SetString("DiaChi", DiaChi);
            _httpContextAccessor.HttpContext.Session.SetString("HoKh", HoKh);
            _httpContextAccessor.HttpContext.Session.SetString("TenKh", TenKh);
            _httpContextAccessor.HttpContext.Session.SetString("UserName", UserName);
            _httpContextAccessor.HttpContext.Session.SetString("SoDienThoai", SoDienThoai);

            if (System.String.IsNullOrEmpty(HoKh))
            {
                ViewData["Loil"] = "Họ khách hàng không được để trống";
            }
            else if (System.String.IsNullOrEmpty(TenKh))
            {
                ViewData["Loi2"] = "Tên khách hàng không được để trống";
            }
            else if (System.String.IsNullOrEmpty(UserName))
            {
                ViewData["Loi3"] = "Phải nhập tên đăng nhập";
            }
            else if (System.String.IsNullOrEmpty(Pass))
            {
                ViewData["Loi4"] = "Phải nhập mật khẩu";
            }
            else if (System.String.IsNullOrEmpty(DiaChi))
            {
                ViewData["Loi5"] = "Phải nhập địa chỉ";
            }
            else if (System.String.IsNullOrEmpty(SoDienThoai))
            {
                ViewData["Loi6"] = "Phải nhập số điện thoại";
            }
            else
            {
                ////Gán giá trị cho đối tượng được tạo mới (kh)
                //kh.HoKh = ho;
                //kh.TenKh = ten;
                //kh.Pass = pass;
                //kh.DiaChi = diachi;
                //kh.SoDienThoai = dienthoai;
                string internationalPhoneNumber = "+84" + SoDienThoai.Substring(1);
                Console.WriteLine(internationalPhoneNumber);
                //var otpService = new OtpService(_configuration, _httpContextAccessor);
                //otpService.SendOtp(internationalPhoneNumber); // Truyền số điện thoại cần gửi mã OTP vào hàm SendOtp

                _httpContextAccessor.HttpContext.Session.SetString("UserPhoneNumber", SoDienThoai);
                return RedirectToAction("VerifyOtp");
            }
            return this.DangKy();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(IFormCollection collection)
        {
            var uname = collection["UserName"];
            var pass = collection["Pass"];
            var idKH = collection["MaKh"];

            if (System.String.IsNullOrEmpty(uname))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (System.String.IsNullOrEmpty(pass))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KhachHang kh = da.KhachHangs.SingleOrDefault(k => k.UserName.Equals(uname.ToString()) && k.Pass.Equals(pass.ToString()));
                if (kh != null)
                {
                    _httpContextAccessor.HttpContext.Session.SetInt32("MaKh", kh.MaKh);
                    _httpContextAccessor.HttpContext.Session.SetString("UserName", kh.UserName);
                    _httpContextAccessor.HttpContext.Session.SetString("HoKh", kh.HoKh); // Lưu họ vào session
                    _httpContextAccessor.HttpContext.Session.SetString("TenKh", kh.TenKh); // Lưu tên vào session
                    _httpContextAccessor.HttpContext.Session.SetString("SuccessMessage", "Chúc mừng đăng nhập thành công");
                    return RedirectToAction("ListBeverage");
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không đúng.");
                    return View();
                }
            }
            return View();
        }
		public List<GioHang> GetListCarts()
		{
			List<GioHang> carts = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<GioHang>>("GioHang");

			//Chưa có thì tạo mới giỏ hàng trống
			if (carts == null)
			{
				carts = new List<GioHang>();
			}
			//Có rồi thì lấy các sp trả về
			return carts;
		}
	}
}

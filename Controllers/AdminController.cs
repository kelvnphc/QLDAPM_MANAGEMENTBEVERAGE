using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PPKBeverageManagement.Models;
using System.IO;
using System;
using System.Linq;
using PPKBeverageManagement.ViewModels;
using System.Threading.Tasks;
using PPKBeverageManagement.Extensions;
using System.Collections.Generic;


namespace PPKBeverageManagement.Controllers
{
    public class AdminController : Controller
    {
        QUANLYCAPHEContext da = new QUANLYCAPHEContext();
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AdminController> _logger;
        public AdminController(IHttpContextAccessor httpContextAccessor, ILogger<AdminController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        // GET: AdminController
        public string UploadImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                Account account = new Account(
                    "dtq05equd",
                    "352349598637749",
                    "eswxczv-nDvwhpe9VUs5qUizy5E"
                );

                Cloudinary cloudinary = new Cloudinary(account);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(fileName, stream)
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);

                    // Lấy URL của ảnh đã tải lên từ kết quả
                    string imageUrl = uploadResult.SecureUri.ToString();

                    // Lưu URL vào cơ sở dữ liệu hoặc thực hiện các thao tác khác
                    // Ví dụ: return URL để sử dụng trong View
                    return imageUrl;
                }
            }
            else
            {
                // Trả về thông báo lỗi nếu không có file được chọn
                return "Error";
            }

        }
        private bool IsAdminLoggedIn()
        {
            string username = _httpContextAccessor.HttpContext.Session.GetString("UserName");
            return !string.IsNullOrEmpty(username) && username == "admin";
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
            var idQL = collection["MaQl"];

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
                QuanLy ql = da.QuanLies.SingleOrDefault(k => k.UserName.Equals(uname.ToString()) && k.Pass.Equals(pass.ToString()));
                if (ql != null)
                {
                    _httpContextAccessor.HttpContext.Session.SetInt32("MaQl", ql.MaQl);
                    _httpContextAccessor.HttpContext.Session.SetString("UserName", ql.UserName);
                    _httpContextAccessor.HttpContext.Session.SetString("HoQl", ql.HoQl); // Lưu họ vào session
                    _httpContextAccessor.HttpContext.Session.SetString("TenQl", ql.TenQl); // Lưu tên vào session
                    TempData["IsLoggedIn"] = true;
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
        // GET: AdminController
        public ActionResult ListBeverage(string? kw, string? page, string? size, int? gia)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("DangNhap");
            }
            string check = _httpContextAccessor.HttpContext.Session.GetString("UserName");
            if (check != null)
            {


                // Khởi tạo truy vấn với các điều kiện lọc ban đầu
                var query = da.SanPhams.Include(sp => sp.Loai).Include(sp => sp.Size).AsQueryable();

                // Lọc theo từ khóa
                if (!string.IsNullOrEmpty(kw))
                {
                    query = query.Where(s => s.Ten.Contains(kw));
                }

                // Lọc theo kích thước (size)
                if (!string.IsNullOrEmpty(size))
                {
                    int sizeid;
                    if (int.TryParse(size, out sizeid))
                    {
                        query = query.Where(s => s.SizeId == sizeid);
                    }
                }

                // Lọc theo giá
                if (gia != null)
                {
                    query = query.Where(s => s.Tien <= (decimal)gia);
                }

                // Xử lý phân trang
                int pageSize = 4;
                int pageNumber = 1; // Trang mặc định là 1

                if (!string.IsNullOrEmpty(page))
                {
                    int.TryParse(page, out pageNumber);
                }

                // Lấy các bản ghi dựa trên trang hiện tại
                var ds = query
                         .OrderBy(item => item.Id)
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();

                return View(ds);
            }
            else
            {
                return RedirectToAction("DangNhap");
            }
        }
        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            var sp = da.SanPhams.Include(c => c.Loai).Include(c => c.Size).FirstOrDefault(s => s.Id == id);
            return View(sp);
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {

            ViewData["Sizes"] = new SelectList(da.Sizes, "Id", "Ten");
            ViewData["Loais"] = new SelectList(da.LoaiSanPhams, "LoaiId", "TenLoai");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, IFormFile Anh)
        {
            try
            {

                string imageUrl = "";
                if (Anh != null && Anh.Length > 0)
                {
                    // Gọi hàm UploadImage để upload ảnh lên Cloudinary và nhận lại URL của ảnh
                    imageUrl = UploadImage(Anh);

                    // Gán URL của ảnh cho thuộc tính Anh của model


                }
                // Lấy SizeID và giá từ form

                // Lấy SizeID và giá từ form
                int sizeID = int.Parse(collection["SizeId"]);
                int loaiID = int.Parse(collection["LoaiId"]);
                Console.WriteLine(sizeID);
                SanPham cp = new SanPham();
                cp.MieuTa = collection["MieuTa"];
                cp.Ten = collection["Ten"];
                cp.Anh = imageUrl;
                cp.SizeId = sizeID;
                cp.Tien = Decimal.Parse(collection["Tien"]);
                cp.LoaiId = loaiID;

                da.SanPhams.Add(cp);
                da.SaveChanges();
                return RedirectToAction("ListBeverage");

            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["Sizes"] = new SelectList(da.Sizes, "Id", "Ten");
            ViewData["Loais"] = new SelectList(da.LoaiSanPhams, "LoaiId", "TenLoai");
            var p = da.SanPhams.Include(c => c.Size).FirstOrDefault(s => s.Id == id);
            return View(p);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, IFormFile Anh)
        {

            try
            {
                var cp = da.SanPhams.Include(c => c.Size).FirstOrDefault(s => s.Id == id);
                string anh = cp.Anh;
                int sizeID = int.Parse(collection["SizeId"]);
                int loaiID = int.Parse(collection["LoaiId"]);

                cp.MieuTa = collection["MieuTa"];
                cp.Ten = collection["Ten"];

                cp.SizeId = sizeID;
                cp.Tien = Decimal.Parse(collection["Tien"]);
                cp.LoaiId = loaiID;
                if (Anh != null && Anh.Length > 0)
                {
                    // Tải tệp hình ảnh mới lên Cloudinary và lấy URL của nó
                    string imageUrl = UploadImage(Anh);

                    // Gán URL của hình ảnh mới cho sản phẩm cà phê
                    cp.Anh = imageUrl;
                }
                else
                {
                    // Nếu không có tệp hình ảnh mới được tải lên, giữ nguyên URL của hình ảnh cũ
                    cp.Anh = anh;
                }
                // Gửi các thay đổi đến cơ sở dữ liệu
                da.SaveChanges();

                // Chuyển hướng đến danh sách sản phẩm sau khi cập nhật thành công
                return RedirectToAction("ListBeverage");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            var sp = da.SanPhams.Include(c => c.Loai).Include(c => c.Size).FirstOrDefault(s => s.Id == id);
            return View(sp);
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var p = da.SanPhams.Include(c => c.Size).FirstOrDefault(s => s.Id == id);
                da.SanPhams.Remove(p);
                da.SaveChanges();
                return RedirectToAction("ListBeverage");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ListDonHang(string? page)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("DangNhap");
            }
            string check = _httpContextAccessor.HttpContext.Session.GetString("UserName");
            if (check != null)
            {
                TempData["IsLoggedIn"] = true;
                ViewData["SuccessMessage"] = _httpContextAccessor.HttpContext.Session.GetString("SuccessMessage");

                // Lấy tên người dùng từ session
                var nameH = _httpContextAccessor.HttpContext.Session.GetString("HoQl");
                var name = _httpContextAccessor.HttpContext.Session.GetString("TenQl");

                if (!string.IsNullOrEmpty(nameH) && !string.IsNullOrEmpty(name))
                {
                    ViewData["FullName"] = $"{nameH} {name}";
                }
                else
                {
                    ViewData["FullName"] = "Quản Lý";
                }
                // Xóa thông báo từ session
                var ds = da.DonHangs.ToList();
                int pageSize = 4;
                if (!string.IsNullOrEmpty(page))
                {
                    TempData["IsLoggedIn"] = true;

                    int pageNumber = int.Parse(page);
                    TempData["IsLoggedIn"] = true;

                    ds = da.DonHangs
                              .OrderBy(item => item.Id)
                              .Skip((pageNumber - 1) * pageSize)//Lấy từ vị trí
                              .Take(pageSize)//Lấy bao nhiêu
                              .ToList();
                    TempData["IsLoggedIn"] = true;
                }
                return View(ds);
            }
            else
                return RedirectToAction("DangNhap");
        }

        public async Task<IActionResult> DonHangDetails(int? id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("DangNhap");
            }
            TempData["IsLoggedIn"] = true;

            if (id == null)
            {
                return NotFound();
            }

            var donHang = await da.DonHangs
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.ChiTietDonHangs)
                    .ThenInclude(ctdh => ctdh.SanPham)
                .Include(dh => dh.ChiTietDonHangs)
                    .ThenInclude(ctdh => ctdh.KhuyenMai)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (donHang == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu KhachHang là null
            if (donHang.KhachHang == null)
            {
                // Xử lý khi KhachHang không có dữ liệu
                // Ví dụ: bạn có thể hiển thị thông báo lỗi hoặc chuyển hướng đến trang khác
                return RedirectToAction("ListDonHang"); // hoặc một hành động khác tùy thuộc vào logic của bạn
            }

            var viewModel = new DonHangDetailsViewModel
            {
                DonHangId = donHang.Id,
                NgayTao = donHang.NgayTao,
                PayPalKey = donHang.PayPalKey,
                KhachHang = donHang.KhachHang,
                ChiTietDonHangs = donHang.ChiTietDonHangs.ToList()
            };

            return View(viewModel);
        }

        // GET: CaPheController/Delete/5
        public ActionResult DeleteDonHang(int id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("DangNhap");
            }
            TempData["IsLoggedIn"] = true;
            var p = da.DonHangs.FirstOrDefault(s => s.Id == id);
            return View(p);
        }


        // POST: CaPheController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDonHang(int id, IFormCollection collection)
        {
            try
            {
                if (!IsAdminLoggedIn())
                {
                    return RedirectToAction("DangNhap");
                }
                TempData["IsLoggedIn"] = true;

                // Xóa tất cả các ChiTietDonHang liên quan đến DonHang
                var chiTietDonHangs = da.ChiTietDonHangs.Where(ct => ct.DonHangId == id);
                da.ChiTietDonHangs.RemoveRange(chiTietDonHangs);
                da.SaveChanges();

                // Sau đó, xóa DonHang
                var dh = da.DonHangs.FirstOrDefault(s => s.Id == id);
                da.DonHangs.Remove(dh);
                da.SaveChanges();

                return RedirectToAction("ListDonHang");
            }
            catch
            {
                return View();
            }
        }
        

    }
}

using Fast_food.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Fast_food.Controllers
{
    public class HomeController : Controller
    {
        Fast_foodContext context = new Fast_foodContext();

        public IActionResult Index()
        {
            var items = context.SanPhams.ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult GetProductName(int id)
        {
            // Lấy tên sản phẩm từ cơ sở dữ liệu dựa trên id
            var productName = context.LoaiSanPhams.SingleOrDefault(e => e.IdLoai == id)?.TenLoai;

            if (string.IsNullOrEmpty(productName))
            {
                return NotFound();
            }

            return Ok(productName);
        }
        [HttpPost]
        public IActionResult GetProductNames(int[] productIds)
        {
            // Lấy danh sách tên sản phẩm từ cơ sở dữ liệu dựa trên danh sách ID sản phẩm
            var productNames = context.LoaiSanPhams
                .Where(p => productIds.Contains(p.IdLoai))
                .Select(p => new { p.IdLoai, p.TenLoai })
                .ToDictionary(p => p.IdLoai, p => p.TenLoai);

            return Ok(productNames);
        }
        [HttpGet]
        public IActionResult Single(int id)
        {
            var item = context.SanPhams.SingleOrDefault(i => i.IdSp == id);
            return View(item);
        }
        public static string GetMD5(string str)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }

            return sbHash.ToString();

        }

        //Đăng ký đăng nhập
        [HttpGet]
        public IActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangNhap(KhachHang model)
        {
            if (ModelState.IsValid)
            {
                var pass = GetMD5(model.Pass);
                var data = context.KhachHangs.Where(s => s.UseName.Equals(model.UseName) && s.Pass.Equals(pass)).FirstOrDefault();
                var getid = from kh in context.KhachHangs where kh.UseName == model.UseName select kh.IdKh;
                if (data != null)
                {
                    HttpContext.Session.SetInt32("Id", getid.FirstOrDefault());
                    HttpContext.Session.SetString("UseName", model.UseName);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["Error"] = "<script>window.onload = function () {alert('Thông tin tài khoản không chính xác');}</script>";
                    return View();
                }
            }
            else
            {
                TempData["Message"] = "<script>window.onload = function () {alert('Vui lòng nhập thông tin');}</script>";
                return View();
            }
        }
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangKy(KhachHang model)
        {
            if (Check(model.UseName))
            {
                TempData["Error"] = "<script>window.onload = function () {alert('Tài khoản đã tồn tại !!!');}</script>";
                return RedirectToAction("DangNhap","Home");
            }
            else
            {
                KhachHang kh = new KhachHang();
                kh.UseName = model.UseName;
                kh.Pass = GetMD5(model.Pass);
                context.KhachHangs.Add(kh);
                context.SaveChanges();
                TempData["Sucsses"] = "<script>window.onload = function () {alert('Đăng ký tài khoản thành công');}</script>";
                return RedirectToAction("DangNhap", "Home");

            }
        }
        [HttpGet]
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DangNhap","Home");
        }
        [NonAction]
        public bool Check(string taikhoan)
        {
            var get = context.KhachHangs.SingleOrDefault(i => i.UseName == taikhoan);
            if (get != null) // Kiểm tra nếu tìm thấy tài khoản trong cơ sở dữ liệu
            {
                return true; // Tài khoản tồn tại
            }
            else
            {
                return false; // Tài khoản không tồn tại
            }
        }
        [HttpGet]
        public IActionResult DanhMuc()
        {
            var items = from sp in context.SanPhams where sp.TrangThai ==true select sp;
            ViewBag.items = items;
            ViewBag.Count = items.Count();
            return View(items);
        }
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public IActionResult TimKiem(string search)
        {
            var result = context.SanPhams
            .Where(p => p.TenSp.Contains(search))
            .ToList();
            return Json(result);
            // Xử lý tìm kiếm ở đây và trả về kết quả dưới dạng JSON.
        }
    }
}
using Fast_food.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;

namespace Fast_food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        Fast_foodContext context = new Fast_foodContext();
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
        public IActionResult Index()
        {
            var doanhthu = from dt in context.ChiTietDonHangs select dt.TongTien;
            decimal tongdoanhthu=doanhthu.Sum();
            ViewBag.tongdoanhthu=tongdoanhthu;
            var donhang = context.DonHangs.Count();
            ViewBag.donhang = donhang;
            var sanpham = context.SanPhams.Count();
            ViewBag.sanpham = sanpham;
            var khachhang =context.KhachHangs.Count();
            ViewBag.khachhang = khachhang;
            var danggiao = from dh in context.DonHangs where dh.TrangThai == false select dh;
            ViewBag.danggiao= danggiao.Count();
            var chuaduyet = from dh in context.DonHangs where dh.TrangThai == null select dh;
            ViewBag.chuaduyet = chuaduyet.Count();
            var dagiaothanhcong= from dh in context.DonHangs where dh.TrangThai == true select dh;
            ViewBag.dagiaothanhcong = dagiaothanhcong.Count();
            var nhanvien = context.TaiKhoans.Count();
            ViewBag.nhanvien = nhanvien;
            return View();
        }
        [HttpGet]
        public JsonResult MyProfile(int id)
        {
            var nhanvien = context.TaiKhoans.Find(id);
            return Json(nhanvien);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(TaiKhoan model)
        {
            if (ModelState.IsValid)
            {
                var pass = GetMD5(model.Pass);
                var data = context.TaiKhoans.Where(s => s.UseName.Equals(model.UseName) && s.Pass.Equals(pass)).FirstOrDefault();
                var getid = from tk in context.TaiKhoans where tk.UseName == model.UseName select tk.IdTk;
                var id = (from a in context.TaiKhoans where a.UseName == model.UseName select a.IdTk).SingleOrDefault();
                if (data != null)
                {
                    int i = id != null ? Convert.ToInt32(id) : 0; // Use 0 as default value if id is null
                    var tk = context.TaiKhoans.SingleOrDefault(a => a.IdTk == i);
                    HttpContext.Session.SetInt32("IdTK", getid.FirstOrDefault());
                    HttpContext.Session.SetString("HinhAnh",tk.HinhAnh);
                    var cv = (from c in context.TaiKhoans where c.UseName == model.UseName select c.ChucVu).SingleOrDefault();
                    if(cv == true){
                        HttpContext.Session.SetInt32("ChucVu", 1);
                    }
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    TempData["Error"] = "<script>window.onload = function () {alert('Thông tin tài khoản không chính xác');}</script>";
                    return View();
                }
            }
                return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}

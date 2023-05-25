using Fast_food.Helper;
using Fast_food.Models;
using Fast_food.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Fast_food.Controllers
{
    public class GioHangController : Controller
    {
        Fast_foodContext context = new Fast_foodContext();
        public List<GioHangItems> GioHangs
        {
            get
            {
                var data = HttpContext.Session.Get<List<GioHangItems>>("GioHang");
                if (data == null)
                {
                    data = new List<GioHangItems>();
                }
                return data;
            }
        }
        public IActionResult Index()
        {
            var myCart = GioHangs;
            var get = HttpContext.Session.GetString("Id");
            if (get == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            else
            {
                int sl = GioHangs.Count();
                var tt = GioHangs.Sum(s => s.Gia * s.SoLuong);
                ViewBag.tt = tt;
                ViewBag.sl = sl;
                HttpContext.Session.SetInt32("SoluongGioHang", sl);
                HttpContext.Session.Set("GioHang", myCart);
                return View(GioHangs);
            }
        }
        [HttpPost]
        public IActionResult AddToCart(int id, int SoLuong,int size)
        {
            var myCart = GioHangs;
            var item = myCart.SingleOrDefault(p => p.IdSp == id);
            if (item == null)//chưa có
            {
                var sp = context.SanPhams.SingleOrDefault(p => p.IdSp == id);
                item = new GioHangItems
                {
                    IdSp = id,
                    TenSp = sp.TenSp,
                    HinhAnh = sp.HinhAnh,
                    Gia = sp.Gia,
                    SoLuong = SoLuong,
                    Size = size
                };
                myCart.Add(item);
            }
            else
            {
                item.SoLuong += SoLuong;
            }
            int sl = GioHangs.Count();
            HttpContext.Session.SetInt32("SoluongGioHang", sl);
            HttpContext.Session.Set("GioHang", myCart);
            return RedirectToAction("Index", "GioHang");
        }
        [HttpGet]
        public IActionResult DeleteToCart(int id)
        {
            var myCart = GioHangs;
            var item = myCart.Where(m => m.IdSp == id).FirstOrDefault();
            if (item != null)//chưa có
            {
                myCart.Remove(item);
            }
            int sl = GioHangs.Count();
            HttpContext.Session.SetInt32("SoluongGioHang", sl);
            HttpContext.Session.Set("GioHang", myCart);
            return RedirectToAction("Index", "GioHang");
        }
        [HttpGet]
        public IActionResult ThanhToan()
        {
            var myCart = GioHangs;
            var idkh = HttpContext.Session.GetInt32("Id");
            var getkh = context.KhachHangs.Where(kh => kh.IdKh == idkh).SingleOrDefault();
            int sl = GioHangs.Count();
            var tt = GioHangs.Sum(s => s.Gia * s.SoLuong);
            ViewBag.tt = tt;
            ViewBag.sl = sl;
            ViewBag.get = getkh;
            ViewBag.id = idkh;
            return View(myCart);
        }
        [HttpPost]
        public IActionResult ThanhToan(DonHang model)
        {
            var myCart = GioHangs;
            var tt = GioHangs.Sum(s => s.Gia * s.SoLuong);
            var idkh = HttpContext.Session.GetInt32("Id");
            var getiddh = from iddh in context.DonHangs where iddh.IdKh == idkh select iddh.IdDh;
            List<GioHangItems> cartItems = HttpContext.Session.Get<List<GioHangItems>>("GioHang");
            DonHang dh = new DonHang();
            dh.IdKh = idkh;
            dh.HoTen = model.HoTen;
            dh.DiaChi = model.DiaChi;
            dh.Sdt = model.Sdt;
            dh.HinhThuc = model.HinhThuc;
            dh.TongTien = tt;
            dh.NgayTao = DateTime.Now;
            context.DonHangs.Add(dh);
            context.SaveChanges();
            var getid = context.DonHangs.ToList().LastOrDefault(dh);
            foreach (var item in cartItems)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.IdDh = getid.IdDh;
                ctdh.IdSp = item.IdSp;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.Gia;
                ctdh.TongTien = tt;
                ctdh.Size = item.Size;
                context.ChiTietDonHangs.Add(ctdh);
                context.SaveChanges();
            }
            if (myCart.Count() > 0)
            {
                myCart.Clear();
            }
            int sl = myCart.Count();
            HttpContext.Session.SetInt32("Soluong", sl);
            HttpContext.Session.Set("GioHang", myCart);
            return RedirectToAction("ThankYou", "GioHang");
        }
        [HttpGet]
        public IActionResult ThankYou()
        {
            var myCart = GioHangs;
                int sl = GioHangs.Count();
                HttpContext.Session.SetInt32("SoluongGioHang", sl);
                HttpContext.Session.Set("GioHang", myCart);
                return View();
        }
    }
}

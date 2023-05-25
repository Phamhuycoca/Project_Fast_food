using Fast_food.Models;
using Fast_food.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using X.PagedList;
using System.Security.Cryptography;
namespace Fast_food.Controllers
{
    public class KhachHangController : Controller
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
        //Lấy đường dẫn file hình ảnh
        public string Upload(int id, IFormFile file)
        {
            //string id=idsp.ToString();
            if (file != null)
            {
                string path = Path.GetFullPath("./wwwroot/KhachHang");
                Directory.CreateDirectory(path + "/" + id);

                //Copy ảnh vào thư mục
                using var stream = new FileStream(string.Format(@"{0}/{1}/{2}",
                    path, id, file.FileName), FileMode.Create);
                file.CopyTo(stream);

                //Trả về đường dẫn ảnh
                return id + "/" + file.FileName;
            }
            return string.Empty;
        }
        public IActionResult Index(int? page)
        {
            var id = HttpContext.Session.GetInt32("Id");
            var info = context.KhachHangs.SingleOrDefault(i => i.IdKh == id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(info);
        }
        public IActionResult Update(KhachHang model, IFormFile file)
        {
            var id = HttpContext.Session.GetInt32("Id");
            var info = context.KhachHangs.SingleOrDefault(i => i.IdKh == id);

            info.HoTen = model.HoTen;
            info.GioiTinh = model.GioiTinh;
            if (file != null)
            {
                string path1 = Path.GetFullPath("./wwwroot/KhachHang/" + id);
                if (path1 != null)
                {
                    string filename = Upload(info.IdKh, file);
                    if (path1 != filename)
                    {
                        Directory.Delete(path1, true);
                        info.HinhAnh = filename;
                    }

                }
                string path = Path.GetFullPath("./wwwroot/KhachHang");
                Directory.CreateDirectory(path + "/" + info.IdKh);

                //Copy ảnh vào thư mục
                using (var stream = new FileStream(string.Format(@"{0}/{1}/{2}",
                    path, info.IdKh, file.FileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                info.HinhAnh = info.IdKh + "/" + file.FileName;
            }
            info.DiaChi = model.DiaChi;
            info.Sdt = model.Sdt;
            context.KhachHangs.Update(info);
            context.SaveChanges();
            TempData["Bell"] = "<script>window.onload = function () {alert('Thay đổi thông tin thành công');}</script>";
            return RedirectToAction("Index", "KhachHang");
        }
        //Thong tin don hang
        public IActionResult DonHang()
        {
            var id = HttpContext.Session.GetInt32("Id");
            if (id == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            else
            {
                var items = from dh in context.DonHangs
                            where dh.IdKh == id
                            select new DonHangItem
                            {
                                IdDh = dh.IdDh,
                                HoTen = dh.HoTen,
                                DiaChi = dh.DiaChi,
                                Sdt = dh.Sdt,
                                NgayTao = dh.NgayTao,
                                TongTien = dh.TongTien,
                                HinhThuc = dh.HinhThuc,
                                TrangThai = dh.TrangThai,
                            };
                var IDDH = from dh in context.DonHangs where dh.IdKh == id select dh.IdDh;
                var CTDH = from dh in IDDH
                           join
                           ct in context.ChiTietDonHangs on dh equals ct.IdDh
                           join sp in context.SanPhams on ct.IdSp equals sp.IdSp
                           select new SanPhamDonHang
                           {
                               IdSp = ct.IdSp,
                               HinhAnh = sp.HinhAnh,
                               Size = ct.Size,
                               SoLuong = ct.SoLuong,
                               DonGia = ct.DonGia,
                               TongTien = ct.TongTien
                           };
                ViewBag.items = items;
                ViewBag.Count = items.Count();
                return View(items);
            }
            //return Ok(CTDH);
        }
        [HttpGet]
        public IActionResult GetProductList(int id)
        {
            var IDDH = from dh in context.DonHangs where dh.IdDh == id select dh.IdDh;
            var CTDH = from dh in IDDH
                       join
                       ct in context.ChiTietDonHangs on dh equals ct.IdDh
                       join sp in context.SanPhams on ct.IdSp equals sp.IdSp
                       select new SanPhamDonHang
                       {
                           TenSp = sp.TenSp,
                           IdSp = ct.IdSp,
                           HinhAnh = sp.HinhAnh,
                           Size = ct.Size,
                           SoLuong = ct.SoLuong,
                           DonGia = ct.DonGia,
                           TongTien = ct.TongTien
                       };
            return Json(CTDH);
        }
        [HttpPost]
        public IActionResult GetData(int id)
        {
            var t = context.DonHangs.SingleOrDefault(i => i.IdDh == id);
            t.TrangThai = true;
            context.DonHangs.Update(t);
            context.SaveChanges();
            return Ok(t);
        }
        private readonly List<DonHang> donHangs;
        private readonly List<ChiTietDonHang> chiTietDons;


        [HttpPost]
        public IActionResult DeleteData(int id)
        {
            var cts = context.ChiTietDonHangs.Where(ct => ct.IdDh == id).ToList();
            var dh = context.DonHangs.FirstOrDefault(d => d.IdDh == id);

            if (dh == null || cts == null)
            {
                return NotFound();
            }

            foreach (var ct in cts)
            {
                context.ChiTietDonHangs.Remove(ct);
            }

            context.DonHangs.Remove(dh);
            context.SaveChanges();

            return Json(dh);
        }
        [HttpPost]
        public IActionResult UpdatePass(string Pass1, string Pass2)
        {
            var id = HttpContext.Session.GetInt32("Id");
            var info = context.KhachHangs.SingleOrDefault(i => i.IdKh == id);
            if (info == null)
            {
                NotFound();
            }
            if (Pass1 != Pass2)
            {
                TempData["Error"] = "<script>window.onload = function () {alert('Mật khẩu không khớp');}</script>";
            }
            else
            {
                info.Pass = GetMD5(Pass1);
                context.KhachHangs.Update(info);
                context.SaveChanges();
                TempData["Sucsses"] = "<script>window.onload = function () {alert('Thay đổi mật khẩu thành công');}</script>";

            }
            return RedirectToAction("Index", "KhachHang");
        }
        public ActionResult LoadData(int page)
        {
            var id = HttpContext.Session.GetInt32("Id");

            var data = from dh in context.DonHangs
                        where dh.IdKh == id
                        select new DonHangItem
                        {
                            IdDh = dh.IdDh,
                            HoTen = dh.HoTen,
                            DiaChi = dh.DiaChi,
                            Sdt = dh.Sdt,
                            NgayTao = dh.NgayTao,
                            TongTien = dh.TongTien,
                            HinhThuc = dh.HinhThuc,
                            TrangThai = dh.TrangThai,
                        };
            return Json(data);
        }
    }
}

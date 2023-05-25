using Fast_food.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Fast_food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhanVienController : Controller
    {
        Fast_foodContext context = new Fast_foodContext();
        public IActionResult Index()
        {
            ViewBag.items = context.TaiKhoans.ToList();
            return View();
        }
        //Lấy đường dẫn file hình ảnh
        public string Upload(int id, IFormFile file)
        {
            //string id=idsp.ToString();
            if (file != null)
            {
                string path = Path.GetFullPath("./wwwroot/NhanVien");
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
        [HttpPost]
        public IActionResult Create(TaiKhoan model,IFormFile file)
        {
            TaiKhoan nv = new TaiKhoan();
            nv.HoTen = model.HoTen;
            nv.GioiTinh = model.GioiTinh;
            nv.Sdt = model.Sdt;
            nv.NgaySinh = model.NgaySinh;
            nv.DiaChi = model.DiaChi;
            nv.ChucVu = model.ChucVu;
            nv.UseName = model.UseName;
            nv.Pass = GetMD5(model.Pass);
            context.TaiKhoans.Add(nv);
            context.SaveChanges();
            string filename = Upload(nv.IdTk, file);
            nv.HinhAnh = filename;
            context.TaiKhoans.Update(nv);
            context.SaveChanges();
            return RedirectToAction("Index","NhanVien");
        }
        [HttpGet]
        public IActionResult GetById(int id)
         {
            var nhanvien = context.TaiKhoans.Find(id);
            return Json(nhanvien);
        }
        [HttpPost]
        public IActionResult Edit(TaiKhoan model,IFormFile file)
        {
            var tk = context.TaiKhoans.Find(model.IdTk);
            var getid = context.TaiKhoans.Find(model.IdTk).IdTk;
            if (tk == null)
            {
                return NotFound();
            }
            tk.HoTen = model.HoTen;
            if (file != null)
            {
                string path1 = Path.GetFullPath("./wwwroot/NhanVien/" + getid);
                if (path1 != null)
                {
                    Directory.Delete(path1, true);

                }
                string path = Path.GetFullPath("./wwwroot/NhanVien");
                Directory.CreateDirectory(path + "/" + tk.IdTk);

                //Copy ảnh vào thư mục
                using (var stream = new FileStream(string.Format(@"{0}/{1}/{2}",
                    path, tk.IdTk, file.FileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                tk.HinhAnh = tk.IdTk + "/" + file.FileName;
            }
            tk.GioiTinh = model.GioiTinh;
            tk.Sdt = model.Sdt;
            tk.NgaySinh = model.NgaySinh;
            tk.DiaChi = model.DiaChi;
            tk.ChucVu = model.ChucVu;
            context.TaiKhoans.Update(tk);
            context.SaveChanges();
            return RedirectToAction("Index", "NhanVien");
        }
        //Xóa thông tin sản phẩm
        [HttpPost]
        public IActionResult DeleteData(int id)
        {
            var i = context.TaiKhoans.SingleOrDefault(i => i.IdTk == id);
            if (i != null)
            {
                NotFound();
            }
            var img = i.HinhAnh;
            string path = Path.GetFullPath("./wwwroot/NhanVien/" + id);
            if (img == "" || img == null)
            {
                context.TaiKhoans.Remove(i);
                context.SaveChanges();
            }
            else
            {
                Directory.Delete(path, true);
                context.TaiKhoans.Remove(i);
                context.SaveChanges();
            }
            return Json(i);
        }
    }
}

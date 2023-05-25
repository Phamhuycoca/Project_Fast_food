using Fast_food.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using X.PagedList;
namespace Fast_food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        Fast_foodContext context = new Fast_foodContext();
        //Lấy đường dẫn file hình ảnh
        public string Upload(int id, IFormFile file)
        {
            //string id=idsp.ToString();
            if (file != null)
            {
                string path = Path.GetFullPath("./wwwroot/SanPham");
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
        //Select danh sách loại sản phẩm vào dropdown
        private List<SelectListItem> get()
        {
            var dslsp = new List<SelectListItem>();
            List<LoaiSanPham> lsp = context.LoaiSanPhams.ToList();
            dslsp = lsp.Select(dssp => new SelectListItem()
            {
                Value = dssp.IdLoai.ToString(),
                Text = dssp.TenLoai

            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chon loai san pham----"
            };
            dslsp.Insert(0, defItem);
            return dslsp;
        }
        [HttpGet]
        public JsonResult GetLoaiSanPham()
        {
            var dslsp = new List<SelectListItem>();
            List<LoaiSanPham> lsp = context.LoaiSanPhams.ToList();
            dslsp = lsp.Select(dssp => new SelectListItem()
            {
                Value = dssp.IdLoai.ToString(),
                Text = dssp.TenLoai
            }).ToList();
            var defItem = new SelectListItem()
            {
                /*Value = "",
                Text = "----Chon loai san pham----"*/
            };
            dslsp.Insert(0, defItem);
            return Json(dslsp);
        }
        [HttpGet]
        //Danh sách sản phẩm
        public IActionResult Index()
        {
            List<LoaiSanPham> lsp = context.LoaiSanPhams.ToList();

            // Tạo SelectList
            SelectList lspList = new SelectList(lsp, "IdLoai", "TenLoai");
            ViewBag.dslsp = lspList;
            var item = context.SanPhams.ToList();
            ViewBag.items = item;
            return View();
        }
        [HttpPost]
        public IActionResult Create(SanPham model,IFormFile file)
        {

            if (model.TenSp==null || model.Gia==0)
            {
                TempData["Message"] = "<script>window.onload = function () {alert('Vui lòng nhập thông tin');}</script>";
                return RedirectToAction("Index", "SanPham");
            }
            else
            {
            SanPham sp = new SanPham();
            sp.TenSp = model.TenSp;
            sp.Gia = model.Gia;
            sp.TrangThai = model.TrangThai;
            sp.IdLoai = model.IdLoai;
            context.SanPhams.Add(sp);
            context.SaveChanges();
            string filename = Upload(sp.IdSp, file);
            sp.HinhAnh = filename;
            context.SanPhams.Update(sp);
            context.SaveChanges();
            TempData["Bell"] = "<script>window.onload = function () {alert('Thêm thành công sản phẩm');}</script>";
            return RedirectToAction("Index", "SanPham");
            }
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
        //Xóa thông tin sản phẩm
        [HttpPost]
        public IActionResult DeleteData(int id)
        {
            var i = context.SanPhams.SingleOrDefault(i => i.IdSp == id);
            if (i != null)
            {
                NotFound();
            }
            var img = i.HinhAnh;
            string path = Path.GetFullPath("./wwwroot/SanPham/" +id);
            if (img == ""||img==null)
            {
                context.SanPhams.Remove(i);
                context.SaveChanges();
            }
            else
            {
                Directory.Delete(path, true);
                context.SanPhams.Remove(i);
            context.SaveChanges();
            }
            return Json(i);
        }
        //Lấy thông tin để chỉnh sửa
        [HttpGet]
        public IActionResult GetData(int id)
        {
            // Lấy dữ liệu cần sửa đổi từ cơ sở dữ liệu
            var data = context.SanPhams.Find(id);
            return Json(data);
        }
        //Edit thông tin
        [HttpPost]
        public IActionResult EditData(SanPham model, IFormFile file)
        {
            var sp = context.SanPhams.Find(model.IdSp);
            var getid = context.SanPhams.Find(model.IdSp).IdSp;
            if (sp == null)
            {
                return NotFound();
            }
            sp.TenSp = model.TenSp;
            if (file != null)
            {
                string path1 = Path.GetFullPath("./wwwroot/SanPham/" + getid);
                if (path1 != null)
                {
                    Directory.Delete(path1, true);

                }
                string path = Path.GetFullPath("./wwwroot/SanPham");
                Directory.CreateDirectory(path + "/" + sp.IdSp);

                //Copy ảnh vào thư mục
                using (var stream = new FileStream(string.Format(@"{0}/{1}/{2}",
                    path, sp.IdSp, file.FileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                sp.HinhAnh = sp.IdSp + "/" + file.FileName;
            }
            sp.Gia = model.Gia;
            sp.IdLoai = model.IdLoai;
            sp.TrangThai = model.TrangThai;
            context.SanPhams.Update(sp);
            context.SaveChanges();
            return RedirectToAction("Index","SanPham");
        }
     
    }
}

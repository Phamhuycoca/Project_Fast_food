using Fast_food.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fast_food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoaiSanPhamController : Controller
    {
        Fast_foodContext context = new Fast_foodContext();
        [HttpGet]
        public IActionResult Index()
        {
            var item = context.LoaiSanPhams.ToList();
           
            return View(item);
        }
        [HttpPost]
        public IActionResult Create(LoaiSanPham model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "<script>window.onload = function () {alert('Vui lòng nhập thông tin');}</script>";
                return RedirectToAction("Index","LoaiSanPham");
            }
            else
            {
                context.LoaiSanPhams.Add(model);
                context.SaveChanges();
                return RedirectToAction("Index", "LoaiSanPham","Admin");
            }
        }
        [HttpGet]
        public IActionResult GetData(int id)
        {
            // Lấy dữ liệu cần sửa đổi từ cơ sở dữ liệu
            var data = context.LoaiSanPhams.Find(id);
            return Json(data);
        }
        [HttpPost]
        public IActionResult EditData([FromBody]LoaiSanPham model)
        {
            try
            {
                // Sửa đổi dữ liệu trong cơ sở dữ liệu
                context.LoaiSanPhams.Update(model);
                context.SaveChanges();
                // Trả về kết quả thành công và ID của dữ liệu được sửa đổi
                return Json(new { success = true, idloai = model.IdLoai });
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult DeleteData(int id)
        {
            var i = context.LoaiSanPhams.SingleOrDefault(i => i.IdLoai == id);
            if (i != null)
            {
                NotFound();
            }
            context.LoaiSanPhams.Remove(i);
            context.SaveChanges();
            return Json(i);
        }
    }
}

using Fast_food.Models;
using Fast_food.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Fast_food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DonHangController : Controller
    {
        Fast_foodContext context = new Fast_foodContext();
        public IActionResult Index()
        {
            var items = context.DonHangs.ToList();
            return View(items);
        }
        [HttpPost]
        public IActionResult Update(bool status)
        {
            var t = status;
            return Ok(t);
        }
        [HttpPost]
        public IActionResult GetData(int id)
        {
            var t = context.DonHangs.SingleOrDefault(i=>i.IdDh==id);
            t.TrangThai = false;
            context.DonHangs.Update(t);
            context.SaveChanges();
            return Ok(t);
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

    }
}

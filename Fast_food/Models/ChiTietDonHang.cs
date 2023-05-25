using System;
using System.Collections.Generic;

namespace Fast_food.Models
{
    public partial class ChiTietDonHang
    {
        public int IdCtdh { get; set; }
        public int IdDh { get; set; }
        public int IdSp { get; set; }
        public int Size { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public int TongTien { get; set; }

        public virtual DonHang IdDhNavigation { get; set; } = null!;
        public virtual SanPham IdSpNavigation { get; set; } = null!;
    }
}

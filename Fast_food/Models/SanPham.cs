using System;
using System.Collections.Generic;

namespace Fast_food.Models
{
    public partial class SanPham
    {
        public SanPham()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        public int IdSp { get; set; }
        public string TenSp { get; set; } = null!;
        public string? HinhAnh { get; set; }
        public int IdLoai { get; set; }
        public int Gia { get; set; }
        public bool TrangThai { get; set; }

        public virtual LoaiSanPham IdLoaiNavigation { get; set; } = null!;
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}

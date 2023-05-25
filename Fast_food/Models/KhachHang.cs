using System;
using System.Collections.Generic;

namespace Fast_food.Models
{
    public partial class KhachHang
    {
        public KhachHang()
        {
            DonHangs = new HashSet<DonHang>();
        }

        public int IdKh { get; set; }
        public string? HoTen { get; set; }
        public string? HinhAnh { get; set; }
        public string? DiaChi { get; set; }
        public bool? GioiTinh { get; set; }
        public string? Sdt { get; set; }
        public string UseName { get; set; } = null!;
        public string Pass { get; set; } = null!;

        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Fast_food.Models
{
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        public int IdDh { get; set; }
        public int? IdKh { get; set; }
        public string HoTen { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public string Sdt { get; set; } = null!;
        public DateTime NgayTao { get; set; }
        public int TongTien { get; set; }
        public bool HinhThuc { get; set; }
        public bool? TrangThai { get; set; }

        public virtual KhachHang? IdKhNavigation { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}

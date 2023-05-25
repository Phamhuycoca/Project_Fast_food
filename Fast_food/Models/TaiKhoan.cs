using System;
using System.Collections.Generic;

namespace Fast_food.Models
{
    public partial class TaiKhoan
    {
        public int IdTk { get; set; }
        public string? HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public bool? GioiTinh { get; set; }
        public string? DiaChi { get; set; }
        public string? Sdt { get; set; }
        public string? HinhAnh { get; set; }
        public string UseName { get; set; } = null!;
        public string? Pass { get; set; }
        public bool? ChucVu { get; set; }
    }
}

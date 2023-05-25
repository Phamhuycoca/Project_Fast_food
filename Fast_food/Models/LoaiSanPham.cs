using System;
using System.Collections.Generic;

namespace Fast_food.Models
{
    public partial class LoaiSanPham
    {
        public LoaiSanPham()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int IdLoai { get; set; }
        public string TenLoai { get; set; } = null!;
        public string? MoTa { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}

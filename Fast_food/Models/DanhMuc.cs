using System;
using System.Collections.Generic;

namespace Fast_food.Models
{
    public partial class DanhMuc
    {
        public int IdDm { get; set; }
        public string TenDm { get; set; } = null!;
        public int? IdSp { get; set; }

        public virtual SanPham? IdSpNavigation { get; set; }
    }
}

namespace Fast_food.Models.ViewModel
{
    public class SanPhamDonHang
    {
        public int IdCtdh { get; set; }
        public int? IdDh { get; set; }
        public int? IdSp { get; set; }
        public string TenSp { get; set; } = null!;
        public string? HinhAnh { get; set; }
        public int Size { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public int TongTien { get; set; }
    }
}

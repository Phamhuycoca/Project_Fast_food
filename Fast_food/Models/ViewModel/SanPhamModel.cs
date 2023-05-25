namespace Fast_food.Models.ViewModel
{
    public class SanPhamModel
    {
        public int IdSp { get; set; }
        public string TenSp { get; set; } = null!;
        public string? HinhAnh { get; set; }
        public int IdLoai { get; set; }
        public int Gia { get; set; }
        public bool TrangThai { get; set; }
    }
}

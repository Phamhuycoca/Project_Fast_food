namespace Fast_food.Models.ViewModel
{
    public class DonHangItem
    {
        public int IdDh { get; set; }
        public int? IdKh { get; set; }
        public string HoTen { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public string Sdt { get; set; } = null!;
        public DateTime NgayTao { get; set; }
        public int TongTien { get; set; }
        public bool HinhThuc { get; set; }
        public bool? TrangThai { get; set; }
    }
}

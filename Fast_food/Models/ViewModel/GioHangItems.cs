namespace Fast_food.Models.ViewModel
{
    public class GioHangItems
    {
        
        public int IdSp { get; set; }
        public string? TenSp { get; set; }
        public string? HinhAnh { get; set; }
        public int Gia { get; set; }
        public int SoLuong { get; set; }
        public int Size { get; set; }
        public int TongTien => SoLuong * Gia;
    }
}

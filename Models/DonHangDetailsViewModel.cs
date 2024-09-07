using System;
using System.Collections.Generic;
using PPKBeverageManagement.Models;

namespace PPKBeverageManagement.ViewModels
{
    public class DonHangDetailsViewModel
    {
        public int DonHangId { get; set; }
        public DateTime? NgayTao { get; set; }
        public string PayPalKey { get; set; }
        public KhachHang KhachHang { get; set; }
        public List<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace PPKBeverageManagement.Models
{
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        public int Id { get; set; }
        public int? KhachHangId { get; set; }
        public DateTime? NgayTao { get; set; }
        public string PayPalKey { get; set; }
        public int? TaiXeId { get; set; }
        public int? Idtx { get; set; }
        public int? Txid { get; set; }
        public int? Idtaixexe { get; set; }

        public virtual KhachHang KhachHang { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}

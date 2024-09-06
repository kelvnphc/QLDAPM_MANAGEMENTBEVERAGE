using System;
using System.Collections.Generic;

#nullable disable

namespace PPKBeverageManagement.Models
{
    public partial class LoaiSanPham
    {
        public LoaiSanPham()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int LoaiId { get; set; }
        public string TenLoai { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}

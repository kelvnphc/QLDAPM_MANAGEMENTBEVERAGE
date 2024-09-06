﻿using System;
using System.Collections.Generic;

#nullable disable

namespace PPKBeverageManagement.Models
{
    public partial class SanPham
    {
        public SanPham()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        public int Id { get; set; }
        public string Ten { get; set; }
        public string MieuTa { get; set; }
        public string Anh { get; set; }
        public int? SizeId { get; set; }
        public decimal? Tien { get; set; }
        public int? LoaiId { get; set; }

        public virtual LoaiSanPham Loai { get; set; }
        public virtual Size Size { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}

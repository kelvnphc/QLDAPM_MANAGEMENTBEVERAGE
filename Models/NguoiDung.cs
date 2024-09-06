using System;
using System.Collections.Generic;

#nullable disable

namespace PPKBeverageManagement.Models
{
    public partial class NguoiDung
    {
        public int Id { get; set; }
        public string HoNv { get; set; }
        public string TenNv { get; set; }
        public string QueQuan { get; set; }
        public string DiaChi { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

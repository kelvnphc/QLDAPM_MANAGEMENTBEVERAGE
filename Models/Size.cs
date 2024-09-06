using System;
using System.Collections.Generic;

#nullable disable

namespace PPKBeverageManagement.Models
{
    public partial class Size
    {
        public Size()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int Id { get; set; }
        public string Ten { get; set; }
        public string DungTich { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}

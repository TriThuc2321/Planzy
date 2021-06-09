using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.DoanhThuThangModel
{
    class DoanhThuThang
    {
        public string MaChuyenBay { get; set; }
        public DateTime NgayBay { get; set; }
        public string NgayBayString { get; set; }
        public int SoVe { get; set; }
        public int DoanhThu { get; set; }
        public float DoanhThuTrieuDong { get; set; }
        public float TyLe { get; set; }
    }
}

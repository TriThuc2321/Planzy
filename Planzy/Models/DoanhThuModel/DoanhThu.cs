using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.DoanhThuModel
{
    class DoanhThu
    {
        public string MaChuyenBay { get; set; }
        public DateTime NgayBay { get; set; }
        public string NgayBayString { get; set; }
        public int SoVe { get; set; }
        public int DoanhThuInt { get; set; }
        public float DoanhThuTrieuDong { get; set; }
        public float TyLe { get; set; }
        public DoanhThu()
        {
            DoanhThuInt = 0;
            DoanhThuTrieuDong = 0;
            TyLe = 0;
        }
    }
}

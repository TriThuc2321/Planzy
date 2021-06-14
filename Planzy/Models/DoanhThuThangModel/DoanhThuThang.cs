using Planzy.Models.ChuyenBayModel;
using Planzy.Models.DoanhThuModel;
using Planzy.Models.DoanhThuThangModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.DoanhThuThangModel
{
    class DoanhThuThang
    {
        public string Thang { get; set; }
        public string NgayBayString { get; set; }
        public int SoChuyenBay { get; set; }
        public int DoanhThu { get; set; }
        public float DoanhThuTrieuDong { get; set; }
        public float TyLe { get; set; }
        public DoanhThuServices doanhThuServices { get; set; }
        public DoanhThuThang(List<ChuyenBay> chuyenBays)
        {
            doanhThuServices = new DoanhThuServices(chuyenBays);
        }
    }
}

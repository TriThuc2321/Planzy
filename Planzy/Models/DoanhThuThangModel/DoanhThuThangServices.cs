using LiveCharts;
using Planzy.Models.ChiTietHangGheModel;
using Planzy.Models.ChuyenBayModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.DoanhThuThangModel
{
    class DoanhThuThangServices
    {
        public ChartValues<DoanhThuThang> doanhThuThangs
        {
            get; set;
        }
        public List<string> labels { get; set; }
        public int tongDoanhThu = 0;
        public float tongDoanhThuTrieuDong = 0;
        public DoanhThuThangServices(string Thang, string Nam, ChuyenBayServices chuyenBayServices)
        {
            doanhThuThangs = new ChartValues<DoanhThuThang>();
            labels = new List<string>();
            foreach (ChuyenBay chuyenBay in chuyenBayServices.GetAll())
            {
                if (chuyenBay.NgayBay.Year == Convert.ToInt32(Nam) && chuyenBay.NgayBay.Month == Convert.ToInt32(Thang))
                {
                    int tongSoVeDaBan = 0;
                    int doanhThu = 0;
                    foreach (ChiTietHangGhe chiTietHangGhe in chuyenBay.ChiTietHangGhesList)
                    {
                        doanhThu += (Convert.ToInt32(chiTietHangGhe.SoLuongGhe) - Convert.ToInt32(chiTietHangGhe.SoLuongGheConLai)) * Convert.ToInt32(chiTietHangGhe.TyLe) /100  * Convert.ToInt32(chuyenBay.GiaVeCoBan);
                        tongSoVeDaBan += (Convert.ToInt32(chiTietHangGhe.SoLuongGhe) - Convert.ToInt32(chiTietHangGhe.SoLuongGheConLai));
                    }
                    tongDoanhThu += doanhThu;
                    DoanhThuThang doanhThuThang = new DoanhThuThang();
                    doanhThuThang.MaChuyenBay = chuyenBay.MaChuyenBay;
                    doanhThuThang.SoVe = tongSoVeDaBan;
                    doanhThuThang.DoanhThu = doanhThu;
                    doanhThuThang.DoanhThuTrieuDong = (float)doanhThuThang.DoanhThu / 1000000;
                    doanhThuThang.NgayBay = chuyenBay.NgayBay;
                    doanhThuThang.NgayBayString = chuyenBay.NgayBay.ToShortDateString();
                    doanhThuThangs.Add(doanhThuThang);
                }    
            }
            tongDoanhThuTrieuDong = (float)tongDoanhThu / 1000000;
            
            for (int i = 0;i<doanhThuThangs.Count;i++)
            {
                doanhThuThangs[i].TyLe = doanhThuThangs[i].DoanhThuTrieuDong / tongDoanhThuTrieuDong * 100;
            }    

            var newList = doanhThuThangs.OrderBy(e => e.NgayBay.Day);
            doanhThuThangs = new ChartValues<DoanhThuThang>(newList);

            foreach (DoanhThuThang doanhThuThang in doanhThuThangs)
            {
                labels.Add(doanhThuThang.MaChuyenBay);
            }
        }
    }
}

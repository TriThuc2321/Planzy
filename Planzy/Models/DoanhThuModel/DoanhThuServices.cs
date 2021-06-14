using LiveCharts;
using Planzy.Models.ChiTietHangGheModel;
using Planzy.Models.ChuyenBayModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.DoanhThuModel
{
    class DoanhThuServices
    {
        public ChartValues<DoanhThu> doanhThus
        {
            get; set;
        }
        public List<string> labels { get; set; }
        public int tongDoanhThu = 0;
        public float tongDoanhThuTrieuDong = 0;
        public DoanhThuServices(List<ChuyenBay> chuyenBays)
        {
            doanhThus = new ChartValues<DoanhThu>();
            labels = new List<string>();
            foreach (ChuyenBay chuyenBay in chuyenBays)
            {
                if (true)
                {
                    int tongSoVeDaBan = 0;
                    int doanhThu = 0;
                    foreach (ChiTietHangGhe chiTietHangGhe in chuyenBay.ChiTietHangGhesList)
                    {
                        doanhThu += (Convert.ToInt32(chiTietHangGhe.SoLuongGhe) - Convert.ToInt32(chiTietHangGhe.SoLuongGheConLai)) * Convert.ToInt32(chiTietHangGhe.TyLe) /100  * Convert.ToInt32(chuyenBay.GiaVeCoBan);
                        tongSoVeDaBan += (Convert.ToInt32(chiTietHangGhe.SoLuongGhe) - Convert.ToInt32(chiTietHangGhe.SoLuongGheConLai));
                    }
                    tongDoanhThu += doanhThu;
                    DoanhThu doanhThuThang = new DoanhThu();
                    doanhThuThang.MaChuyenBay = chuyenBay.MaChuyenBay;
                    doanhThuThang.SoVe = tongSoVeDaBan;
                    doanhThuThang.DoanhThuInt = doanhThu;
                    doanhThuThang.DoanhThuTrieuDong = (float)doanhThuThang.DoanhThuInt / 1000000;
                    doanhThuThang.NgayBay = chuyenBay.NgayBay;
                    doanhThuThang.NgayBayString = chuyenBay.NgayBay.ToShortDateString();
                    doanhThus.Add(doanhThuThang);
                }    
            }
            tongDoanhThuTrieuDong = (float)tongDoanhThu / 1000000;
            
            for (int i = 0;i< doanhThus.Count;i++)
            {
                doanhThus[i].TyLe = doanhThus[i].DoanhThuTrieuDong / tongDoanhThuTrieuDong * 100;
            }    

            var newList = doanhThus.OrderBy(e => e.NgayBay.Day);
            doanhThus = new ChartValues<DoanhThu>(newList);

            foreach (DoanhThu doanhThuThang in doanhThus)
            {
                labels.Add(doanhThuThang.MaChuyenBay);
            }
        }
    }
}

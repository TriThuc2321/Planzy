using LiveCharts;
using Planzy.Models.ChuyenBayModel;
using Planzy.Models.DoanhThuModel;
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

        public List<List<ChuyenBay>> chuyenBayCuaNam = new List<List<ChuyenBay>>()
        {
            new List<ChuyenBay>(),
            new List<ChuyenBay>(),
            new List<ChuyenBay>(),
            new List<ChuyenBay>(),
            new List<ChuyenBay>(),
            new List<ChuyenBay>(),
            new List<ChuyenBay>(),
            new List<ChuyenBay>(),
            new List<ChuyenBay>(),
            new List<ChuyenBay>(),
            new List<ChuyenBay>(),
            new List<ChuyenBay>(),
        };
        public DoanhThuThangServices(string Nam, ChuyenBayServices chuyenBayServices)
        {
           
            foreach(ChuyenBay chuyenBay in chuyenBayServices.GetAll())
            {
                if (chuyenBay.NgayBay.Year == Convert.ToInt32(Nam))
                {
                    //if (chuyenBayCuaNam[chuyenBay.NgayBay.Month - 1] != null)
                        chuyenBayCuaNam[chuyenBay.NgayBay.Month - 1].Add(chuyenBay);   
                }    
            }

            labels = new List<string>();
            for (int i = 0;i<12;i++)
            {
                labels.Add((i+1).ToString());
            }

            doanhThuThangs = new ChartValues<DoanhThuThang>();
            for (int i = 0; i < 12; i++)
            {
                DoanhThuThang doanhThuThang = new DoanhThuThang(chuyenBayCuaNam[i]);
                doanhThuThang.DoanhThu = doanhThuThang.doanhThuServices.tongDoanhThu;
                doanhThuThang.DoanhThuTrieuDong = doanhThuThang.doanhThuServices.tongDoanhThuTrieuDong;
                doanhThuThang.SoChuyenBay = chuyenBayCuaNam[i].Count;
                doanhThuThang.Thang = (i + 1).ToString();
                tongDoanhThu += doanhThuThang.DoanhThu;

                doanhThuThangs.Add(doanhThuThang);
            }
            tongDoanhThuTrieuDong = (float)tongDoanhThu / 1000000;

            for (int i = 0; i < doanhThuThangs.Count; i++)
            {
                doanhThuThangs[i].TyLe = doanhThuThangs[i].DoanhThuTrieuDong / tongDoanhThuTrieuDong * 100;
            }
        }
    }
}

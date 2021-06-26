using LiveCharts;
using Planzy.Models.ChiTietHangGheModel;
using Planzy.Models.ChuyenBayModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.DoanhThuModel
{
    class DoanhThuServices
    {
        private static SqlConnection SanBayConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        public ChartValues<DoanhThu> doanhThus
        {
            get; set;
        }
        public List<string> labels { get; set; }
        public int tongDoanhThu = 0;
        public int doanhThuTangThem = 0;
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
                ThemDoanhThuVaoSQL(doanhThus[i]);
            }    

            var newList = doanhThus.OrderBy(e => e.NgayBay.Day);
            doanhThus = new ChartValues<DoanhThu>(newList);

            foreach (DoanhThu doanhThuThang in doanhThus)
            {
                labels.Add(doanhThuThang.MaChuyenBay);
            }
        }
        public DoanhThuServices(List<DoanhThu> doanhThus)
        {
            this.doanhThus = new ChartValues<DoanhThu>();
            labels = new List<string>();
            foreach(DoanhThu doanhThu in doanhThus)
            {
                this.doanhThus.Add(doanhThu);
                tongDoanhThu += doanhThu.DoanhThuInt;
            }
            tongDoanhThuTrieuDong = (float)tongDoanhThu / 1000000;
            var newList = this.doanhThus.OrderBy(e => e.NgayBay.Day);
            this.doanhThus = new ChartValues<DoanhThu>(newList);
            foreach (DoanhThu doanhThuThang in this.doanhThus)
            {
                labels.Add(doanhThuThang.MaChuyenBay);
            }
        }
        public void ThemDoanhThu(ChuyenBay chuyenBay)
        {
            DoanhThu doanhThu = new DoanhThu();
            doanhThu.MaChuyenBay = chuyenBay.MaChuyenBay;
            doanhThu.NgayBay = chuyenBay.NgayBay;
            doanhThu.NgayBayString = doanhThu.NgayBay.ToString();
            int tongSoVeDaBan = 0;
            int doanhThuVe = 0;
            foreach (ChiTietHangGhe chiTietHangGhe in chuyenBay.ChiTietHangGhesList)
            {
                doanhThuVe += (Convert.ToInt32(chiTietHangGhe.SoLuongGhe) - Convert.ToInt32(chiTietHangGhe.SoLuongGheConLai)) * Convert.ToInt32(chiTietHangGhe.TyLe) / 100 * Convert.ToInt32(chuyenBay.GiaVeCoBan);
                tongSoVeDaBan += (Convert.ToInt32(chiTietHangGhe.SoLuongGhe) - Convert.ToInt32(chiTietHangGhe.SoLuongGheConLai));
            }
            doanhThu.SoVe = tongSoVeDaBan;
            doanhThu.DoanhThuInt = doanhThuVe;
            doanhThu.DoanhThuTrieuDong = (float)doanhThu.DoanhThuInt / 1000000;
            tongDoanhThu += doanhThu.DoanhThuInt;
            doanhThuTangThem = doanhThu.DoanhThuInt;
            tongDoanhThuTrieuDong += doanhThu.DoanhThuTrieuDong;

            doanhThus.Add(doanhThu);
            labels.Add(doanhThu.MaChuyenBay);

            for (int i = 0; i < doanhThus.Count; i++)
            {
                if(doanhThus[i].DoanhThuInt !=0)
                doanhThus[i].TyLe = (float) doanhThus[i].DoanhThuTrieuDong / tongDoanhThuTrieuDong * 100;
            }

            XoaTatCaChuyenBayThangSQLLichSuChuyenBay(doanhThus[0].NgayBay.Year.ToString(), doanhThus[0].NgayBay.Month.ToString());

            for (int i = 0; i < doanhThus.Count; i++)
            {
                ThemDoanhThuVaoSQL(doanhThus[i]);
            }
        }

        public bool ThemDoanhThuVaoSQL(DoanhThu doanhThu)
        {
            bool result;
            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("Insert into LICH_SU_CHUYEN_BAY(MA_CHUYEN_BAY,SO_VE,DOANH_THU,NGAY_BAY,TY_LE) values('" +
                    doanhThu.MaChuyenBay + "','" +
                    doanhThu.SoVe + "','" +
                    doanhThu.DoanhThuInt + "','" +
                    doanhThu.NgayBayString + "','" +
                    doanhThu.TyLe + "')", SanBayConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                SanBayConnection.Close();
            }
            return result;
        }
        public bool XoaTatCaChuyenBayThangSQLLichSuChuyenBay(string Nam,string Thang)
        {
            bool result;
            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("Delete from LICH_SU_CHUYEN_BAY where year(NGAY_BAY) = '" + Nam + "' and month(NGAY_BAY) = '" + Thang + "'", SanBayConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                SanBayConnection.Close();
            }
            return result;
        }    
        public DoanhThuServices()
        { }
    }
}

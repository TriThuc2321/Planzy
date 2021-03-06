using LiveCharts;
using Planzy.Models.ChuyenBayModel;
using Planzy.Models.DoanhThuModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Planzy.Models.DoanhThuThangModel
{
    class DoanhThuThangServices
    {
        private static SqlConnection SanBayConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        public ChartValues<DoanhThuThang> doanhThuThangs
        {
            get; set;
        }
        public List<string> labels { get; set; }
        private int tongDoanhThu = 0;
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
        public List<List<DoanhThu>> doanhThuChuyenBayCuaNam = new List<List<DoanhThu>>()
        {
            new List<DoanhThu>(),
            new List<DoanhThu>(),
            new List<DoanhThu>(),
            new List<DoanhThu>(),
            new List<DoanhThu>(),
            new List<DoanhThu>(),
            new List<DoanhThu>(),
            new List<DoanhThu>(),
            new List<DoanhThu>(),
            new List<DoanhThu>(),
            new List<DoanhThu>(),
            new List<DoanhThu>(),
        };
        #region khởi tạo tất cả dữ liệu
        public DoanhThuThangServices(string Nam, ChuyenBayServices chuyenBayServices)
        {

            foreach (ChuyenBay chuyenBay in chuyenBayServices.GetAll())
            {
                if (chuyenBay.NgayBay.Year == Convert.ToInt32(Nam) && chuyenBay.IsDaBay == true)
                {
                    chuyenBayCuaNam[chuyenBay.NgayBay.Month - 1].Add(chuyenBay);
                }
            }

            labels = new List<string>();
            for (int i = 0; i < 12; i++)
            {
                labels.Add((i + 1).ToString());
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
        #endregion
        public void LoadSQL(string Nam)
        {

            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("Select * from LICH_SU_CHUYEN_BAY where year(NGAY_BAY) = '" + Nam + "'", SanBayConnection);
                command.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                foreach (DataRow row in dataTable.Rows)
                {
                    DoanhThu doanhThu = new DoanhThu();
                    doanhThu.MaChuyenBay = row["MA_CHUYEN_BAY"].ToString();
                    doanhThu.NgayBayString = row["NGAY_BAY"].ToString();
                    doanhThu.NgayBay = DateTime.Parse(doanhThu.NgayBayString);
                    doanhThu.SoVe = Convert.ToInt32(row["SO_VE"].ToString());
                    doanhThu.DoanhThuInt = Convert.ToInt32(row["DOANH_THU"].ToString());
                    doanhThu.DoanhThuTrieuDong = (float)doanhThu.DoanhThuInt / 1000000;
                    doanhThu.TyLe = (float)Convert.ToDouble(row["TY_LE"].ToString());
                    doanhThuChuyenBayCuaNam[doanhThu.NgayBay.Month - 1].Add(doanhThu);
                }
            }
            catch
            {

            }
            finally
            {
                SanBayConnection.Close();
            }
        }

        public void ThemDoanhThu(ChuyenBay chuyenBay)
        {
            doanhThuThangs[chuyenBay.NgayBay.Month - 1].doanhThuServices.ThemDoanhThu(chuyenBay);
            doanhThuThangs[chuyenBay.NgayBay.Month - 1].SoChuyenBay++;
            doanhThuThangs[chuyenBay.NgayBay.Month - 1].DoanhThu += doanhThuThangs[chuyenBay.NgayBay.Month - 1].doanhThuServices.doanhThuTangThem;
            doanhThuThangs[chuyenBay.NgayBay.Month - 1].DoanhThuTrieuDong = (float)doanhThuThangs[chuyenBay.NgayBay.Month - 1].DoanhThu / 1000000;
            tongDoanhThu += doanhThuThangs[chuyenBay.NgayBay.Month - 1].doanhThuServices.doanhThuTangThem;
            tongDoanhThuTrieuDong = (float)tongDoanhThu / 1000000;

            for (int i = 0; i < doanhThuThangs.Count; i++)
            {
                doanhThuThangs[i].TyLe = doanhThuThangs[i].DoanhThuTrieuDong / tongDoanhThuTrieuDong * 100;
            }
        }
        public DoanhThuThangServices(string Nam)
        {

            LoadSQL(Nam);

            labels = new List<string>();
            for (int i = 0; i < 12; i++)
            {
                labels.Add((i + 1).ToString());
            }

            doanhThuThangs = new ChartValues<DoanhThuThang>();
            for (int i = 0; i < 12; i++)
            {
                DoanhThuThang doanhThuThang = new DoanhThuThang(doanhThuChuyenBayCuaNam[i]);
                doanhThuThang.DoanhThu = doanhThuThang.doanhThuServices.tongDoanhThu;
                doanhThuThang.DoanhThuTrieuDong = doanhThuThang.doanhThuServices.tongDoanhThuTrieuDong;
                doanhThuThang.SoChuyenBay = doanhThuChuyenBayCuaNam[i].Count;
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
        public void XoaDuLieuSQL(string Nam, string Thang)
        {
            try
            {
                SanBayConnection.Open();
                SqlCommand command;
                if (Thang != "Tất cả")
                    command = new SqlCommand("delete from LICH_SU_CHUYEN_BAY WHERE year(NGAY_BAY) = '" + Nam + "' AND MONTH(NGAY_BAY) = '" + Thang + "'", SanBayConnection);
                else
                    command = new SqlCommand("delete from LICH_SU_CHUYEN_BAY WHERE year(NGAY_BAY) = '" + Nam + "'", SanBayConnection);
                command.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                SanBayConnection.Close();
            }
        }
        public void XoaDuLieu(string Nam, string Thang)
        {
            if (Thang != "Tất cả")
            {
                tongDoanhThu -= doanhThuThangs[Convert.ToInt32(Thang) - 1].DoanhThu;
                tongDoanhThuTrieuDong = (float)tongDoanhThu / 1000000;
                DoanhThuThang doanhThuThang = new DoanhThuThang(Thang);
                doanhThuThangs.RemoveAt(Convert.ToInt32(Thang) - 1);
                doanhThuThangs.Insert(Convert.ToInt32(Thang) - 1, doanhThuThang);
                for (int i = 0; i < doanhThuThangs.Count; i++)
                {
                    doanhThuThangs[i].TyLe = doanhThuThangs[i].DoanhThuTrieuDong / tongDoanhThuTrieuDong * 100;
                }
                XoaDuLieuSQL(Nam, Thang);
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    DoanhThuThang doanhThuThang = new DoanhThuThang((i + 1).ToString());
                    doanhThuThangs.RemoveAt(i);
                    doanhThuThangs.Insert(i, doanhThuThang);
                }
                tongDoanhThu = 0;
                tongDoanhThuTrieuDong = 0;
                XoaDuLieuSQL(Nam, Thang);
            }
        }
    }
}

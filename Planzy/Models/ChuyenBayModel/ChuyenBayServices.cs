using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planzy.Models.ChuyenBayModel;
using Planzy.Models.SanBayModel;
using Planzy.Models.SanBayTrungGianModel;

namespace Planzy.Models.ChuyenBayModel
{
    class ChuyenBayServices
    {
        private static SqlConnection SanBayConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        private static List<ChuyenBay> ChuyenBaysList;
        

        public ChuyenBayServices()
        {
            ChuyenBaysList = new List<ChuyenBay>();
            LayDuLieuTuSql();
        }
        public List<ChuyenBay> GetAll()
        {
            return ChuyenBaysList;
        }

        public bool Add(ChuyenBay newChuyenBay)
        {
            if (ChuyenBaysList.Count == 0)
            {
                ChuyenBaysList.Add(newChuyenBay);
                return true;
            }
            else
            {
               if ( ChuyenBaysList.Exists(e => e.MaChuyenBay == newChuyenBay.MaChuyenBay))
                {
                    return false;
                }    
               else
                {
                    ChuyenBaysList.Add(newChuyenBay);
                    return true;
                }    
            }

        }
        public bool Update(ChuyenBay chuyenBayUpdate)
        {
            bool isUpdate = false;
            for (int index = 0; index < ChuyenBaysList.Count; index++)
            {
                if (ChuyenBaysList[index].MaChuyenBay == chuyenBayUpdate.MaChuyenBay)
                {
                    ChuyenBaysList[index].MaChuyenBay = chuyenBayUpdate.MaChuyenBay;
                    isUpdate = true;
                }
            }
            return isUpdate;
        }
        public bool Delete(string Id)
        {
            bool isDeleted = false;
            for (int index = 0; index < ChuyenBaysList.Count; index++)
            {
                if (ChuyenBaysList[index].MaChuyenBay == Id)
                {
                    ChuyenBaysList.RemoveAt(index);
                    isDeleted = true;
                    break;
                }
            }
            return isDeleted;
        }
        //public SanBay SearchID(string Id)
        //{
        //    return SanBaysList.FirstOrDefault(e => e.Id == Id);
        //}
        //public SanBay SearchTen(string tenSanBay)
        //{
        //    return SanBaysList.FirstOrDefault(e => e.TenSanBay == tenSanBay);
        //}
        public void LayDuLieuTuSql()
        {

            try
            {
                SanBayConnection.Open();
                #region Truy vấn dữ liệu từ sql
                SqlCommand command = new SqlCommand("SELECT * FROM CHUYEN_BAY", SanBayConnection);
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                #endregion
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        //a = Convert.ToInt32(row["Doi1"].ToString());
                        //b = Convert.ToInt32(row["Doi2"].ToString());
                        //c = Convert.ToInt32(row["Doi3"].ToString());
                        //d = Convert.ToInt32(row["Doi4"].ToString());
                        //this.doi1Diem.Text = a.ToString();
                        //this.doi2Diem.Text = b.ToString();
                        //this.doi3Diem.Text = c.ToString();
                        //this.doi4Diem.Text = d.ToString();
                        ChuyenBay chuyenBay = new ChuyenBay();
                        chuyenBay.MaChuyenBay = row["MA_CHUYEN_BAY"].ToString();
                        chuyenBay.GiaVeCoBan = row["GIA_VE_CO_BAN"].ToString();
                        string maSanBayDen;
                        string maSanBayDi;
                        maSanBayDen = row["SAN_BAY_DEN"].ToString();
                        maSanBayDi = row["SAN_BAY_DI"].ToString();
                        List<SanBay> listAirport = SanBayService.GetAirportForFlight(maSanBayDen, maSanBayDi);
                        chuyenBay.SanBayDen = listAirport[0];
                        chuyenBay.SanBayDi = listAirport[1];
                        // tu gan 
                        chuyenBay.SoGheHang1 = "30";
                        chuyenBay.SoGheHang2 = "30";
                        chuyenBay.SoGheHang3 = "0";
                        chuyenBay.SoGheHang4 = "0";
                        chuyenBay.SanBayTrungGian = SanBayTrungGianService.GetAirportForFlight(listAirport, chuyenBay.MaChuyenBay);
                        chuyenBay.NgayBay = DateTime.Parse(row["NGAY_GIO_BAY"].ToString());
                        chuyenBay.ThoiGianBay = row["THOI_GIAN_BAY"].ToString();
                        if (row["GIA_VE_CO_BAN"].ToString() == "false")
                            chuyenBay.IsDaBay = false;
                        else
                            chuyenBay.IsDaBay = true;

                        ChuyenBaysList.Add(chuyenBay);
                    }
                    
                }
            }
            catch (Exception e)
            {
                
            }
            finally
            {
                SanBayConnection.Close();
            }
        }
        public ObservableCollection<ChuyenBay> GetFlightBookingList(ObservableCollection<ChuyenBay> FlightBookingList)
        {
            FlightBookingList = new ObservableCollection<ChuyenBay>();
            try
            {
               
                SanBayConnection.Open();
                #region Truy vấn dữ liệu từ sql
                SqlCommand command = new SqlCommand("SELECT * FROM CHUYEN_BAY WHERE DA_BAY = 'False'", SanBayConnection);
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                #endregion
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        //a = Convert.ToInt32(row["Doi1"].ToString());
                        //b = Convert.ToInt32(row["Doi2"].ToString());
                        //c = Convert.ToInt32(row["Doi3"].ToString());
                        //d = Convert.ToInt32(row["Doi4"].ToString());
                        //this.doi1Diem.Text = a.ToString();
                        //this.doi2Diem.Text = b.ToString();
                        //this.doi3Diem.Text = c.ToString();
                        //this.doi4Diem.Text = d.ToString();
                        ChuyenBay chuyenBay = new ChuyenBay();
                        chuyenBay.MaChuyenBay = row["MA_CHUYEN_BAY"].ToString();
                        chuyenBay.GiaVeCoBan = row["GIA_VE_CO_BAN"].ToString();
                        string maSanBayDen;
                        string maSanBayDi;
                        maSanBayDen = row["SAN_BAY_DEN"].ToString();
                        maSanBayDi = row["SAN_BAY_DI"].ToString();
                        List<SanBay> listAirport = SanBayService.GetAirportForFlight(maSanBayDen, maSanBayDi);
                        chuyenBay.SanBayDen = listAirport[0];
                        chuyenBay.SanBayDi = listAirport[1];
                        // tu gan 
                        chuyenBay.SoGheHang1 = "30";
                        chuyenBay.SoGheHang2 = "30";
                        chuyenBay.SoGheHang3 = "0";
                        chuyenBay.SoGheHang4 = "0";
                        chuyenBay.SanBayTrungGian = SanBayTrungGianService.GetAirportForFlight(listAirport, chuyenBay.MaChuyenBay);
                        chuyenBay.NgayBay = DateTime.Parse(row["NGAY_GIO_BAY"].ToString());
                        chuyenBay.ThoiGianBay = row["THOI_GIAN_BAY"].ToString();
                        if (row["GIA_VE_CO_BAN"].ToString() == "false")
                            chuyenBay.IsDaBay = false;
                        else
                            chuyenBay.IsDaBay = true;

                        FlightBookingList.Add(chuyenBay);
                    }
                }
                return FlightBookingList;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                
                SanBayConnection.Close();
            }
        }

    }
}

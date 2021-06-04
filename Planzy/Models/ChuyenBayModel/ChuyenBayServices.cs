using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planzy.Models.ChiTietHangGheModel;
using Planzy.Models.ChuyenBayModel;
using Planzy.Models.SanBayModel;
using Planzy.Models.SanBayTrungGianModel;

namespace Planzy.Models.ChuyenBayModel
{
    class ChuyenBayServices
    {
        private static SqlConnection SanBayConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        private static List<ChuyenBay> ChuyenBaysList;
        

        public ChuyenBayServices(SanBayTrungGianService sanBayTrungGianService, SanBayService sanBayService, ChiTietHangGheServices chiTietHangGheServices)
        {
            ChuyenBaysList = new List<ChuyenBay>();
            LoadFromSQL();
            LoadSanBayDiVaDen(sanBayService);
            LoadChiTietHangGhe(chiTietHangGheServices);
            LoadSanBayTrungGian(sanBayTrungGianService, sanBayService);
        }
        public List<ChuyenBay> GetAll()
        {
            return ChuyenBaysList;
        }

        public bool Add(ChuyenBay newChuyenBay)
        {
            if (ThemChuyenBaySQL(newChuyenBay))
            {
                if (ChuyenBaysList.Count == 0)
                {
                    ChuyenBaysList.Add(newChuyenBay);
                    return true;
                }
                else
                {
                    if (ChuyenBaysList.Exists(e => e.MaChuyenBay == newChuyenBay.MaChuyenBay))
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
            else
                return false;
        }
        public bool Update(ChuyenBay chuyenBayUpdate)
        {
            if (SuaChuyenBaySql(chuyenBayUpdate))
            {
                bool isUpdate = false;
                for (int index = 0; index < ChuyenBaysList.Count; index++)
                {
                    if (ChuyenBaysList[index].MaChuyenBay == chuyenBayUpdate.MaChuyenBay)
                    {
                        ChuyenBaysList[index] = chuyenBayUpdate;
                        isUpdate = true;
                    }
                }
                return isUpdate;
            }
            else
            {
                return false;
            }    
        }
        public bool Delete(string Id,SanBayTrungGianService sanBayTrungGianService, ChiTietHangGheServices chiTietHangGheServices)
        {
            if (XoaChuyenBaySql(Id, sanBayTrungGianService, chiTietHangGheServices))
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
            else
            {
                return false;
            }    
        }
        public bool IsEditable(string Id)
        {
            if (ChuyenBaysList.Exists(e => e.MaChuyenBay == Id))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool ThemChuyenBaySQL(ChuyenBay chuyenBay)
        {
            bool result;
            try
            {
                DateTime ngayGioBay = new DateTime(chuyenBay.NgayBay.Year, chuyenBay.NgayBay.Month, chuyenBay.NgayBay.Day, chuyenBay.GioBay.Hour, chuyenBay.GioBay.Minute, chuyenBay.GioBay.Second);
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("Insert into CHUYEN_BAY(MA_CHUYEN_BAY,GIA_VE_CO_BAN,SAN_BAY_DI,SAN_BAY_DEN,NGAY_GIO_BAY,THOI_GIAN_BAY,DA_BAY,SO_LOAI_HANG_GHE) VALUES ('" + chuyenBay.MaChuyenBay + "','" + chuyenBay.GiaVeCoBan + "','" + chuyenBay.SanBayDi.Id + "','" + chuyenBay.SanBayDen.Id + "','" + ngayGioBay.ToString() + "','" + chuyenBay.ThoiGianBay + "','" + chuyenBay.IsDaBay.ToString() + "','" + chuyenBay.SoLoaiHangGhe + "')", SanBayConnection);
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

        public bool SuaChuyenBaySql(ChuyenBay chuyenBay)
        {
            bool result;
            DateTime ngayGioBay = new DateTime(chuyenBay.NgayBay.Year, chuyenBay.NgayBay.Month, chuyenBay.NgayBay.Day, chuyenBay.GioBay.Hour, chuyenBay.GioBay.Minute, chuyenBay.GioBay.Second);

            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("UPDATE CHUYEN_BAY SET GIA_VE_CO_BAN = '" + chuyenBay.GiaVeCoBan + "',SAN_BAY_DI = '" + chuyenBay.SanBayDi.Id + "',SAN_BAY_DEN = '" + chuyenBay.SanBayDen.Id + "',NGAY_GIO_BAY = '" + ngayGioBay.ToString() + "',THOI_GIAN_BAY = '" + chuyenBay.ThoiGianBay + "',DA_BAY = '" + chuyenBay.IsDaBay.ToString() + "',SO_LOAI_HANG_GHE = '" + chuyenBay.SoLoaiHangGhe + "' WHERE MA_CHUYEN_BAY = '" + chuyenBay.MaChuyenBay + "'", SanBayConnection);
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
        public bool XoaChuyenBaySql(String maChuyenBay, SanBayTrungGianService sanBayTrungGianService, ChiTietHangGheServices chiTietHangGheServices)
        {
            if (sanBayTrungGianService.ClearSpecializeSanBay(maChuyenBay) && chiTietHangGheServices.Delete(maChuyenBay))
            {
                bool result;
                try
                {
                    SanBayConnection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM CHUYEN_BAY WHERE MA_CHUYEN_BAY = '" + maChuyenBay + "'", SanBayConnection);
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
            else
            {
                return false;
            }    
        }
        public bool LoadFromSQL()
        {
            bool result;
            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("Select * from CHUYEN_BAY", SanBayConnection);
                command.CommandType = CommandType.Text;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                foreach(DataRow row in dataTable.Rows)
                {
                    ChuyenBay chuyenBay = new ChuyenBay();
                    chuyenBay.MaChuyenBay = row["MA_CHUYEN_BAY"].ToString();
                    chuyenBay.GiaVeCoBan = row["GIA_VE_CO_BAN"].ToString();
                    chuyenBay.SanBayDi = new SanBay();chuyenBay.SanBayDi.Id = row["SAN_BAY_DI"].ToString();
                    chuyenBay.SanBayDen = new SanBay(); chuyenBay.SanBayDen.Id = row["SAN_BAY_DEN"].ToString();
                    chuyenBay.NgayBay = Convert.ToDateTime( row["NGAY_GIO_BAY"].ToString());
                    chuyenBay.GioBay = Convert.ToDateTime(row["NGAY_GIO_BAY"].ToString());
                    chuyenBay.ThoiGianBay = row["THOI_GIAN_BAY"].ToString();
                    chuyenBay.SoLoaiHangGhe = Convert.ToInt32(row["SO_LOAI_HANG_GHE"].ToString());
                    ChuyenBaysList.Add(chuyenBay);
                }
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
        public void LoadSanBayTrungGian(SanBayTrungGianService sanBayTrungGianService, SanBayService sanBayService)
        {
            if (ChuyenBaysList != null)
                foreach (ChuyenBay chuyenBay in ChuyenBaysList)
            {
                chuyenBay.SanBayTrungGian = sanBayTrungGianService.TimSanBayTrungGianList(chuyenBay.MaChuyenBay, chuyenBay.SanBayDi.Id, sanBayService);
            }
        }
        public void LoadSanBayDiVaDen(SanBayService sanBayService)
        {
            if (ChuyenBaysList != null)
                foreach (ChuyenBay chuyenBay in ChuyenBaysList)
            {
                chuyenBay.SanBayDi = sanBayService.SearchID(chuyenBay.SanBayDi.Id);
                chuyenBay.SanBayDen = sanBayService.SearchID(chuyenBay.SanBayDen.Id);
            }
        }
        public void LoadChiTietHangGhe(ChiTietHangGheServices chiTietHangGheServices)
        {
            if (ChuyenBaysList != null)
            foreach (ChuyenBay chuyenBay in ChuyenBaysList)
            {
                chuyenBay.ChiTietHangGhesList = chiTietHangGheServices.TimListHangGhe(chuyenBay.MaChuyenBay);
            }
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

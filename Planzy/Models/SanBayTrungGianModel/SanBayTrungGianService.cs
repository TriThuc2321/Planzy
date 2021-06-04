using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planzy.Models.SanBayModel;
namespace Planzy.Models.SanBayTrungGianModel
{
    class SanBayTrungGianService
    {
        private static SqlConnection SanBayConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        private static List<SanBayTrungGian> SanBayTrungGiansList;

        public SanBayTrungGianService()
        {
            SanBayTrungGiansList = new List<SanBayTrungGian>();
            LoadFromSQL();
        }
        public List<SanBayTrungGian> GetAll()
        {
            return SanBayTrungGiansList;
        }
        public bool Add(SanBayTrungGian newSanbay)
        {
            //if (SanBayTrungGiansList.Count == 0)
            //{
            //    SanBayTrungGiansList.Add(newSanbay);
            //    return true;
            //}
            //else
            //{
            //    for (int index = 0; index < SanBayTrungGiansList.Count; index++)
            //    {
            //        if (newSanbay.MaSanBaySau == SanBayTrungGiansList[index].MaSanBay)
            //        {
            //            SanBayTrungGiansList.Insert(index, newSanbay);
            //            return true;
            //        }    
            //    }
            //    SanBayTrungGiansList.Add(newSanbay);
            //    return true;
            //}    
            if(ThemSanBayTrungGianVaoSQL(newSanbay))
            {
                SanBayTrungGiansList.Add(newSanbay);
                return true;
            }    
            else
            {
                return false;
            }    
               
        }
        public void ThemListSanBayTrungGian(ObservableCollection<SanBayTrungGian> sanBayTrungGiansList)
        {
            if (sanBayTrungGiansList != null)
            {
                foreach (SanBayTrungGian sanBayTrungGian in sanBayTrungGiansList)
                {
                    Add(sanBayTrungGian);
                }
            }
            else
                return;
        }
        public bool ClearSpecializeSanBay(String maChuyenBay)
        {
            if (XoaSanBayTrungGianSQL(maChuyenBay))
            {
                SanBayTrungGiansList.RemoveAll(e => e.MaChuyenBay == maChuyenBay);
                return true;
            }
            else
                return false;
        }
        public bool ThemSanBayTrungGianVaoSQL(SanBayTrungGian newSanBay)
        {
            bool result;
            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO SAN_BAY_TRUNG_GIAN(MA_CHUYEN_BAY,MA_SAN_BAY,MA_SAN_BAY_TRUOC,MA_SAN_BAY_SAU,THOI_GIAN_DUNG) VALUES ('" + newSanBay.MaChuyenBay + 
                    "','" + newSanBay.MaSanBay +
                    "','" + newSanBay.MaSanBayTruoc +
                    "','" + newSanBay.MaSanBaySau +
                    "','" + newSanBay.ThoiGianDung +
                    "')", SanBayConnection);
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
        public bool XoaSanBayTrungGianSQL(String maChuyenBay)
        {
            bool result;
            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM SAN_BAY_TRUNG_GIAN WHERE MA_CHUYEN_BAY = '" + maChuyenBay + "'", SanBayConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch(Exception e)
            {
                result = false;
            }
            finally
            {
                SanBayConnection.Close();
            }
            return result;
        }
        public bool LoadFromSQL()
        {
            bool result;
            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("Select * from SAN_BAY_TRUNG_GIAN", SanBayConnection);
                command.CommandType = CommandType.Text;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                foreach(DataRow row in dataTable.Rows)
                {
                    SanBayTrungGian sanBayTrungGian = new SanBayTrungGian();
                    sanBayTrungGian.MaChuyenBay = row["MA_CHUYEN_BAY"].ToString();
                    sanBayTrungGian.MaSanBay = row["MA_SAN_BAY"].ToString();
                    sanBayTrungGian.MaSanBayTruoc = row["MA_SAN_BAY_TRUOC"].ToString();
                    sanBayTrungGian.MaSanBaySau = row["MA_SAN_BAY_SAU"].ToString();
                    sanBayTrungGian.ThoiGianDung = row["THOI_GIAN_DUNG"].ToString();
                    SanBayTrungGiansList.Add(sanBayTrungGian);
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
        public ObservableCollection<SanBayTrungGian> TimSanBayTrungGianList(string maChuyenBay, string sanBayDi, SanBayService sanBayService)
        {
            ObservableCollection<SanBayTrungGian> sanBayTrungGians = new ObservableCollection<SanBayTrungGian>();
            ObservableCollection<SanBayTrungGian> temp = new ObservableCollection<SanBayTrungGian>();
            foreach (SanBayTrungGian sanBayTrungGian in SanBayTrungGiansList)
            {
                if (sanBayTrungGian.MaChuyenBay == maChuyenBay)
                {
                    sanBayTrungGian.TenSanBay = sanBayService.SearchTen(sanBayTrungGian.MaSanBay);
                    temp.Add(sanBayTrungGian);
                }
            }
            int turn = 0;
            while(temp.Count != 0)
            {
                if (turn == temp.Count)
                    turn = 0;
                if (sanBayTrungGians.Count == 0)
                {
                    if (temp[turn].MaSanBayTruoc == sanBayDi)
                    {
                        sanBayTrungGians.Add(temp[turn]);
                        temp.RemoveAt(turn);
                    }
                    else
                        turn++;
                }
                else
                {
                    if (temp[turn].MaSanBay == sanBayTrungGians[sanBayTrungGians.Count - 1].MaSanBaySau)
                    {
                        sanBayTrungGians.Add(temp[turn]);
                        temp.RemoveAt(turn);
                    }
                    else
                        turn++;
                } 
                    
            }    
            return sanBayTrungGians;
        }
        //public bool ThemListSanBayTrungGianVaoSQL(ObservableCollection<SanBayTrungGian> sanBayTrungGians)
        //{
        //    bool result;
        //    try
        //    {
        //        SanBayConnection.Open();
        //        foreach (SanBayTrungGian newSanBay in sanBayTrungGians)
        //        {
        //            SqlCommand command = new SqlCommand("INSERT INTO SAN_BAY_TRUNG_GIAN(MA_CHUYEN_BAY,MA_SAN_BAY,MA_SAN_BAY_TRUOC,MA_SAN_BAY_SAU,THOI_GIAN_DUNG) VALUES ('" + newSanBay.MaChuyenBay +
        //                "','" + newSanBay.MaSanBay +
        //                "','" + newSanBay.MaSanBayTruoc +
        //                "','" + newSanBay.MaSanBaySau +
        //                "','" + newSanBay.ThoiGianDung +
        //                "')", SanBayConnection);
        //            command.ExecuteNonQuery();
        //        }
        //        result = true;
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    finally
        //    {
        //        SanBayConnection.Close();
        //    }
        //    return result;
        //}
        //public bool Update(SanBay sanBayUpdate)
        //{
        //    bool isUpdate = false;
        //    for (int index = 0; index < SanBaysList.Count; index++)
        //    {
        //        if (SanBaysList[index].Id == sanBayUpdate.Id)
        //        {
        //            SanBaysList[index].TenSanBay = sanBayUpdate.TenSanBay;
        //            isUpdate = true;
        //        }
        //    }
        //    return isUpdate;
        //}
        //public bool Delete(string Id)
        //{
        //    bool isDeleted = false;
        //    for (int index = 0; index < SanBaysList.Count; index++)
        //    {
        //        if (SanBaysList[index].Id == Id)
        //        {
        //            SanBaysList.RemoveAt(index);
        //            isDeleted = true;
        //            break;
        //        }
        //    }
        //    return isDeleted;
        //}
        //public SanBay SearchID(string Id)
        //{
        //    return SanBaysList.FirstOrDefault(e => e.Id == Id);
        //}
        //public SanBay SearchTen(string tenSanBay)
        //{
        //    return SanBaysList.FirstOrDefault(e => e.TenSanBay == tenSanBay);
        //}
        public static ObservableCollection<SanBayTrungGian> GetAirportForFlight(List<SanBay> ListAirport, string flightID)
        {
            SqlConnection PlanzyConnection_ = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
            ObservableCollection<SanBayTrungGian> processedList = new ObservableCollection<SanBayTrungGian>();
            List<SanBayTrungGian> list = new List<SanBayTrungGian>();
            try
            {
                PlanzyConnection_.Open();
                #region Truy vấn dữ liệu từ sql
                string Query = string.Format("SELECT * FROM SAN_BAY_TRUNG_GIAN WHERE MA_CHUYEN_BAY ='{0}'",
                    flightID);
                SqlCommand command = new SqlCommand(Query, PlanzyConnection_);
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                #endregion
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        SanBayTrungGian sanBay = new SanBayTrungGian();
                        sanBay.MaChuyenBay = row["MA_CHUYEN_BAY"].ToString();
                        sanBay.MaSanBay = row["MA_SAN_BAY"].ToString();
                        sanBay.MaSanBayTruoc = row["MA_SAN_BAY_TRUOC"].ToString();
                        sanBay.MaSanBaySau = row["MA_SAN_BAY_SAU"].ToString();
                        sanBay.ThoiGianDung = row["THOI_GIAN_DUNG"].ToString();
                        sanBay.TenSanBay = SanBayService.GetNameOfAirPort(sanBay.MaSanBay);
                        list.Add(sanBay);
                    }
                    SortAirportList(processedList, list, ListAirport[0].Id, ListAirport[1].Id);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                PlanzyConnection_.Close();
            }
            return processedList;
        }
        public static void SortAirportList(ObservableCollection<SanBayTrungGian> ProcessedList, List<SanBayTrungGian> list, String firstAirportID, String lastAirportID  )
        {
            String theBeforeAirPortID = firstAirportID;
            String theLastAirPortID = lastAirportID;
            if (list == null) return;
            for (int i = 0; i < list.Count || list.Count == 0; i++)
            {             
                if (list[i].MaSanBayTruoc == theBeforeAirPortID)
                {
                    theBeforeAirPortID = list[i].MaSanBay;
                    ProcessedList.Add(list[i]);
                    list.RemoveAt(i);
                    i = -1;                  
                }
            }
            

        }
    }
}

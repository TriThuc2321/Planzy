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
        private static List<SanBayTrungGian> SanBayTrungGiansList;

        public SanBayTrungGianService()
        {
            SanBayTrungGiansList = new List<SanBayTrungGian>();
        }
        public List<SanBayTrungGian> GetAll()
        {
            return SanBayTrungGiansList;
        }
        public bool Add(SanBayTrungGian newSanbay)
        {
            if (SanBayTrungGiansList.Count == 0)
            {
                SanBayTrungGiansList.Add(newSanbay);
                return true;
            }
            else
            {
                for (int index = 0; index < SanBayTrungGiansList.Count; index++)
                {
                    if (newSanbay.MaSanBaySau == SanBayTrungGiansList[index].MaSanBay)
                    {
                        SanBayTrungGiansList.Insert(index, newSanbay);
                        return true;
                    }    
                }
                SanBayTrungGiansList.Add(newSanbay);
                return true;
            }    
               
        }
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

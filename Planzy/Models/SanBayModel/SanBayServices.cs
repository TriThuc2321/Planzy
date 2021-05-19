using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace Planzy.Models.SanBayModel
{
    class SanBayService
    {
        private static SqlConnection SanBayConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        private static List<SanBay> SanBaysList;

        public SanBayService()
        {
            SanBaysList = new List<SanBay>();
            LayDuLieuTuSql();
        }
        public List<SanBay> GetAll()
        {
            return SanBaysList;
        }
        public bool Add(SanBay newSanbay)
        {
            if (SanBaysList.Contains(newSanbay))
            {
                return false;
            }
            else
            {
                SanBaysList.Add(newSanbay);
                return true;
            }
        }
        public bool Update(SanBay sanBayUpdate)
        {
            bool isUpdate = false;
            for (int index = 0; index < SanBaysList.Count; index++)
            {
                if (SanBaysList[index].Id == sanBayUpdate.Id)
                {
                    SanBaysList[index].TenSanBay = sanBayUpdate.TenSanBay;
                    isUpdate = true;
                }
            }
            return isUpdate;
        }
        public bool Delete(string Id)
        {
            bool isDeleted = false;
            for (int index = 0; index < SanBaysList.Count; index++)
            {
                if (SanBaysList[index].Id == Id)
                {
                    SanBaysList.RemoveAt(index);
                    isDeleted = true;
                    break;
                }
            }
            return isDeleted;
        }
        public SanBay SearchID(string Id)
        {
            return SanBaysList.FirstOrDefault(e => e.Id == Id);
        }
        public SanBay SearchTen(string tenSanBay)
        {
            return SanBaysList.FirstOrDefault(e => e.TenSanBay == tenSanBay);
        }
        #region SQL Command
        public void LayDuLieuTuSql()
        {

            try
            {
                SanBayConnection.Open();
                #region Truy vấn dữ liệu từ sql
                SqlCommand command = new SqlCommand("SELECT * FROM SAN_BAY ", SanBayConnection);
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
                        SanBay sanBay = new SanBay();
                        sanBay.Id = row["MA_SAN_BAY"].ToString();
                        sanBay.TenSanBay = row["TEN_SAN_BAY"].ToString();
                        SanBaysList.Add(sanBay);
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
        public void UpdateCommand(SanBay updateSanBay)
        {
            try
            {

            }
            catch (Exception e)
            {

            }
            finally
            {

            }
        }
        #endregion

        public static List<SanBay> GetAirportForFlight(string maSanBayDen, string maSanBayDi)
        {
            SqlConnection SanBayConnection_ = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
            List<SanBay> list = new List<SanBay>();
            try
            {
                SanBayConnection_.Open();
                #region Truy vấn dữ liệu từ sql
                string Query = string.Format("SELECT * FROM SAN_BAY WHERE MA_SAN_BAY = '{0}' OR MA_SAN_BAY ='{1}'",
                    maSanBayDen, maSanBayDi);
                SqlCommand command = new SqlCommand(Query, SanBayConnection_);
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                #endregion
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        SanBay sanBay = new SanBay();
                        sanBay.Id = row["MA_SAN_BAY"].ToString();
                        sanBay.TenSanBay = row["TEN_SAN_BAY"].ToString();
                        list.Add(sanBay);
                    }
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                SanBayConnection_.Close();
            }
            return list;
        }
    }

}

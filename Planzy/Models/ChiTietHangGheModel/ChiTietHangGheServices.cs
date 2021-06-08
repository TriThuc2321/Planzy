using Planzy.Models.LoaiHangGheModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.ChiTietHangGheModel
{
    class ChiTietHangGheServices
    {
        private static SqlConnection SanBayConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        private static List<ChiTietHangGhe> chiTietHangGhesList;

        public ChiTietHangGheServices()
        {
            chiTietHangGhesList = new List<ChiTietHangGhe>();
            LoadFromSQL();
        }
        public List<ChiTietHangGhe> GetAll()
        {
            return chiTietHangGhesList;
        }
        public bool Add(ChiTietHangGhe chiTietHangGhe)
        {
            if(AddSQL(chiTietHangGhe))
            {
                chiTietHangGhesList.Add(chiTietHangGhe);
                return false;
            }   
            else
            {
                return false;
            }    
        }
        public bool AddSQL(ChiTietHangGhe chiTietHangGhe)
        {
            bool result;
            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("insert into CHI_TIET_HANG_GHE(MA_LOAI_HANG_GHE,MA_CHUYEN_BAY,SO_LUONG_TONG,SO_LUONG_CON_LAI) VALUES('" +
                    chiTietHangGhe.MaLoaiHangGhe + "','" +
                    chiTietHangGhe.MaChuyenBay + "','" +
                    chiTietHangGhe.SoLuongGhe + "','" +
                    chiTietHangGhe.SoLuongGheConLai +
                    "')",SanBayConnection);
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
        public bool ThemListChiTietHangGheSQL(List<ChiTietHangGhe> chiTietHangGhes)
        {
            bool result;
            try
            {
                if (chiTietHangGhes != null)
                {
                    foreach (ChiTietHangGhe chiTietHangGhe in chiTietHangGhes)
                    {
                        Add(chiTietHangGhe);
                    }
                    result = true;
                }
                else
                    result = false;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public void ThemListChiTietHangGhe(List<ChiTietHangGhe> chiTietHangGhes)
        {
            if(ThemListChiTietHangGheSQL(chiTietHangGhes))
            {
                ThemListChiTietHangGheSQL(chiTietHangGhes);
            }    
        }
        public bool DeleteSQL(string maChuyenBay)
        {
            bool result;
            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("Delete from CHI_TIET_HANG_GHE WHERE MA_CHUYEN_BAY = '" + maChuyenBay + "'",SanBayConnection);
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
        public bool Delete(string maChuyenBay)
        {
            if (DeleteSQL(maChuyenBay))
            {
                chiTietHangGhesList.RemoveAll(e => e.MaChuyenBay == maChuyenBay);
                return true;
            }
            else
                return false;
        }
        public bool LoadFromSQL()
        {
            bool result;
            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("Select * from CHI_TIET_HANG_GHE", SanBayConnection);
                command.CommandType = CommandType.Text;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                    foreach(DataRow row in dataTable.Rows)
                    {
                        ChiTietHangGhe chiTietHangGhe = new ChiTietHangGhe();
                        chiTietHangGhe.MaChuyenBay = row["MA_CHUYEN_BAY"].ToString();
                        chiTietHangGhe.MaLoaiHangGhe = row["MA_LOAI_HANG_GHE"].ToString();
                        chiTietHangGhe.SoLuongGhe = row["SO_LUONG_TONG"].ToString();
                        chiTietHangGhe.SoLuongGheConLai = row["SO_LUONG_CON_LAI"].ToString();
                    chiTietHangGhesList.Add(chiTietHangGhe);
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
        public List<ChiTietHangGhe> TimListHangGhe(string maChuyenBay)
        {
            List<ChiTietHangGhe> chiTietHangGhes = new List<ChiTietHangGhe>();
            foreach (ChiTietHangGhe chiTietHangGhe in chiTietHangGhesList)
            {
                if (chiTietHangGhe.MaChuyenBay == maChuyenBay)
                    chiTietHangGhes.Add(chiTietHangGhe);
            }

            var newList = chiTietHangGhes.OrderByDescending(e => Convert.ToInt32( e.TyLe));
            chiTietHangGhes = new List<ChiTietHangGhe>(newList);
            return chiTietHangGhes;
        }
    }
}

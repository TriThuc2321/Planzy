using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.LoaiHangGheModel
{
    class LoaiHangGheServices
    {
        private static SqlConnection SanBayConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        private static List<LoaiHangGhe> LoaiHangGhesList;
        private const int SO_HANG_GHE_TOI_DA = 8;
        public LoaiHangGheServices()
        {
            LoaiHangGhesList = new List<LoaiHangGhe>();
            LoadSQL();
            while(LoaiHangGhesList.Count != 8)
            {
                LoaiHangGhesList.Add(new LoaiHangGhe());
            }    
        }    
        public List<LoaiHangGhe> GetAll()
        {
            return LoaiHangGhesList;
        }

        public bool LoadSQL()
        {
            bool result;
            try
            {
                SanBayConnection.Open();
                #region Truy vấn dữ liệu từ sql
                SqlCommand command = new SqlCommand("SELECT * FROM LOAI_HANG_GHE ", SanBayConnection);
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                #endregion
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        LoaiHangGhe hangGhe = new LoaiHangGhe();
                        hangGhe.MaLoaiHangGhe = row["MA_LOAI_HANG_GHE"].ToString();
                        hangGhe.TenLoaiHangGhe = row["TEN_LOAI_HANG_GHE"].ToString();
                        hangGhe.TyLe = row["TY_LE"].ToString();
                        LoaiHangGhesList.Add(hangGhe);
                    }
                }

                var newList = LoaiHangGhesList.OrderByDescending(e => Convert.ToInt32( e.TyLe));
                LoaiHangGhesList = new List<LoaiHangGhe>(newList);
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
    }
}

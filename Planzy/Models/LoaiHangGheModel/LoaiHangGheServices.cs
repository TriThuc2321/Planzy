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
        private  SqlConnection SanBayConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        private List<LoaiHangGhe> LoaiHangGhesList;
        private const int SO_HANG_GHE_TOI_DA = 8;
        public LoaiHangGheServices()
        {
            LoaiHangGhesList = new List<LoaiHangGhe>();
            LoadSQL();
        }    
        public List<LoaiHangGhe> GetAll()
        {
            return LoaiHangGhesList;
        }
        public void Add(LoaiHangGhe loaiHangGhe)
        {
            LoaiHangGhesList.Add(loaiHangGhe);
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

                var newList = LoaiHangGhesList.OrderByDescending(e =>Convert.ToInt32( e.TyLe));
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

        public string GetId(int k)
        {
            string temp = RandomString(k);
            while (true)
            {
                int i = 0;
                for ( i = 0; i < LoaiHangGhesList.Count; i++)
                {
                    if (temp == LoaiHangGhesList[i].MaLoaiHangGhe)
                    {
                        temp = RandomString(k);
                        break;
                    }
                }
                if (i == LoaiHangGhesList.Count) return temp;
            }
            


        }

        private  Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

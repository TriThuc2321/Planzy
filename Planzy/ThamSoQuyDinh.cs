using Planzy.Models.LoaiHangGheModel;
using Planzy.Models.ChiTietHangGheModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy
{
    public class ThamSoQuyDinh
    {
        private static SqlConnection SanBayConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        public static string SO_SAN_BAY_TOI_DA;
        public static string THOI_GIAN_BAY_TOI_THIEU;
        public static string SO_SAN_BAY_TRUNG_GIAN_TOI_DA;
        public static string THOI_GIAN_DUNG_TOI_DA;
        public static string THOI_GIAN_DUNG_TOI_THIEU;
        public static string SO_LUONG_CAC_HANG_VE;
        public static string THOI_GIAN_CHAM_NHAT_HUY_VE;
        public static string THOI_GIAN_CHAM_NHAT_DAT_VE;
        public static string TY_LE_HANG_GHE_LOAI_1;
        public static string TY_LE_HANG_GHE_LOAI_2;
        public static string TY_LE_HANG_GHE_LOAI_3;
        public static string TY_LE_HANG_GHE_LOAI_4;
        public static bool LoadThamSoQuyDinhTuSQL()
        {
            bool result;
            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("Select * from THAM_SO_QUY_DINH", SanBayConnection);
                command.CommandType = CommandType.Text;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                SO_SAN_BAY_TOI_DA = dataTable.Rows[0]["SO_SAN_BAY_TOI_DA"].ToString();
                THOI_GIAN_BAY_TOI_THIEU = dataTable.Rows[0]["THOI_GIAN_BAY_TOI_THIEU"].ToString();
                SO_SAN_BAY_TRUNG_GIAN_TOI_DA = dataTable.Rows[0]["SO_SAN_BAY_TRUNG_GIAN_TOI_DA"].ToString();
                THOI_GIAN_DUNG_TOI_DA = dataTable.Rows[0]["THOI_GIAN_DUNG_TOI_DA"].ToString();
                THOI_GIAN_DUNG_TOI_THIEU = dataTable.Rows[0]["THOI_GIAN_DUNG_TOI_THIEU"].ToString();
                SO_LUONG_CAC_HANG_VE = dataTable.Rows[0]["SO_LUONG_CAC_HANG_VE"].ToString();
                THOI_GIAN_CHAM_NHAT_HUY_VE = dataTable.Rows[0]["THOI_GIAN_CHAM_NHAT_HUY_VE"].ToString();
                THOI_GIAN_CHAM_NHAT_DAT_VE = dataTable.Rows[0]["THOI_GIAN_CHAM_NHAT_DAT_VE"].ToString();
                TY_LE_HANG_GHE_LOAI_1 = dataTable.Rows[0]["TY_LE_HANG_GHE_LOAI_1"].ToString();
                TY_LE_HANG_GHE_LOAI_2 = dataTable.Rows[0]["TY_LE_HANG_GHE_LOAI_2"].ToString();
                TY_LE_HANG_GHE_LOAI_3 = dataTable.Rows[0]["TY_LE_HANG_GHE_LOAI_3"].ToString();
                TY_LE_HANG_GHE_LOAI_4 = dataTable.Rows[0]["TY_LE_HANG_GHE_LOAI_4"].ToString();

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
        public static bool LoadThamSoQuyDinhXuongSQL (string SO_SAN_BAY_TOI_DA,
            string THOI_GIAN_BAY_TOI_THIEU,
            string SO_SAN_BAY_TRUNG_GIAN_TOI_DA, 
            string THOI_GIAN_DUNG_TOI_DA,
            string THOI_GIAN_DUNG_TOI_THIEU,
            string SO_LUONG_CAC_HANG_VE,
            string THOI_GIAN_CHAM_NHAT_HUY_VE,
            string THOI_GIAN_CHAM_NHAT_DAT_VE,
            string TY_LE_HANG_GHE_LOAI_1,
            string TY_LE_HANG_GHE_LOAI_2,
            string TY_LE_HANG_GHE_LOAI_3,
            string TY_LE_HANG_GHE_LOAI_4)
        {
            bool result;
            try
            {
                SanBayConnection.Open();
                SqlCommand command = new SqlCommand("delete from THAM_SO_QUY_DINH", SanBayConnection);
                command.ExecuteNonQuery();

                command = new SqlCommand("Insert into THAM_SO_QUY_DINH(SO_SAN_BAY_TOI_DA,THOI_GIAN_BAY_TOI_THIEU, SO_SAN_BAY_TRUNG_GIAN_TOI_DA, THOI_GIAN_DUNG_TOI_DA, THOI_GIAN_DUNG_TOI_THIEU, SO_LUONG_CAC_HANG_VE, THOI_GIAN_CHAM_NHAT_HUY_VE, THOI_GIAN_CHAM_NHAT_DAT_VE, TY_LE_HANG_GHE_LOAI_1, TY_LE_HANG_GHE_LOAI_2, TY_LE_HANG_GHE_LOAI_3, TY_LE_HANG_GHE_LOAI_4) values ('" +
                    SO_SAN_BAY_TOI_DA + "','" +
                    THOI_GIAN_BAY_TOI_THIEU + "','" +
                    SO_SAN_BAY_TRUNG_GIAN_TOI_DA + "','" +
                    THOI_GIAN_DUNG_TOI_DA + "','" +
                    THOI_GIAN_DUNG_TOI_THIEU + "','" +
                    SO_LUONG_CAC_HANG_VE + "','" +
                    THOI_GIAN_CHAM_NHAT_HUY_VE + "','" +
                    THOI_GIAN_CHAM_NHAT_DAT_VE + "','" + 
                    TY_LE_HANG_GHE_LOAI_1 + "','" +
                    TY_LE_HANG_GHE_LOAI_2 + "','" +
                    TY_LE_HANG_GHE_LOAI_3 + "','" +
                    TY_LE_HANG_GHE_LOAI_4 + "','" +
                    ")",SanBayConnection);
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
        #region loaihangghe
        static List<LoaiHangGhe> listUpdate;
        static List<LoaiHangGhe> listInsert;
        static List<LoaiHangGhe> listDelete;
        static LoaiHangGheServices loaiHangGheServices;
        static List<SqlCommand> listSqlCommands;
        public static void updateTicketTypeToSql(List<LoaiHangGhe> listTicketType)
        {
            for(int i=0; i<listTicketType.Count; i++)
            {
                listTicketType[i].KhaDung = "1";
            }

            classify(listTicketType);

            listSqlCommands = new List<SqlCommand>();

            for (int i=0; i<listInsert.Count; i++)
            {
                string query = "INSERT INTO LOAI_HANG_GHE (MA_LOAI_HANG_GHE, TEN_LOAI_HANG_GHE, TY_LE, KHA_DUNG) VALUES(@Maloaihangghe, @Tenloaihangghe, @Tyle, @Khadung)";
                SqlCommand command = new SqlCommand(query, SanBayConnection);

                command.Parameters.AddWithValue("@Maloaihangghe", listInsert[i].MaLoaiHangGhe);
                command.Parameters.AddWithValue("@Tenloaihangghe", listInsert[i].TenLoaiHangGhe);
                command.Parameters.AddWithValue("@Tyle", listInsert[i].TyLe);
                command.Parameters.AddWithValue("@Khadung", "1");

                listSqlCommands.Add(command);
            }
            
            for(int i=0; i<listUpdate.Count; i++)
            {
                string query = "UPDATE LOAI_HANG_GHE SET TEN_LOAI_HANG_GHE = @Tenloaihangghe, TY_LE = @Tyle, KHA_DUNG = @Khadung WHERE MA_LOAI_HANG_GHE = @Maloaihangghe";
                SqlCommand command = new SqlCommand(query, SanBayConnection);

                command.Parameters.AddWithValue("@Maloaihangghe", listUpdate[i].MaLoaiHangGhe);
                command.Parameters.AddWithValue("@Tenloaihangghe", listUpdate[i].TenLoaiHangGhe);
                command.Parameters.AddWithValue("@Tyle", listUpdate[i].TyLe);
                command.Parameters.AddWithValue("@Khadung", "1");

                listSqlCommands.Add(command);
            }

            for (int i = 0; i < listDelete.Count; i++)
            {
                string query = "UPDATE LOAI_HANG_GHE SET KHA_DUNG = @Khadung WHERE MA_LOAI_HANG_GHE = @Maloaihangghe";
                SqlCommand command = new SqlCommand(query, SanBayConnection);

                command.Parameters.AddWithValue("@Maloaihangghe", listDelete[i].MaLoaiHangGhe);
                command.Parameters.AddWithValue("@Khadung", "0");

                listSqlCommands.Add(command);
            }

            string deleteCTHG = "DELETE CHI_TIET_HANG_GHE WHERE CHI_TIET_HANG_GHE.MA_CHUYEN_BAY IN(SELECT CTHG.MA_CHUYEN_BAY FROM CHUYEN_BAY CB, CHI_TIET_HANG_GHE CTHG, LOAI_HANG_GHE LHG WHERE CB.MA_CHUYEN_BAY = CTHG.MA_CHUYEN_BAY AND CTHG.MA_LOAI_HANG_GHE = LHG.MA_LOAI_HANG_GHE AND CB.DA_BAY = 0 AND CTHG.SO_LUONG_TONG = CTHG.SO_LUONG_CON_LAI AND LHG.KHA_DUNG = '0')";
            SqlCommand commandCTHG = new SqlCommand(deleteCTHG, SanBayConnection);
            listSqlCommands.Add(commandCTHG);

            try
            {
                SanBayConnection.Open();
                for(int i=0; i<listSqlCommands.Count; i++)
                {
                    listSqlCommands[i].ExecuteNonQuery();
                }

            }
            catch (SqlException e)
            {

            }
            finally
            {
                SanBayConnection.Close();
            }
        }
        static void classify(List<LoaiHangGhe> list)
        {
            loaiHangGheServices = new LoaiHangGheServices();
            List<LoaiHangGhe> listSql = loaiHangGheServices.GetAll(); 

            listInsert = new List<LoaiHangGhe>();
            listUpdate = new List<LoaiHangGhe>();
            listDelete = new List<LoaiHangGhe>();

            for(int i =0; i< list.Count; i++)
            {
                int k = 0;
                if (listSql.Contains(list[i])) continue;

                for( k =0; k<listSql.Count; k++)
                {
                    if(listSql[k].MaLoaiHangGhe == list[i].MaLoaiHangGhe) { 
                        listUpdate.Add(list[i]);  
                        break; 
                    }
                    
                }
                if(k == listSql.Count) { listInsert.Add(list[i]); }
            }
            
            for(int i=0; i< listSql.Count; i++)
            {
                int k = 0;
                for( k =0; k<list.Count(); k++)
                {
                    if (listSql[i].MaLoaiHangGhe == list[k].MaLoaiHangGhe) break;
                }
                if (k == list.Count) listDelete.Add(listSql[i]);
            }

        }
        

        #endregion
    }
}

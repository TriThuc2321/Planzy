using Planzy.Models.LoaiHangGheModel;
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



        #endregion
    }
}

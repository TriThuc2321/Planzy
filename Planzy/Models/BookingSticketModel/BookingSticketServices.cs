using Planzy.Models.ChuyenBayModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.BookingSticketModel
{
    public class BookingSticketServices 
    {
        private static SqlConnection BookingSticketConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        public static void BookingSticketProcess(BookingSticket sticket,string account)
        {

            try
            {
                BookingSticketConnection.Open();
                string query = string.Format("INSERT INTO PHIEU_DAT_CHO " +
                    "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')",
                    sticket.BookingSticketID, sticket.FlightID, sticket.SticketTypeID,
                    sticket.PassengerName, sticket.CMND, sticket.Contact, sticket.Cost.Replace(" VND",""),
                    sticket.BookingDate, sticket.Gmail, sticket.Address, sticket.Request);

                SqlCommand command = new SqlCommand(query, BookingSticketConnection);
                command.ExecuteNonQuery();
                /// Them vao Chi Tiet Phieu Dat Cho

                query = string.Format("INSERT INTO CHI_TIET_PHIEU_DAT_CHO " +
                   " VALUES('{0}', '{1}')",
                   sticket.BookingSticketID,account);
                command = new SqlCommand(query, BookingSticketConnection);
                command.ExecuteNonQuery();

                //
                query = string.Format("UPDATE CHI_TIET_HANG_GHE SET SO_LUONG_CON_LAI = SO_LUONG_CON_LAI - 1 WHERE MA_LOAI_HANG_GHE = '{0}' and MA_CHUYEN_BAY = '{1}'",
                  sticket.SticketTypeID, sticket.FlightID);
                command = new SqlCommand(query, BookingSticketConnection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                BookingSticketConnection.Close();
            }
        }

        public static void DeletingSticketProcess(BookingSticket sticket)
        {

            try
            {
                BookingSticketConnection.Open();
                string query = string.Format("delete  from CHI_TIET_PHIEU_DAT_CHO WHERE MA_PHIEU ='{0}' " +
                    " delete  from PHIEU_DAT_CHO WHERE MA_PHIEU = '{0}'", sticket.BookingSticketID);

                SqlCommand command = new SqlCommand(query, BookingSticketConnection);
                command.ExecuteNonQuery();
                
            }
            catch (Exception e)
            {

            }
            finally
            {
                BookingSticketConnection.Close();
            }
        }
        public static void XoaPhieuDatCho(string maPhieu)
        {

            try
            {
                BookingSticketConnection.Open();
                string query = string.Format("delete  from CHI_TIET_PHIEU_DAT_CHO WHERE MA_PHIEU ='{0}' " +
                    " delete  from PHIEU_DAT_CHO WHERE MA_PHIEU = '{0}'", maPhieu);

                SqlCommand command = new SqlCommand(query, BookingSticketConnection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                BookingSticketConnection.Close();
            }
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static ObservableCollection<BookingSticket> GetFlightBookingList(string account)
        {
            ObservableCollection<BookingSticket> BookingSticketList = new ObservableCollection<BookingSticket>();
            try
            {

                BookingSticketConnection.Open();
                #region Truy vấn dữ liệu từ sql
                string query = String.Format("Select distinct * from CHI_TIET_PHIEU_DAT_CHO a, PHIEU_DAT_CHO b, CHUYEN_BAY c" +
                    " where a.MA_PHIEU = b.MA_PHIEU and b.MA_CHUYEN_BAY = c.MA_CHUYEN_BAY and MA_TAI_KHOAN = '{0}'", account);
                SqlCommand command = new SqlCommand(query, BookingSticketConnection);
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                #endregion
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        BookingSticket sticket = new BookingSticket();
                        sticket.BookingSticketID = row["MA_PHIEU"].ToString();
                        sticket.FlightID = row["MA_CHUYEN_BAY"].ToString();
                        sticket.SticketTypeID = row["MA_LOAI_HANG_GHE"].ToString();
                        sticket.PassengerName = row["TEN_HANH_KHACH"].ToString();
                        sticket.CMND = row["CMND"].ToString();
                        sticket.Contact = row["DIEN_THOAI"].ToString();
                        sticket.Cost = row["GIA_TIEN"].ToString();
                        sticket.Address = row["DIA_CHI"].ToString();
                        sticket.BookingDate = DateTime.Parse(row["NGAY_DAT"].ToString());
                        sticket.Gmail = row["GMAIL"].ToString();
                        sticket.Request = row["YEU_CAU"].ToString();
                        BookingSticketList.Add(sticket);
                    }
                }
                return BookingSticketList;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                BookingSticketConnection.Close();
            }
        }
    }
}

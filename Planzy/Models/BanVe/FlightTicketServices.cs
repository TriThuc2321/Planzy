using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.BanVe
{
    public class FlightTicketServices
    {
        private static SqlConnection SellTicketConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        public static void Add(FlightTicket ticket, string account)
        {
            try
            {
                SellTicketConnection.Open();
                string query = string.Format("INSERT INTO VE_CHUYEN_BAY " +
                    "VALUES('{0}', '{1}', '{2}', N'{3}', '{4}', '{5}', '{6}', '{7}', '{8}', N'{9}', N'{10}')",
                    ticket.TicketId, ticket.FlightId, ticket.TicketTypeId,
                    ticket.Passenger, ticket.CMND, ticket.PhoneNumber, ticket.Cost.Replace(" VND", ""),
                    ticket.SaleDate, ticket.Gmail, ticket.Address, ticket.Request);

                SqlCommand command = new SqlCommand(query, SellTicketConnection);
                command.ExecuteNonQuery();


                query = string.Format("INSERT INTO CHI_TIET_BAN_VE " +
                   " VALUES('{0}', '{1}')",
                   ticket.TicketId, account);
                command = new SqlCommand(query, SellTicketConnection);
                command.ExecuteNonQuery();

                
                query = string.Format("UPDATE CHI_TIET_HANG_GHE SET SO_LUONG_CON_LAI = SO_LUONG_CON_LAI - 1 WHERE MA_LOAI_HANG_GHE = '{0}' and MA_CHUYEN_BAY = '{1}'",
                  ticket.TicketTypeId, ticket.FlightId);
                command = new SqlCommand(query, SellTicketConnection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                SellTicketConnection.Close();
            }
        }

        public static void Delete(FlightTicket ticket)
        {

            try
            {
                SellTicketConnection.Open();
                string query = string.Format("delete  from CHI_TIET_BAN_VE WHERE MA_VE ='{0}' " +
                    " delete  from VE_CHUYEN_BAY WHERE MA_VE = '{0}'", ticket.TicketId);

                SqlCommand command = new SqlCommand(query, SellTicketConnection);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                SellTicketConnection.Close();
            }
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static ObservableCollection<FlightTicket> GetFlightBookingList(string account)
        {
            ObservableCollection<FlightTicket> FlightTicketList = new ObservableCollection<FlightTicket>();
            try
            {
                SellTicketConnection.Open();
                #region Truy vấn dữ liệu từ sql
                string query = String.Format("Select distinct * from CHI_TIET_BAN_VE a, VE_CHUYEN_BAY b, CHUYEN_BAY c" +
                    " where a.MA_VE = b.MA_VE and b.MA_CHUYEN_BAY = c.MA_CHUYEN_BAY and MA_TAI_KHOAN = '{0}'", account);
                SqlCommand command = new SqlCommand(query, SellTicketConnection);
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                #endregion
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        FlightTicket ticket = new FlightTicket();
                        ticket.TicketId = row["MA_VE"].ToString();
                        ticket.FlightId = row["MA_CHUYEN_BAY"].ToString();
                        ticket.TicketTypeId = row["MA_LOAI_HANG_GHE"].ToString();
                        ticket.Passenger = row["TEN_HANH_KHACH"].ToString();
                        ticket.CMND = row["CMND"].ToString();
                        ticket.PhoneNumber = row["DIEN_THOAI"].ToString();
                        ticket.Cost = row["GIA_TIEN"].ToString();
                        ticket.Address = row["DIA_CHI"].ToString();
                        ticket.SaleDate = DateTime.Parse(row["NGAY_DAT"].ToString());
                        ticket.Gmail = row["GMAIL"].ToString();
                        ticket.Request = row["YEU_CAU"].ToString();
                        FlightTicketList.Add(ticket);
                    }
                }
                return FlightTicketList;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                SellTicketConnection.Close();
            }
        }
    }
}

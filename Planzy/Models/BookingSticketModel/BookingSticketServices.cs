using System;
using System.Collections.Generic;
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
        public static void BookingSticketProcess(BookingSticket sticket)
        {

            try
            {
                BookingSticketConnection.Open();
                string query = string.Format("INSERT INTO PHIEU_DAT_CHO " +
                    "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')",
                    sticket.BookingSticketID, sticket.FlightID, sticket.SticketTypeID,
                    sticket.PassengerName, sticket.CMND, sticket.Contact, sticket.Cost,
                    sticket.BookingDate, sticket.Gmail, sticket.Address, sticket.Request);

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
    }
}

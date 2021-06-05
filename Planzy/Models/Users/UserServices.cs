using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.Users
{
    class UserServices
    {
        private static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        private static List<User> listUsers;
        public UserServices()
        {
            listUsers = new List<User>();
            getUsers();
        }
        public List<User> GetAll()
        {
            return listUsers;
        }

        public void pushUserToSql(User user)
        {
            string query = "INSERT INTO HANH_KHACH (MA_TAI_KHOAN, GMAIL, MAT_KHAU, TEN_HANH_KHACH, CMND, SDT, DIA_CHI) VALUES(@Mataikhoan, @Gmail, @Matkhau, @Tenhanhkhach, @CMND, @SDT, @Diachi)";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Mataikhoan", user.ID);
            command.Parameters.AddWithValue("@Gmail", user.Gmail);
            command.Parameters.AddWithValue("@Matkhau", user.Password);
            command.Parameters.AddWithValue("@Tenhanhkhach", user.Name);
            command.Parameters.AddWithValue("@CMND", user.CMND);
            command.Parameters.AddWithValue("@SDT", user.PhoneNumer);
            command.Parameters.AddWithValue("@Diachi", user.Address);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {

            }
            finally
            {
                connection.Close();
            }
        }


        public void getUsers()
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM HANH_KHACH", connection);
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    foreach(DataRow dataRow in dataTable.Rows)
                    {
                        User temp = new User();
                        temp.ID = dataRow["MA_TAI_KHOAN"].ToString();
                        temp.Gmail = dataRow["GMAIL"].ToString();
                        temp.Name = dataRow["TEN_HANH_KHACH"].ToString();
                        temp.Password = dataRow["MAT_KHAU"].ToString();
                        temp.PhoneNumer = dataRow["SDT"].ToString();
                        temp.CMND = dataRow["CMND"].ToString();
                        listUsers.Add(temp);
                    }
                }
                
            }
            catch (Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public User getUserById(string id)
        {
            for (int i = 0; i < listUsers.Count(); i++)
            {
                if (listUsers[i].ID == id) return listUsers[i];
            }
            return null;
        }
        public User getUserByEmail(string mail)
        {
            for (int i = 0; i < listUsers.Count(); i++)
            {
                if (listUsers[i].Gmail == mail) return listUsers[i];
            }
            return null;
        }
        public string getIdUserDefault()
        {
            int i = listUsers.Count();

            while (true)
            {
                int k = 0;
                for ( k = 0; k < listUsers.Count(); k++)
                {
                    if (listUsers[k].ID == ("HK" + i))
                    {
                        i++;
                        break;
                    }
                }
                if (k == listUsers.Count()) break;
            }
            
            return "HK" + i;
        }
        public bool ExistEmail(string email)
        {
            for(int i=0; i<listUsers.Count; i++)
            {
                if (email == listUsers[i].Gmail) return true;
            }
            return false;
        }
        public bool ExistId(string id)
        {
            for (int i = 0; i < listUsers.Count; i++)
            {
                if (id == listUsers[i].ID) return true;
            }
            return false;
        }

        public string Encode(string stringValue)
        {
            MD5 mh = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(stringValue);
            byte[] hash = mh.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}

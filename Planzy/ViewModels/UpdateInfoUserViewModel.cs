using Planzy.Commands;
using Planzy.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Planzy.ViewModels
{
    
    public class UpdateInfoUserViewModel : INotifyPropertyChanged
    {
        #region onpropertychange
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);

        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }

        UserServices userServices;
        User user;

        public UpdateInfoUserViewModel(User _user)
        {
            Save = new RelayCommand2<Window>((p) => { return true; }, (p) => { save(p); });
            Cancel = new RelayCommand2<Window>((p) => { return true; }, (p) => { p.Close(); });

            userServices = new UserServices();
            user = _user;
            setUI();
        }

        void save(Window p)
        {
            if(InvalidCMNDVisibility == "Collapsed" && InvalidPhoneNumberVisibility == "Collapsed")
            {               
                update();
                p.Close();
            }
        }

        void setUI()
        {
            UserName = user.Name;
            Gmail = user.Gmail;
            CMND = user.CMND;
            PhoneNumber = user.PhoneNumer;
            Address = user.Address;

            InvalidCMNDVisibility = "Collapsed";
            InvalidPhoneNumberVisibility = "Collapsed";
        }

        void update()
        {
            string query = "UPDATE HANH_KHACH SET TEN_HANH_KHACH = @Tenhanhkhach, CMND = @CMND, SDT = @SDT, DIA_CHI = @Diachi WHERE GMAIL = @gmail";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Gmail", Gmail);

            if(UserName==null || UserName == "")
            {
                command.Parameters.AddWithValue("@Tenhanhkhach", "");
            }
            else
            {
                command.Parameters.AddWithValue("@Tenhanhkhach", UserName);
            }

            if (CMND == null || CMND == "")
            {
                command.Parameters.AddWithValue("@CMND", "");
            }
            else
            {
                command.Parameters.AddWithValue("@CMND", CMND);
            }

            if (PhoneNumber == null || PhoneNumber == "")
            {
                command.Parameters.AddWithValue("@SDT", "");
            }
            else
            {
                command.Parameters.AddWithValue("@SDT", PhoneNumber);
            }

            if (Address == null || Address == "")
            {
                command.Parameters.AddWithValue("@Diachi", "");
            }
            else
            {
                command.Parameters.AddWithValue("@Diachi", Address);
            }


            
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




        private string invalidCMNDVisibility;
        public string InvalidCMNDVisibility
        {
            get { return invalidCMNDVisibility; }
            set
            {
                invalidCMNDVisibility = value;
                OnPropertyChanged("InvalidCMNDVisibility");                  
            }
        }

        private string invalidPhoneNumberVisibility;
        public string InvalidPhoneNumberVisibility
        {
            get { return invalidPhoneNumberVisibility; }
            set
            {
                invalidPhoneNumberVisibility = value;
                OnPropertyChanged("InvalidPhoneNumberVisibility");
            }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }
        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (userServices.IsPhoneNumber(value))
                {
                    phoneNumber = value;
                    InvalidPhoneNumberVisibility = "Collapsed";
                }
                else
                {
                    InvalidPhoneNumberVisibility = "Visible";
                }
                OnPropertyChanged("PhoneNumber");
            }
        }
        private string cmnd;
        public string CMND
        {
            get { return cmnd; }
            set
            {
                if (userServices.IsCMND(value))
                {
                    cmnd = value;
                    InvalidCMNDVisibility = "Collapsed";
                }
                else
                {
                    InvalidCMNDVisibility = "Visible";
                }
                
                OnPropertyChanged("CMND");
            }
        }

        private string gmail;
        public string Gmail
        {
            get { return gmail; }
            set
            {
                gmail = value;
                OnPropertyChanged("CMND");
            }
        }
        private string address;
        public string Address
        {
            get { return address; }
            set { address = value; OnPropertyChanged("Address"); }
        }

        
    }
}

using Planzy.Commands;
using Planzy.Models.Users;
using Planzy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
        public ICommand LoadWindowCommand { get; set; }

        UserServices userServices;
        User user;
        Window parentView;
        Window mainWindow;
        private DispatcherTimer timer;
        private DispatcherTimer timerText;


        public UpdateInfoUserViewModel(User _user, Window main)
        {
            mainWindow = main;

            Save = new RelayCommand2<Window>((p) => { return true; }, (p) => { save(p); });
            Cancel = new RelayCommand2<Window>((p) => { return true; }, (p) => { p.Close(); });
            LoadWindowCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { this.parentView = p; });


            userServices = new UserServices();
            user = _user;
            setUI();

            timerText = new DispatcherTimer();
            timerText.Interval = TimeSpan.FromSeconds(1);
            timerText.Tick += timerText_Tick;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void save(Window p)
        {
            if (!userServices.IsCMND(CMND) || !userServices.IsPhoneNumber(PhoneNumber))
            {
                TextMessageVisibility = "Collapsed";
            }
            else
            {
                TextMessageVisibility = "Visible";
                update();
                timerText.Start();
            }

        }
        private void timerText_Tick(object sender, EventArgs e)
        {
            timerText.Stop();
            parentView.Close();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!IsConnectedToInternet())
            {
                timer.Stop();
                InternetCheckingView internetCheckingView = new InternetCheckingView(parentView, mainWindow);
                internetCheckingView.ShowDialog();
                timer.Start();
            }
        }

        public bool IsConnectedToInternet()
        {
            try
            {
                IPHostEntry i = Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch
            {
                return false;
            }
        }

        void setUI()
        {
            UserName = user.Name;
            Gmail = user.Gmail;
            CMND = user.CMND;
            PhoneNumber = user.PhoneNumer;
            Address = user.Address.Trim();

            InvalidCMNDVisibility = "Collapsed";
            InvalidPhoneNumberVisibility = "Collapsed";
            TextMessageVisibility = "Collapsed";
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

        private string textMessageVisibility;
        public string TextMessageVisibility
        {
            get { return textMessageVisibility; }
            set
            {
                textMessageVisibility = value;
                OnPropertyChanged("TextMessageVisibility");
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
                    InvalidPhoneNumberVisibility = "Collapsed";
                }
                else
                {
                    InvalidPhoneNumberVisibility = "Visible";
                }
                phoneNumber = value;
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
                    InvalidCMNDVisibility = "Collapsed";
                }
                else
                {
                    InvalidCMNDVisibility = "Visible";
                }
                cmnd = value;
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
            set {
                if (value.Length == 0) address = "";
                else address = value;
                OnPropertyChanged("Address"); }
        }

     


    }
}

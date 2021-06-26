using Planzy.Models.KiemTraModel;
using Planzy.Resources.Component.CustomMessageBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Planzy.Models.BanVe
{
    public class FlightTicket: INotifyPropertyChanged
    {
        private static SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public FlightTicket()
        {
            ticketId = null;
            flightId = null;
            ticketTypeId = null;
            departure = null;
            destination = null;
            passenger = null;
            cmnd = null;
            phoneNumber = null;
            cost = null;
            gmail = null;
            SaleDate = DateTime.Now.AddDays(0);
            flownDate = DateTime.Now.AddDays(0);
            address = null;
            request = null;

        }

        bool checkEmail(string inputEmail)
        {
            if (inputEmail == null) return false;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return true;
            else
                return false;
        }

        private string departure;

        public string Departure
        {
            get { return departure; }
            set
            {
                departure = value;
                OnPropertyChanged("Departure");
            }
        }
        private string destination;

        public string Destination
        {
            get { return destination; }
            set
            {
                destination = value;
                OnPropertyChanged("Destination");
            }
        }
        private string flightId;

        public string FlightId
        {
            get { return flightId; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraMa(value))
                        flightId = value.ToUpper();
                }
                OnPropertyChanged("FlightId");
            }
        }
        private string ticketId;

        public string TicketId
        {
            get { return ticketId; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.CheckBookingSticketID(value))
                        ticketId = value.ToUpper();
                }
                OnPropertyChanged("TicketId");
            }
        }
        private string ticketTypeId;

        public string TicketTypeId
        {
            get { return ticketTypeId; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.CheckBookingSticketID(value))
                        ticketTypeId = value.ToUpper();
                }
                OnPropertyChanged("TicketTypeId");
            }
        }

        private string passenger;

        public string Passenger
        {
            get { return passenger; }
            set
            {
                passenger = value;
                OnPropertyChanged("Passenger");
            }
        }

        private string cmnd;

        public string CMND
        {
            get { return cmnd; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                        cmnd = value;
                    else
                        CustomMessageBox.Show("CMND không hợp lệ", "Nhắc nhở", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                OnPropertyChanged("CMND");
            }
        }
        private string phoneNumber;

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                        phoneNumber = value;
                    else
                        CustomMessageBox.Show("Số điện thoại không hợp lệ", "Nhắc nhở", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                OnPropertyChanged("PhoneNumber");
            }
        }

        private string cost;

        public string Cost
        {
            get { return cost; }
            set
            {
                if (value != null)
                {
                    //if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    cost = value.ToUpper();
                }
                OnPropertyChanged("Cost");
            }
        }

        private DateTime saleDate = DateTime.Now.AddDays(0);

        public DateTime SaleDate
        {
            get { return saleDate; }
            set { saleDate = value; OnPropertyChanged("SaleDate"); }
        }
        private DateTime flownDate = DateTime.Now.AddDays(0);

        public DateTime FlownDate
        {
            get { return flownDate; }
            set { flownDate = value; OnPropertyChanged("FlownDate"); }
        }
        private string gmail;

        public string Gmail
        {
            get { return gmail; }
            set
            {                
                
                if (checkEmail(value))
                {
                    gmail = value;
                }
                else
                {
                    CustomMessageBox.Show("Email không hợp lệ", "Nhắc nhở", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                OnPropertyChanged("Gmail");
            }
        }

        private string address;

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        private string request;

        public string Request
        {
            get { return request; }
            set
            {
                request = value;
                OnPropertyChanged("Request");
            }
        }
    }
}

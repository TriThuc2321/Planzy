using Planzy.Models.ChuyenBayModel;
using Planzy.Models.KiemTraModel;
using Planzy.Resources.Component.CustomMessageBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.BookingSticketModel
{
    public class BookingSticket : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BookingSticket()
        {
            bookingSticketID = null;
            flightID = null;
            sticketTypeID = null;
            departure = null;
            destination = null;
            passengerName = null;
            cmnd = null;
            contact = null;
            cost = null;
            gmail = null;
            bookingDate = DateTime.Now.AddDays(0);
            flownDate = DateTime.Now.AddDays(0);
            address = null;
            request = null;
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
        private string bookingSticketID;

        public string BookingSticketID
        {
            get { return bookingSticketID; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.CheckBookingSticketID(value))
                        bookingSticketID = value.ToUpper();
                }
                OnPropertyChanged("BookingSticketID");
            }
        }
        private string flightID;

        public string FlightID
        {
            get { return flightID; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraMa(value))
                        flightID = value.ToUpper();
                }
                OnPropertyChanged("FlightID");
            }
        }
        private string sticketTypeID;

        public string SticketTypeID
        {
            get { return sticketTypeID; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.CheckBookingSticketID(value))
                        sticketTypeID = value.ToUpper();
                }
                OnPropertyChanged("SticketTypeID");
            }
        }

        private string passengerName;

        public string PassengerName
        {
            get { return passengerName; }
            set
            {
                passengerName = value.ToUpper();
                OnPropertyChanged("PassengerName");
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
                    if (KiemTraHopLeInput.IsCMND(value))
                        cmnd = value;
                    else
                        CustomMessageBox.Show("CMND không hợp lệ", "Nhắc nhở", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                OnPropertyChanged("CMND");
            }
        }
        private string contact;

        public string Contact
        {
            get { return contact; }
            set
            {
                if (value != null)
                {
                    if (value != null)
                    {
                        if (KiemTraHopLeInput.IsPhoneNumber(value))
                            contact = value;
                        else
                            CustomMessageBox.Show("Số điện thoại không hợp lệ", "Nhắc nhở", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }
                OnPropertyChanged("Contact");
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

        private DateTime bookingDate = DateTime.Now.AddDays(0);

        public DateTime BookingDate
        {
            get { return bookingDate; }
            set { bookingDate = value; OnPropertyChanged("BookingDate"); }
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

                if (KiemTraHopLeInput.IsEmail(value))
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
                if (value != null)
                {                    
                    address = value.ToUpper();
                }
                OnPropertyChanged("Address");
            }
        }

        private string request;

        public string Request
        {
            get { return request; }
            set
            {
                if (value != null)
                {
                        request = value.ToUpper();
                }
                OnPropertyChanged("Request");
            }
        }


    }


}
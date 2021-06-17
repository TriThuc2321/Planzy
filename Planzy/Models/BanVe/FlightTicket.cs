using Planzy.Models.KiemTraModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.BanVe
{
    class FlightTicket: INotifyPropertyChanged
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
            SaleDate = DateTime.UtcNow.AddDays(0);
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
        private string flightId;

        public string FlightId
        {
            get { return flightId; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.CheckBookingSticketID(value))
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
                    if (KiemTraHopLeInput.KiemTraMa(value))
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
                        cmnd = value.ToUpper();
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
                        phoneNumber = value.ToUpper();
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

        private DateTime saleDate = DateTime.UtcNow.AddDays(0);

        public DateTime SaleDate
        {
            get { return saleDate; }
            set { saleDate = value; OnPropertyChanged("SaleDate"); }
        }

        private string gmail;

        public string Gmail
        {
            get { return gmail; }
            set
            {
                gmail = value;
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
                    if (KiemTraHopLeInput.CheckAddress(value))
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
                    if (KiemTraHopLeInput.CheckAddress(value))
                        request = value.ToUpper();
                }
                OnPropertyChanged("Request");
            }
        }
    }
}

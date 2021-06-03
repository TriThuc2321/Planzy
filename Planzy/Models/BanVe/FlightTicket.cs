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

        private string ticketId;
        private string flightId;
        private string ticketClassId;
        private string ticketClassName;
        private string passenger;
        private string cmnd;
        private string phoneNumber;
        private string fare;

        FlightTicket()
        {
            ticketId = null;
            flightId = null;
            ticketClassId = null;
            ticketClassName = null;
            passenger = null;
            cmnd = null;
            phoneNumber = null;
            fare = null;

        }

        private DateTime flightDate = DateTime.UtcNow.AddDays(1);

        public DateTime FightDate
        {
            get { return flightDate; }
            set { flightDate = value; OnPropertyChanged("FlightDate"); }
        }

        public string TicketId {
            get
            { return ticketId; }
            set {
                ticketId = value;
                OnPropertyChanged("TicketId");
            }
        }

        public string FlightId
        {
            get { return flightId; }
            set
            {
                flightId = value;
                OnPropertyChanged("FlightId");
            }
        }
        
        public string Passenger {
            get { return passenger; }
            set
            {
                passenger = value;
                OnPropertyChanged("Passenger");
            }
        }
        
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }

        public string CMND
        {
            get { return cmnd; }
            set
            {
                cmnd = value;
                OnPropertyChanged("CMND");
            }
        }

        public string Fare
        {
            get { return fare; }
            set
            {
                fare = value;
                OnPropertyChanged("Fare");
            }
        }

        public string TicketClassId
        {
            get { return ticketClassId; }
            set
            {
                ticketClassId = value;
                OnPropertyChanged("TicketClassId");
            }
        }
        public string TicketClassName
        {
            get { return ticketClassName; }
            set {
                ticketClassName = value;
                OnPropertyChanged("TicketClassName");
            }
        }

       
        public bool Push()
        {
            bool isPushed = false;

            return isPushed;
        }
    }
}

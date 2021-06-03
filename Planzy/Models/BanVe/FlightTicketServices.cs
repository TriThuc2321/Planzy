using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.BanVe
{
    class FlightTicketServices
    {
        private static List<FlightTicket> FlightTicketList;

        public FlightTicketServices()
        {
            FlightTicketList = new List<FlightTicket>();
        }
        public bool Add( FlightTicket newFlightTicket)
        {
            bool isAdded = false;
            if (FlightTicketList.Count == 0)
            {
                FlightTicketList.Add(newFlightTicket);
                isAdded = true;
            }
            else
            {
                if (FlightTicketList.Exists(e => e.TicketId == newFlightTicket.TicketId))
                {
                    isAdded = false;
                }
                else
                {
                    FlightTicketList.Add(newFlightTicket);
                    isAdded =  true;
                }
            }
            return isAdded;
        }

        public bool Delete(string id)
        {
            bool isDeleted = false;

            for(int i =0; i<FlightTicketList.Count(); i++)
            {
                if(FlightTicketList[i].TicketId == id)
                {
                    FlightTicketList.Remove(FlightTicketList[i]);
                    isDeleted = true;
                }
            }
            return isDeleted;
        }

        public bool Update(FlightTicket newFlightTicket)
        {
            bool isUpdated = false;
            for(int i=0; i<FlightTicketList.Count(); i++)
            {
                if(FlightTicketList[i].TicketId == newFlightTicket.TicketId)
                {
                    FlightTicketList[i] = newFlightTicket;
                    isUpdated = true;
                }
            }
            return isUpdated;
        }
    }
}

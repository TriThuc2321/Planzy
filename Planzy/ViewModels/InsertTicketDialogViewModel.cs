using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.ViewModels
{
    public class InsertTicketDialogViewModel : INotifyPropertyChanged
    {
        #region onpropertychange
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string nameTicket;
        public string NameTicket
        {
            get { return nameTicket; }
            set
            {
                nameTicket = value;
                OnPropertyChanged("NameTicket");
            }
        }
        private string ratio;
        public string Ratio
        {
            get { return ratio; }
            set
            {
                ratio = value;
                OnPropertyChanged("Ratio");
            }
        }
    }
}

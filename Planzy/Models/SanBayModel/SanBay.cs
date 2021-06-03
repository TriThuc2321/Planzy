using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Planzy.Models.SanBayModel
{
    class SanBay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }
        private string tenSanBay;

        public string TenSanBay
        {
            get { return tenSanBay; }
            set { tenSanBay = value; OnPropertyChanged("TenSanBay"); }
        }

        public override string ToString()
        {
            return Id + " - " + TenSanBay;
        }
        
    }
}

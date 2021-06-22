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
        public SanBay()
        {

        }
        public SanBay(SanBay sanBay)
        {
            Id = sanBay.id;
            TenSanBay = sanBay.tenSanBay;
            IsHoatDong = sanBay.IsHoatDong;
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
        public bool IsHoatDong;

        public override string ToString()
        {
            return Id + " - " + TenSanBay;
        }
        
    }
}

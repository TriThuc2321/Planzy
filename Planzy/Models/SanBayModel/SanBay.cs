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
        private int thoiGianDungToiThieu;

        public int ThoiGianDungToiThieu
        {
            get { return thoiGianDungToiThieu; }
            set { thoiGianDungToiThieu = value; OnPropertyChanged("ThoiGianDungToiThieu"); }
        }

        private int thoiGianDungToiDa;

        public int ThoiGianDungToiDa
        {
            get { return thoiGianDungToiDa; }
            set { thoiGianDungToiDa = value; OnPropertyChanged("ThoiGianDungToiDa"); }
        }

        public override string ToString()
        {
            return Id + " - " + TenSanBay;
        }
    }
}

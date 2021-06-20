using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace Planzy.Models.LoaiHangGheModel
{
    public class LoaiHangGhe : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LoaiHangGhe()
        {
            MaLoaiHangGhe = "";
            TenLoaiHangGhe = "";
            TyLe = "";
        }
        private string maLoaiHangGhe;

        public string MaLoaiHangGhe
        {
            get { return maLoaiHangGhe; }
            set { maLoaiHangGhe = value; OnPropertyChanged("MaLoaiHangGhe"); }
        }
        private string tenLoaiHangGhe;

        public string TenLoaiHangGhe
        {
            get { return tenLoaiHangGhe; }
            set { tenLoaiHangGhe = value; OnPropertyChanged("TenLoaiHangGhe"); }
        }
        private string tyLe;

        public string TyLe
        {
            get { return tyLe; }
            set { tyLe = value; OnPropertyChanged("TyLe"); }
        }

    }
}

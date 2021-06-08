using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace Planzy.Models.ChiTietHangGheModel
{
    class ChiTietHangGhe : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string  maChuyenBay;

        public ChiTietHangGhe()
        {

        }
        public ChiTietHangGhe(string maChuyenBay, string maLoaiHangGhe, string tongSoGhe, string tenLoaiHangGhe, string tyLe)
        {
            MaChuyenBay = maChuyenBay;
            MaLoaiHangGhe = maLoaiHangGhe;
            SoLuongGhe = tongSoGhe;
            SoLuongGheConLai = tongSoGhe;
            TenLoaiHangGhe = tenLoaiHangGhe;
            TyLe = tyLe;
        }
        public string MaChuyenBay
        {
            get { return maChuyenBay; }
            set { maChuyenBay = value; OnPropertyChanged("MaChuyenBay"); }
        }
        private string maLoaiHangGhe;

        public string MaLoaiHangGhe
        {
            get { return maLoaiHangGhe; }
            set { maLoaiHangGhe = value; OnPropertyChanged("MaLoaiHangGhe"); }
        }
        private string soLuongGhe;

        public string SoLuongGhe
        {
            get { return soLuongGhe; }
            set { soLuongGhe = value; OnPropertyChanged("SoLuongGhe"); }
        }

        private string soLuongGheConLai;

        public string SoLuongGheConLai
        {
            get { return soLuongGheConLai; }
            set { soLuongGheConLai = value; OnPropertyChanged("SoLuongGheConLai"); }
        }

        private string tyLe;

        public string TyLe
        {
            get { return tyLe; }
            set { tyLe = value; OnPropertyChanged("TyLe"); }
        }

        private string tenLoaiHangGhe;

        public string TenLoaiHangGhe
        {
            get { return tenLoaiHangGhe; }
            set { tenLoaiHangGhe = value;OnPropertyChanged("TenLoaiHangGhe"); }
        }

    }
}

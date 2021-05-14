using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Planzy.Models.SanBayModel;
namespace Planzy.Models.SanBayTrungGianModel
{
    class SanBayTrungGian : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string maChuyenBay;

        public string MaChuyenBay
        {
            get { return maChuyenBay; }
            set { maChuyenBay = value; OnPropertyChanged("MaChuyenBay"); }
        }

        private string maSanBay;

        public string MaSanBay
        {
            get { return maSanBay; }
            set { maSanBay = value; OnPropertyChanged("MaSanBay"); }
        }

        private string maSanBayTruoc;

        public string MaSanBayTruoc
        {
            get { return maSanBayTruoc; }
            set { maSanBayTruoc = value; OnPropertyChanged("MaSanBayTruoc"); }
        }

        private string maSanBaySau;

        public string MaSanBaySau
        {
            get { return maSanBaySau; }
            set { maSanBaySau = value; OnPropertyChanged("MaSanBaySau"); }
        }

        private int thoiGianDung;

        public int ThoiGianDung
        {
            get { return thoiGianDung; }
            set { thoiGianDung = value; OnPropertyChanged("ThoiGianDung"); }
        }
        private string tenSanBay;

        public string TenSanBay
        {
            get { return tenSanBay; }
            set { tenSanBay = value; OnPropertyChanged("TenSanBay"); }
        }
        public SanBayTrungGian()
        {

        }
        public SanBayTrungGian(SanBay newSanBay)
        {
            if (newSanBay != null)
            {
                MaSanBay = newSanBay.Id;
                TenSanBay = newSanBay.TenSanBay;
            }
        }
    }
}

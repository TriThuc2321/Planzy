using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Planzy.Models.SanBayModel;
using Planzy.Models.KiemTraModel;
using Planzy.Models.SanBayTrungGianModel;
using System.Collections.ObjectModel;
using System.Windows;
using Planzy.Models.ChiTietHangGheModel;

namespace Planzy.Models.ChuyenBayModel
{
    class ChuyenBay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ChuyenBay()
        {
            MaChuyenBay = "";
            GiaVeCoBan = "";
            ThoiGianBay = "";
            SanBayDi = null;
            SanBayDen = null;
            SoGheHang1 = "";
            SoGheHang2 = "";
            SoGheHang3 = "";
            SoGheHang4 = "";
            SoGheHang5 = "";
            SoGheHang6 = "";
            SoGheHang7 = "";
            SoGheHang8 = "";
            IsDaBay = false;
            SanBayTrungGian = null;
            ChiTietHangGhesList = null;
            SoLoaiHangGhe = 0;
        }

        public string MaChuyenBay { get; set; }

        //public string MaChuyenBay
        //{
        //    get { return maChuyenBay; }
        //    set 
        //    {
        //        maChuyenBay = value;
        //        OnPropertyChanged("MaChuyenBay"); 
        //    }
        //}
        private DateTime ngayBay = DateTime.UtcNow.AddDays(1);

        public DateTime NgayBay
        {
            get { return ngayBay; }
            set { ngayBay = value; OnPropertyChanged("NgayBay"); }
        }
        private string giaVeCoBan;

        public string GiaVeCoBan
        {
            get { return giaVeCoBan; }
            set 
            {
                giaVeCoBan = value;
                OnPropertyChanged("GiaVeCoBan"); 
            }
        }
        private DateTime gioBay = new DateTime(1,1,1,0,0,0);
        
        public DateTime GioBay
        {
            get { return gioBay; }
            set { gioBay = value; OnPropertyChanged("GioBay"); }
        }
        private SanBay sanBayDi;

        public SanBay SanBayDi
        {
            get { return sanBayDi; }
            set { sanBayDi = value; OnPropertyChanged("SanBayDi"); }
        }
        private SanBay sanBayDen;

        public SanBay SanBayDen
        {
            get { return sanBayDen; }
            set { sanBayDen = value; OnPropertyChanged("SanBayDen"); }
        }

        private string thoiGianBay;

        public string ThoiGianBay
        {
            get { return thoiGianBay; }
            set { thoiGianBay = value; OnPropertyChanged("ThoiGianBay"); }
        }

        private string soGheHang1;

        public string SoGheHang1
        {
            get { return soGheHang1; }
            set { soGheHang1 = value; OnPropertyChanged("SoGheHang1"); }
        }

        private string soGheHang2;

        public string SoGheHang2
        {
            get { return soGheHang2; }
            set { soGheHang2 = value; OnPropertyChanged("SoGheHang2"); }
        }
        private string soGheHang3;

        public string SoGheHang3
        {
            get { return soGheHang3; }
            set { soGheHang3 = value; OnPropertyChanged("SoGheHang3"); }
        }
        private string soGheHang4;

        public string SoGheHang4
        {
            get { return soGheHang4; }
            set { soGheHang4 = value; OnPropertyChanged("SoGheHang4"); }
        }
        private string soGheHang5;

        public string SoGheHang5
        {
            get { return soGheHang5; }
            set { soGheHang5 = value; OnPropertyChanged("SoGheHang5"); }
        }
        private string soGheHang6;

        public string SoGheHang6
        {
            get { return soGheHang6; }
            set { soGheHang6 = value; OnPropertyChanged("SoGheHang6"); }
        }
        private string soGheHang7;

        public string SoGheHang7
        {
            get { return soGheHang7; }
            set { soGheHang7 = value; OnPropertyChanged("SoGheHang7"); }
        }
        private string soGheHang8;

        public string SoGheHang8
        {
            get { return soGheHang8; }
            set { soGheHang8 = value; OnPropertyChanged("SoGheHang8"); }
        }

        private bool isDaBay;

        public bool IsDaBay
        {
            get { return isDaBay; }
            set { isDaBay = value; OnPropertyChanged("IsDaBay"); }
        }

        private  ObservableCollection<SanBayTrungGian> sanBayTrungGian;

        public ObservableCollection<SanBayTrungGian> SanBayTrungGian
        {
            get { return sanBayTrungGian; }
            set { sanBayTrungGian = value; OnPropertyChanged("SanBayTrungGian"); }
        }
        private int soLoaiHangGhe;

        public int SoLoaiHangGhe
        {
            get { return soLoaiHangGhe; }
            set { soLoaiHangGhe = value; OnPropertyChanged("SoLoaiHangGhe"); }
        }
        private List<ChiTietHangGhe> chiTietHangGhesList;

        public List<ChiTietHangGhe> ChiTietHangGhesList
        {
            get { return chiTietHangGhesList; }
            set { chiTietHangGhesList = value; OnPropertyChanged("ChiTietHangGhesList"); }
        }

    }
}

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
            MaChuyenBay = null;
            GiaVeCoBan = null;
            ThoiGianBay = null;
            SanBayDi = null;
            SanBayDen = null;
            SoGheHang1 = null;
            SoGheHang2 = null;
            SoGheHang3 = null;
            SoGheHang4 = null;
            IsDaBay = false;
            SanBayTrungGian = null;
            
        }

        private string maChuyenBay;

        public string MaChuyenBay
        {
            get { return maChuyenBay; }
            set 
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraMa(value))
                        maChuyenBay = value.ToUpper();
                }    
                OnPropertyChanged("MaChuyenBay"); 
            }
        }
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
                if(value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        giaVeCoBan = value;
                    }    
                    else
                    {
                        MessageBox.Show("");
                    }    
                }    
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

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using Planzy.Resources.Component.CustomMessageBox;

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
            KhaDung = "";
        }
        public LoaiHangGhe(string ma,string ten)
        {
            MaLoaiHangGhe = ma;
            TenLoaiHangGhe = ten;
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
            set {
                if (IsNumber(value))
                {
                    tyLe = value;
                }
                else
                {
                    CustomMessageBox.Show("Tỷ lệ không hợp lệ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                OnPropertyChanged("TyLe"); 
            }
        }
        private string khaDung;

        public string KhaDung
        {
            get { return khaDung; }
            set { khaDung = value; OnPropertyChanged("KhaDung"); }
        }
        public bool IsNumber(string number)
        {
            for (int i = 0; i < number.Length; i++)
            {
                if (number[i] < 48 || number[i] > 57) return false;
            }
            return true;
        }
    }
}

using Planzy.Models.ChuyenBayModel;
using Planzy.Models.DoanhThuModel;
using Planzy.Models.DoanhThuThangModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace Planzy.Models.DoanhThuThangModel
{
    class DoanhThuThang: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Thang { get; set; }
        public string NgayBayString { get; set; }
        public int SoChuyenBay { get; set; }
        public int DoanhThu { get; set; }
        public float DoanhThuTrieuDong { get; set; }
        public float TyLe { get; set; }
        public DoanhThuServices doanhThuServices { get; set; }
        public DoanhThuThang(List<ChuyenBay> chuyenBays)
        {
            doanhThuServices = new DoanhThuServices(chuyenBays);
        }
        public DoanhThuThang(List<DoanhThu> doanhThus)
        {
            doanhThuServices = new DoanhThuServices(doanhThus);
        }
        public DoanhThuThang(string Thang)
        {
            this.Thang = Thang;
            doanhThuServices = new DoanhThuServices();
            SoChuyenBay = 0;
            DoanhThu = 0;
            DoanhThuTrieuDong = 0;
            TyLe = 0;
        }
    }
}

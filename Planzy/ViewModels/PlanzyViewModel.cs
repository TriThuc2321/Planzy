using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planzy.Models.KiemTraModel;
using Planzy.Models.SanBayModel;
using Planzy.Models.SanBayTrungGianModel;
using Planzy.Models.SupportUI;
using Planzy.Models.ChuyenBayModel;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Planzy.Commands;
using Planzy.Resources.Component.CustomMessageBox ;
using static Planzy.ThamSoQuyDinh;
using Planzy.Models.LoaiHangGheModel;
using FootballFieldManagement.Views;
using System.Windows;
using Planzy.Models.ChiTietHangGheModel;

namespace Planzy.ViewModels
{
    class PlanzyViewModel : INotifyPropertyChanged
    {
        #region PropertyChange
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        SanBayService sanBayServices;
        SanBayTrungGianService sanBayTrungGianService;
        ChuyenBayServices chuyenBayServices;
        LoaiHangGheServices loaiHangGheServices;
        ChiTietHangGheServices chiTietHangGheServices;
        public PlanzyViewModel()
        {
            sanBayServices = new SanBayService();
            sanBayTrungGianService = new SanBayTrungGianService();
            loaiHangGheServices = new LoaiHangGheServices();
            chiTietHangGheServices = new ChiTietHangGheServices();
            chuyenBayServices = new ChuyenBayServices(sanBayTrungGianService,sanBayServices,chiTietHangGheServices);
            ThamSoQuyDinh.LoadThamSoQuyDinhTuSQL();
            LoadData();
            doiViTriSanBayCommand = new RelayCommand(DoiViTriSanBay);
            xoaSanBayTrungGianCommand = new RelayCommand(xoaSanBayTrungGian);
            themSanBayTrungGianCommand = new RelayCommand(themSanBayTrungGian);
            huyThemCommand = new RelayCommand(huyThemSanBayTrungGian);
            xacNhanThemCommand = new RelayCommand(xacNhanThemSanBayTrungGian);
            themChuyenBayCommand = new RelayCommand(themChuyenBay);
            suaChuyenBayCommand = new RelayCommand(suaChuyenBay);
            xoaChuyenBayCommand = new RelayCommand(xoaChuyenBay);
            huyThemVaSuaChuyenBayCommand = new RelayCommand(huyThemVaSuaChuyenBay);
            luuSuaChuyenBayCommand = new RelayCommand(luuSuaChuyenBay);
            #region Xử lý giao diện ban đầu
            LoadUIHangGheTheoQuyDinh();
            chonLayoutCommand1 = new RelayCommand(Button1);
            chonLayoutCommand2 = new RelayCommand(Button2);
            chonLayoutCommand3 = new RelayCommand(Button3);
            chonLayoutCommand4 = new RelayCommand(Button4);
            chonLayoutCommand5 = new RelayCommand(Button5);
            #endregion
        }
        #region Xử lý chung
        private void LoadData()
        {
            SanbaysList = new List<SanBay>(sanBayServices.GetAll());
            SanBayDensList = new ObservableCollection<SanBay>(sanBayServices.GetAll());
            SanBayDisList = new ObservableCollection<SanBay>(sanBayServices.GetAll());
            
            SanBayTrungGianSapThemsList = new ObservableCollection<SanBay>(sanBayServices.GetAll());
            LoaiHangGhesList = loaiHangGheServices.GetAll();
            //SanBayTrungGiansList = new ObservableCollection<SanBayTrungGian>(sanBayTrungGianService.GetAll());
            ChuyenBaysList = new ObservableCollection<ChuyenBay>(chuyenBayServices.GetAll());

            
        }
        private List<SanBay> sanbaysList;

        public List<SanBay> SanbaysList
        {
            get { return sanbaysList; }
            set { sanbaysList = value; }
        }
        #endregion
        #region Xử lý chọn sân bay
        private ObservableCollection<SanBay> sanBayDisList;

        public ObservableCollection<SanBay> SanBayDisList
        {
            get { return sanBayDisList; }
            set { sanBayDisList = value; OnPropertyChanged("SanBayDisList"); }
        }

        private ObservableCollection<SanBay> sanBayDensList;

        public ObservableCollection<SanBay> SanBayDensList
        {
            get { return sanBayDensList; }
            set { sanBayDensList = value; OnPropertyChanged("SanBayDensList"); }
        }

        private SanBay sanBayDiDaChon;
        public SanBay SanBayDiDaChon
        {
            get { return sanBayDiDaChon; }
            set 
            {
                if (SanBayDiDaChon != null)
                {
                    SanBayDensList.Add(SanBayDiDaChon);
                    SanBayTrungGianSapThemsList.Add(SanBayDiDaChon);
                }
                sanBayDiDaChon = value;
                OnPropertyChanged("SanBayDiDaChon");
                //xử lý để không bị trùng sân bay đến và đi
                XoaListSanBayDen(SanBayDiDaChon);
                XoaSanBayTrungGianSapThem(SanBayDiDaChon);
                if(SanBayTrungGiansList != null && SanBayTrungGiansList.Count != 0)
                    xoaSanBayTrungGian(new SanBayTrungGian(SanBayDiDaChon));
            }
        }
        private SanBay sanBayDenDaChon;
        public SanBay SanBayDenDaChon
        {
            get { return sanBayDenDaChon; }
            set
            {
                if (SanBayDenDaChon != null)
                {
                    SanBayDisList.Add(SanBayDenDaChon);
                    SanBayTrungGianSapThemsList.Add(SanBayDenDaChon);
                }
                sanBayDenDaChon = value;
                OnPropertyChanged("SanBayDenDaChon");
                //xử lý để không bị trùng sân bay đến và đi
                XoaListSanBayDi(SanBayDenDaChon);
                XoaSanBayTrungGianSapThem(SanBayDenDaChon);
                if (SanBayTrungGiansList != null && SanBayTrungGiansList.Count != 0)
                    xoaSanBayTrungGian( new SanBayTrungGian(SanBayDenDaChon));
            }
        }
        public void XoaListSanBayDi(SanBay sanBay)
        {
            for(int i = 0; i< SanBayDisList.Count;i++)
            {
                if (sanBay.Id == SanBayDisList[i].Id)
                {
                    SanBayDisList.RemoveAt(i);
                    break;
                }    
            }    
        }
        public void XoaSanBayTrungGianSapThem(SanBay sanBay)
        {
            for (int i = 0; i < SanBayTrungGianSapThemsList.Count; i++)
            {
                if (sanBay.Id == SanBayTrungGianSapThemsList[i].Id)
                {
                    SanBayTrungGianSapThemsList.RemoveAt(i);
                    break;
                }
            }
        }
        public void XoaListSanBayDen(SanBay sanBay)
        {
            for (int i = 0; i < SanBayDensList.Count; i++)
            {
                if (sanBay.Id == SanBayDensList[i].Id)
                {
                    SanBayDensList.RemoveAt(i);
                    break;
                }
            }
        }
        private RelayCommand doiViTriSanBayCommand;

        public RelayCommand DoiViTriSanBayCommand
        {
            get { return doiViTriSanBayCommand; }
        }

        public void DoiViTriSanBay()
        {
            SanBay temp = new SanBay();
            if (SanBayDenDaChon != null && SanBayDiDaChon != null)
            {
                temp.TenSanBay = SanBayDenDaChon.TenSanBay;
                temp.Id = SanBayDenDaChon.Id;
                SanBayDenDaChon.Id = SanBayDiDaChon.Id;
                SanBayDenDaChon.TenSanBay = SanBayDiDaChon.TenSanBay;
                SanBayDiDaChon.Id = temp.Id;
                SanBayDiDaChon.TenSanBay = temp.TenSanBay;            
            }
            IsFocusGiaChuyenBay = "True";
        }
        #endregion
        #region Xử lý giao diện ban đầu
        private ButtonDuocChon duocChon = new ButtonDuocChon(true);

        public ButtonDuocChon DuocChon
        {
            get { return duocChon; }
            set { duocChon = value;}
        }
        private ButtonDuocChon khongDuocChon = new ButtonDuocChon(false);
        public ButtonDuocChon KhongDuocChon
        {
            get { return khongDuocChon; }
            set { khongDuocChon = value; }
        }
        private ButtonDuocChon isDuocChon1 = new ButtonDuocChon(true);

        public ButtonDuocChon IsDuocChon1
        {
            get { return isDuocChon1; }
            set { isDuocChon1 = value; OnPropertyChanged("IsDuocChon1"); }
        }
        private ButtonDuocChon isDuocChon2 = new ButtonDuocChon(false);

        public ButtonDuocChon IsDuocChon2
        {
            get { return isDuocChon2; }
            set { isDuocChon2 = value; OnPropertyChanged("IsDuocChon2"); }
        }

        private ButtonDuocChon isDuocChon3 = new ButtonDuocChon(false);

        public ButtonDuocChon IsDuocChon3
        {
            get { return isDuocChon3; }
            set { isDuocChon3 = value; OnPropertyChanged("IsDuocChon3"); }
        }

        private ButtonDuocChon isDuocChon4 = new ButtonDuocChon(false);

        public ButtonDuocChon IsDuocChon4
        {
            get { return isDuocChon4; }
            set { isDuocChon4 = value; OnPropertyChanged("IsDuocChon4"); }
        }
        private ButtonDuocChon isDuocChon5 = new ButtonDuocChon(false);

        public ButtonDuocChon IsDuocChon5
        {
            get { return isDuocChon5; }
            set { isDuocChon5 = value; OnPropertyChanged("IsDuocChon5"); }
        }

        private RelayCommand chonLayoutCommand1;

        public RelayCommand ChonLayoutCommand1
        {
            get { return chonLayoutCommand1; }
        }
        private RelayCommand chonLayoutCommand2;

        public RelayCommand ChonLayoutCommand2
        {
            get { return chonLayoutCommand2; }
        }
        private RelayCommand chonLayoutCommand3;

        public RelayCommand ChonLayoutCommand3
        {
            get { return chonLayoutCommand3; }
        }
        private RelayCommand chonLayoutCommand4;

        public RelayCommand ChonLayoutCommand4
        {
            get { return chonLayoutCommand4; }
        }
        private RelayCommand chonLayoutCommand5;

        public RelayCommand ChonLayoutCommand5
        {
            get { return chonLayoutCommand5; }
        }
        public void Button1()
        {
            IsDuocChon1 = DuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
        }
        public void Button2()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = DuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
        }
        public void Button3()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = DuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
        }
        public void Button4()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = DuocChon;
            IsDuocChon5 = KhongDuocChon;
        }
        public void Button5()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = DuocChon;
        }
        #endregion
        #region Xử lý xóa sân bay trung gian
        private RelayCommand xoaSanBayTrungGianCommand;

        public RelayCommand XoaSanBayTrungGianCommand
        {
            get { return xoaSanBayTrungGianCommand; }
        }
        public void xoaSanBayTrungGian(object sanBayBiXoa)
        {
            for (int i = 0; i<SanBayTrungGiansList.Count; i++)
            {
                if (((SanBayTrungGian)sanBayBiXoa).MaSanBay == SanBayTrungGiansList[i].MaSanBay)
                {
                    if (SanBayTrungGiansList[i].MaSanBay != SanBayDenDaChon.Id && SanBayTrungGiansList[i].MaSanBay != SanBayDiDaChon.Id)
                    {
                        SanBay temp = new SanBay();
                        temp.Id = SanBayTrungGiansList[i].MaSanBay;
                        temp.TenSanBay = SanBayTrungGiansList[i].TenSanBay;
                        SanBayTrungGianSapThemsList.Add(temp);
                    }
                    if (i == 0)
                    {
                        if (SanBayTrungGiansList.Count == 1)
                        {
                            SanBayTrungGiansList.RemoveAt(i);
                            return;
                        }
                        else
                        {
                            SanBayTrungGiansList[1].MaSanBayTruoc = SanBayDiDaChon.Id;
                            SanBayTrungGiansList.RemoveAt(i);
                            return;
                        }                        
                    }
                    else if (i != SanBayTrungGiansList.Count -1)
                    {
                        SanBayTrungGiansList[i - 1].MaSanBaySau = SanBayTrungGiansList[i + 1].MaSanBay;
                        SanBayTrungGiansList[i + 1].MaSanBayTruoc = SanBayTrungGiansList[i - 1].MaSanBay;
                        SanBayTrungGiansList.RemoveAt(i);
                        return;
                    } 
                    else
                    {
                        SanBayTrungGiansList[i-1].MaSanBaySau = SanBayDenDaChon.Id;
                        SanBayTrungGiansList.RemoveAt(i);
                        return;
                    }    
                }    
            }    
        }
        private ObservableCollection<SanBayTrungGian> sanBayTrungGiansList = new ObservableCollection<SanBayTrungGian>();

        public ObservableCollection<SanBayTrungGian> SanBayTrungGiansList
        {
            get { return sanBayTrungGiansList; }
            set 
            {
                bool isCheck;
                if(SanBayTrungGiansList == null)
                {
                    isCheck = true;
                }    
                else
                {
                    isCheck = false;
                }    
                sanBayTrungGiansList = value;
                if (isCheck && sanBayTrungGiansList != null)
                {
                    for (int i = 0;i< sanBayTrungGiansList.Count;i++)
                    {
                        for (int j = 0;j< SanBayTrungGianSapThemsList.Count; j++)
                        {
                            if (sanBayTrungGiansList[i].MaSanBay == SanBayTrungGianSapThemsList[j].Id)
                                SanBayTrungGianSapThemsList.RemoveAt(j);
                        }    
                    }    
                }    
                OnPropertyChanged("SanBayTrungGiansList"); 
            }
        }
        #endregion
        #region Xử lý chèn sân bay trung gian
        private string isVisible = "Hidden";

        public string IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; OnPropertyChanged("IsVisible"); }
        }
        private string isDropDown = "False";

        public string IsDropDown
        {
            get { return isDropDown; }
            set { isDropDown = value; OnPropertyChanged("IsDropDown"); }
        }

        private RelayCommand themSanBayTrungGianCommand;

        public RelayCommand ThemSanBayTrungGianCommand
        {
            get { return themSanBayTrungGianCommand; }
        }

        private RelayCommand huyThemCommand;

        public RelayCommand HuyThemCommand
        {
            get { return huyThemCommand; }
        }
        private RelayCommand xacNhanThemCommand;

        public RelayCommand XacNhanThemCommand
        {
            get { return xacNhanThemCommand; }
        }
        private SanBay sanBayTrungGianSapThem;
        public SanBay SanBayTrungGianSapThem
        {
            get { return sanBayTrungGianSapThem; }
            set { sanBayTrungGianSapThem = value; OnPropertyChanged("SanBayTrungGianSapThem"); }
        }

        private ObservableCollection<SanBay> sanBayTrungGianSapThemsList;
        public ObservableCollection<SanBay> SanBayTrungGianSapThemsList
        {
            get { return sanBayTrungGianSapThemsList; }
            set { sanBayTrungGianSapThemsList = value; OnPropertyChanged("SanBayTrungGianSapThemsList"); }
        }
        public void themSanBayTrungGian(object sanBayBenDuoiSanBaySapThem)
        {
            if (SanBayDenDaChon != null && SanBayDiDaChon != null)
            {
                #region Kiểm tra quy định
                if (SanBayTrungGiansList.Count == Convert.ToInt32(ThamSoQuyDinh.SO_SAN_BAY_TRUNG_GIAN_TOI_DA))
                {
                    CustomMessageBox.Show("Tối đa " + ThamSoQuyDinh.SO_SAN_BAY_TRUNG_GIAN_TOI_DA + " sân bay trung gian", "Nhắc nhở");
                    return;
                }
                #endregion
                IsDropDown = "True";
                IsVisible = "Visible";
                this.sanBayBenDuoiSanBaySapThem = sanBayBenDuoiSanBaySapThem;
            }
        }

        public void huyThemSanBayTrungGian()
        {
            IsDropDown = "False";
            IsVisible = "Hidden";
            SanBayTrungGianSapThem = null;
        }
        object sanBayBenDuoiSanBaySapThem;
        public void xacNhanThemSanBayTrungGian()
        {
            if (ThoiGianDungSapThem == null || ThoiGianDungSapThem =="" )
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc nhở");
                IsFocusThoiGianDung = "True";
                return;
            }    

            int ViTriSanBayTrungGianDuocThem = 0;
            SanBayTrungGian SanBayTrungGianDuocChon = new SanBayTrungGian();
            if (SanBayTrungGiansList == null)
                SanBayTrungGiansList = new ObservableCollection<SanBayTrungGian>();    
            int index;
            if (sanBayBenDuoiSanBaySapThem == null)
            {
                if (SanBayTrungGiansList.Count == 0)
                {
                    SanBayTrungGian newSanBay = new SanBayTrungGian();
                    newSanBay.MaChuyenBay = MaChuyenBay;
                    newSanBay.MaSanBay = SanBayTrungGianSapThem.Id;
                    newSanBay.TenSanBay = SanBayTrungGianSapThem.TenSanBay;
                    newSanBay.MaSanBayTruoc = SanBayDiDaChon.Id;
                    newSanBay.MaSanBaySau = SanBayDenDaChon.Id;
                    newSanBay.ThoiGianDung = ThoiGianDungSapThem;

                    SanBayTrungGiansList.Add(newSanBay);
                    SanBayTrungGianSapThemsList.Remove(SanBayTrungGianSapThem);
                    SanBayTrungGianSapThem = null;
                    IsDropDown = "False";
                    IsVisible = "Hidden";
                }
                else
                {
                    SanBayTrungGian newSanBay = new SanBayTrungGian();
                    newSanBay.MaChuyenBay = MaChuyenBay;
                    newSanBay.MaSanBay = SanBayTrungGianSapThem.Id;
                    newSanBay.TenSanBay = SanBayTrungGianSapThem.TenSanBay;
                    newSanBay.MaSanBayTruoc = SanBayTrungGiansList[SanBayTrungGiansList.Count - 1].MaSanBay;
                    newSanBay.MaSanBaySau = SanBayDenDaChon.Id;
                    SanBayTrungGiansList[SanBayTrungGiansList.Count - 1].MaSanBaySau = newSanBay.MaSanBay;
                    newSanBay.ThoiGianDung = ThoiGianDungSapThem;

                    SanBayTrungGiansList.Add(newSanBay);
                    SanBayTrungGianSapThemsList.Remove(SanBayTrungGianSapThem);
                    SanBayTrungGianSapThem = null;
                    IsDropDown = "False";
                    IsVisible = "Hidden";
                }
                return;
            }
            else
            {
                for (index = 0; index < SanBayTrungGiansList.Count; index++)
                {
                    if (((SanBayTrungGian)sanBayBenDuoiSanBaySapThem).MaSanBay == SanBayTrungGiansList[index].MaSanBay)
                    {
                        if (index == 0)
                        {
                            SanBayTrungGian newSanBay = new SanBayTrungGian();
                            newSanBay.MaChuyenBay = MaChuyenBay;
                            newSanBay.MaSanBay = SanBayTrungGianSapThem.Id;
                            newSanBay.TenSanBay = SanBayTrungGianSapThem.TenSanBay;
                            newSanBay.MaSanBayTruoc = SanBayDiDaChon.Id;
                            newSanBay.MaSanBaySau = SanBayTrungGiansList[index].MaSanBay;
                            newSanBay.ThoiGianDung = ThoiGianDungSapThem;

                            ViTriSanBayTrungGianDuocThem = index;
                            SanBayTrungGianDuocChon = newSanBay;
                            break;
                        }
                        else if (index != SanBayTrungGiansList.Count - 1)
                        {
                            SanBayTrungGian newSanBay = new SanBayTrungGian();
                            newSanBay.MaChuyenBay = MaChuyenBay;
                            newSanBay.MaSanBay = SanBayTrungGianSapThem.Id;
                            newSanBay.TenSanBay = SanBayTrungGianSapThem.TenSanBay;
                            newSanBay.MaSanBayTruoc = SanBayTrungGiansList[index - 1].MaSanBay;
                            newSanBay.MaSanBaySau = SanBayTrungGiansList[index].MaSanBay;
                            newSanBay.ThoiGianDung = ThoiGianDungSapThem;

                            ViTriSanBayTrungGianDuocThem = index;
                            SanBayTrungGianDuocChon = newSanBay;
                            break;
                        }
                        else
                        {
                            SanBayTrungGian newSanBay = new SanBayTrungGian();
                            newSanBay.MaChuyenBay = MaChuyenBay;
                            newSanBay.MaSanBay = SanBayTrungGianSapThem.Id;
                            newSanBay.TenSanBay = SanBayTrungGianSapThem.TenSanBay;
                            newSanBay.MaSanBayTruoc = SanBayTrungGiansList[index - 1].MaSanBay;
                            newSanBay.MaSanBaySau = SanBayDenDaChon.Id;
                            newSanBay.ThoiGianDung = ThoiGianDungSapThem;

                            ViTriSanBayTrungGianDuocThem = index;
                            SanBayTrungGianDuocChon = newSanBay;
                            break;
                        }
                    }
                }
            }
            SanBayTrungGiansList.Insert(ViTriSanBayTrungGianDuocThem, SanBayTrungGianDuocChon);
            

            SanBayTrungGianSapThemsList.Remove(SanBayTrungGianSapThem);
            SanBayTrungGianSapThem = null;
            IsDropDown = "False";
            IsVisible = "Hidden";
        }
        private string thoiGianDungSapThem;

        public string ThoiGianDungSapThem
        {
            get { return thoiGianDungSapThem; }
            set 
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        #region Kiểm tra quy định
                        if (Convert.ToInt32 (value) < Convert.ToInt32(ThamSoQuyDinh.THOI_GIAN_DUNG_TOI_THIEU) || Convert.ToInt32(value) > Convert.ToInt32(ThamSoQuyDinh.THOI_GIAN_DUNG_TOI_DA))
                        {
                            CustomMessageBox.Show("Thời gian dừng từ " + ThamSoQuyDinh.THOI_GIAN_DUNG_TOI_THIEU + " đến " + ThamSoQuyDinh.THOI_GIAN_DUNG_TOI_DA + " phút.", "Nhắc nhở.");
                        }   
                        else
                            thoiGianDungSapThem = value;
                        #endregion
                    }
                    else
                    {
                        CustomMessageBox.Show("Thời Gian Dừng phải là số nguyên dương", "Nhắc nhở");
                    }
                }
                else
                    thoiGianDungSapThem = value; 
                OnPropertyChanged("ThoiGianDungSapThem"); 
            }
        }

        #endregion
        #region Xử lý focus textbox
        private string isFocusGiaChuyenBay = "False";

        public string IsFocusGiaChuyenBay
        {
            get { return isFocusGiaChuyenBay; }
            set 
            { 
                isFocusGiaChuyenBay = value; 
                OnPropertyChanged("IsFocusGiaChuyenBay"); 
            }
        }
        private string isFocusMaChuyenBay = "False";

        public string IsFocusMaChuyenBay
        {
            get { return isFocusMaChuyenBay; }
            set
            {
                isFocusMaChuyenBay = value; 
                OnPropertyChanged("IsFocusMaChuyenBay"); 
            }
        }
        private string isFocusSanBayDi = "False";

        public string IsFocusSanBayDi
        {
            get { return isFocusSanBayDi; }
            set 
            {
                isFocusSanBayDi = value;
                OnPropertyChanged("IsFocusSanBayDi"); 
            }
        }
        private string isFocusSanBayDen = "False";
        public string IsFocusSanBayDen
        {
            get { return isFocusSanBayDen; }
            set { isFocusSanBayDen = value; OnPropertyChanged("IsFocusSanBayDen"); }
        }

        private string isFocusThoiGianDung = "False";

        public string IsFocusThoiGianDung
        {
            get { return isFocusThoiGianDung; }
            set 
            {
                isFocusThoiGianDung = value; 
                OnPropertyChanged("IsFocusThoiGianDung");
            }
        }
        private string isFocusThoiGianBay = "False";

        public string IsFocusThoiGianBay
        {
            get { return isFocusThoiGianBay; }
            set 
            {
                isFocusThoiGianBay = value;
                OnPropertyChanged("IsFocusThoiGianBay"); 
            }
        }
        private string isFocusSoGheHang4 = "False";

        public string IsFocusSoGheHang4
        {
            get { return isFocusSoGheHang4; }
            set 
            {
                isFocusSoGheHang4 = value; 
                OnPropertyChanged("IsFocusSoGheHang4"); 
            }
        }
        private string isFocusSoGheHang3 = "False";

        public string IsFocusSoGheHang3
        {
            get { return isFocusSoGheHang3; }
            set 
            {
                isFocusSoGheHang3 = value; 
                OnPropertyChanged("IsFocusSoGheHang3"); 
            }
        }
        private string isFocusSoGheHang2 = "False";

        public string IsFocusSoGheHang2
        {
            get { return isFocusSoGheHang2; }
            set 
            {
                isFocusSoGheHang2 = value; 
                OnPropertyChanged("IsFocusSoGheHang2"); 
            }
        }
        private string isFocusSoGheHang1 = "False";

        public string IsFocusSoGheHang1
        {
            get { return isFocusSoGheHang1; }
            set 
            {
                isFocusSoGheHang1 = value; 
                OnPropertyChanged("IsFocusSoGheHang1"); 
            }
        }

        private string isFocusSoGheHang5 = "False";

        public string IsFocusSoGheHang5
        {
            get { return isFocusSoGheHang5; }
            set
            {
                isFocusSoGheHang5 = value;
                OnPropertyChanged("IsFocusSoGheHang5");
            }
        }

        private string isFocusSoGheHang6 = "False";

        public string IsFocusSoGheHang6
        {
            get { return isFocusSoGheHang6; }
            set
            {
                isFocusSoGheHang6 = value;
                OnPropertyChanged("IsFocusSoGheHang6");
            }
        }

        private string isFocusSoGheHang7 = "False";

        public string IsFocusSoGheHang7
        {
            get { return isFocusSoGheHang7; }
            set
            {
                isFocusSoGheHang7 = value;
                OnPropertyChanged("IsFocusSoGheHang7");
            }
        }

        private string isFocusSoGheHang8 = "False";

        public string IsFocusSoGheHang8
        {
            get { return isFocusSoGheHang8; }
            set
            {
                isFocusSoGheHang8 = value;
                OnPropertyChanged("IsFocusSoGheHang8");
            }
        }

        #endregion
        #region Xử lý chuyến bay
        #region thông tin đang nhập
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
                    else
                    {
                        CustomMessageBox.Show("Mã Chuyến Bay không được chứa ký tự đặc biệt", "Nhắc nhở");
                    }    
                }
                else
                {
                    maChuyenBay = value;
                }    
                OnPropertyChanged("MaChuyenBay");
            }
        }
        private DateTime blackoutCollection = DateTime.UtcNow;

        public  DateTime BlackoutCollection
        {
            get { return blackoutCollection; }
            set { blackoutCollection = value; OnPropertyChanged("BlackoutCollection"); }
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
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        giaVeCoBan = value;
                    }
                    else
                    {
                        CustomMessageBox.Show("Giá vé phải là số nguyên dương", "Nhắc nhở");
                    }
                }
                else
                {
                    giaVeCoBan = value;
                }
                OnPropertyChanged("GiaVeCoBan");
            }
        }
        private DateTime gioBay = new DateTime(1, 1, 1, 0, 0, 0);

        public DateTime GioBay
        {
            get { return gioBay; }
            set { gioBay = value; OnPropertyChanged("GioBay"); }
        }
        private string thoiGianBay;

        public string ThoiGianBay
        {
            get { return thoiGianBay; }
            set 
            {
                if (value != null)
                {
                    if(KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        #region Kiểm tra quy định
                        if(Convert.ToInt32(value) < Convert.ToInt32(ThamSoQuyDinh.THOI_GIAN_BAY_TOI_THIEU))
                        {
                            CustomMessageBox.Show("Thời gian bay tối thiểu là " + ThamSoQuyDinh.THOI_GIAN_BAY_TOI_THIEU + " phút", "Nhắc nhở");
                        }
                        else
                            thoiGianBay = value;
                        #endregion
                    }    
                    else
                    {
                        CustomMessageBox.Show("Giá Vé phải là số nguyên dương", "Nhắc nhở");
                    }    
                }
                else
                    thoiGianBay = value;
                OnPropertyChanged("ThoiGianBay"); 
            }
        }
        private List<LoaiHangGhe> loaiHangGhesList;

        public List<LoaiHangGhe> LoaiHangGhesList
        {
            get { return loaiHangGhesList; }
            set { loaiHangGhesList = value; OnPropertyChanged("LoaiHangGhesList"); }
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
        #endregion
        private ChuyenBay chuyenBayDaChon;

        public ChuyenBay ChuyenBayDaChon
        {
            get { return chuyenBayDaChon; }
            set 
            {
                if (chuyenBayDaChon == null && value != null)
                {
                    IsVisibleSuaChuyenBay = "Visible";
                    IsVisibleXoaChuyenBay = "Visible";
                    chuyenBayDaChon = value;
                }
                else if (chuyenBayDaChon != null && value ==null)
                {
                    chuyenBayDaChon = value;
                    IsVisibleSuaChuyenBay = "Hidden";
                    IsVisibleXoaChuyenBay = "Hidden";
                }    
                else if(chuyenBayDaChon != null && value != null)
                {
                    chuyenBayDaChon = value;
                }    
                OnPropertyChanged("ChuyenBayDaChon");
            }
        }
        private string soCacHangVe = ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE;

        public string SoCacHangVe
        {
            get { return soCacHangVe; }
            set { soCacHangVe = value; OnPropertyChanged("SoCacHangVe"); }
        }

        private List<ChiTietHangGhe> chiTietHangGhesList = new List<ChiTietHangGhe>();

        public List<ChiTietHangGhe> ChiTietHangGhesList
        {
            get { return chiTietHangGhesList; }
            set { chiTietHangGhesList = value; OnPropertyChanged("ChiTietHangGhesList"); }
        }
        
        private ObservableCollection<ChuyenBay> chuyenBaysList;

        public ObservableCollection<ChuyenBay> ChuyenBaysList
        {
            get { return chuyenBaysList; }
            set { chuyenBaysList = value; OnPropertyChanged("ChuyenBaysList"); }
        }
        private ChuyenBay chuyenBayHienTai = new ChuyenBay();

        public ChuyenBay ChuyenBayHienTai
        {
            get { return chuyenBayHienTai; }
            set { chuyenBayHienTai = value; OnPropertyChanged("ChuyenBayHienTai"); }
        }
        private string isVisibleThemChuyenBayTrungGian = "Visible";

        public string IsVisibleThemChuyenBayTrungGian
        {
            get { return isVisibleThemChuyenBayTrungGian; }
            set { isVisibleThemChuyenBayTrungGian = value; OnPropertyChanged("isVisibleThemChuyenBayTrungGian"); }
        }

        private string isVisibleSuaChuyenBay = "Hidden";

        public string IsVisibleSuaChuyenBay
        {
            get { return isVisibleSuaChuyenBay; }
            set { isVisibleSuaChuyenBay = value; OnPropertyChanged("IsVisibleSuaChuyenBay"); }
        }
        private string isVisibleLuuChuyenBay = "Hidden";

        public string IsVisibleLuuChuyenBay
        {
            get { return isVisibleLuuChuyenBay; }
            set { isVisibleLuuChuyenBay = value; OnPropertyChanged("isVisibleLuuChuyenBay"); }
        }
        private string  isVisibleXoaChuyenBay = "Hidden";

        public  string IsVisibleXoaChuyenBay
        {
            get { return isVisibleXoaChuyenBay; }
            set { isVisibleXoaChuyenBay =  value; OnPropertyChanged("IsVisibleXoaChuyenBay"); }
        }
        private string isVisibleNhanLichChuyenBay = "Visible";

        public string IsVisibleNhanLichChuyenBay
        {
            get { return isVisibleNhanLichChuyenBay ; }
            set { isVisibleNhanLichChuyenBay  = value; OnPropertyChanged("IsVisibleNhanLichChuyenBay"); }
        }
        private string isVisibleHuyThaoTac = "Visible";

        public string IsVisibleHuyThaoTac
        {
            get { return isVisibleHuyThaoTac; }
            set { isVisibleHuyThaoTac = value; OnPropertyChanged("IsVisibleHuyThaoTac"); }
        }
        private string isReadOnlyMaChuyenBay = "False";

        public string IsReadOnlyMaChuyenBay
        {
            get { return isReadOnlyMaChuyenBay; }
            set { isReadOnlyMaChuyenBay = value; OnPropertyChanged("IsReadOnlyMaChuyenBay"); }
        }

        private RelayCommand themChuyenBayCommand;

        public RelayCommand ThemChuyenBayCommand
        {
            get { return themChuyenBayCommand; }
        }
        private RelayCommand xoaChuyenBayCommand;

        public RelayCommand XoaChuyenBayCommand
        {
            get { return xoaChuyenBayCommand; }
        }
        private RelayCommand suaChuyenBayCommand;

        public RelayCommand SuaChuyenBayCommand
        {
            get { return suaChuyenBayCommand; }
        }
        private RelayCommand huyThemVaSuaChuyenBayCommand;

        public RelayCommand HuyThemVaSuaChuyenBayCommand
        {
            get { return huyThemVaSuaChuyenBayCommand; }
        }
        private RelayCommand luuSuaChuyenBayCommand;
        public RelayCommand LuuSuaChuyenBayCommand
        {
            get { return luuSuaChuyenBayCommand; }
        }
        public void themChuyenBay()
        {
            #region Bắt ex
            if (MaChuyenBay == "" || MaChuyenBay == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusMaChuyenBay = "True";
                return;
            } 

            if (GiaVeCoBan == "" || GiaVeCoBan ==null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusGiaChuyenBay = "True";
                return;
            }

            if (ThoiGianBay == "" || ThoiGianBay == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusThoiGianBay = "True";
                return;
            }

            if (SanBayDiDaChon == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusSanBayDi = "True";
                return;
            }

            if (SanBayDenDaChon == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusSanBayDen = "True";
                return;
            }

            #endregion
            ChuyenBayHienTai.SanBayDi = SanBayDiDaChon;
            ChuyenBayHienTai.SanBayDen = SanBayDenDaChon;
            ChuyenBayHienTai.SanBayTrungGian = SanBayTrungGiansList;
            ChuyenBayHienTai.MaChuyenBay = MaChuyenBay;
            ChuyenBayHienTai.GiaVeCoBan = GiaVeCoBan;
            ChuyenBayHienTai.GioBay = GioBay;
            ChuyenBayHienTai.NgayBay = NgayBay;
            ChuyenBayHienTai.ThoiGianBay = ThoiGianBay;
            ChuyenBayHienTai.SoLoaiHangGhe = Convert.ToInt32(ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE);
            for(int i = 0;i<ChuyenBayHienTai.SoLoaiHangGhe; i++)
            {
                switch(i)
                {
                    case 0: chiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang1));break;
                    case 1: chiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang2));break;
                    case 2: chiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang3)); break;
                    case 3: chiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang4)); break;
                    case 4: chiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang5)); break;
                    case 5: chiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang6)); break;
                    case 6: chiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang7)); break;
                    default : chiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang8)); break;
                }
            }
            ChuyenBayHienTai.ChiTietHangGhesList = ChiTietHangGhesList;

            #region Cập nhật dữ liệu
            if (chuyenBayServices.Add(ChuyenBayHienTai))
                ChuyenBaysList.Add(ChuyenBayHienTai);
            else
            {
                CustomMessageBox.Show("Mã Chuyến Bay đã tồn tại", "Nhắc nhở");
                //resetNhapChuyenBay();
                return;
            }
            sanBayTrungGianService.ThemListSanBayTrungGian(SanBayTrungGiansList);
            chiTietHangGheServices.ThemListChiTietHangGhe(ChiTietHangGhesList);
            #endregion

            ChuyenBayHienTai = new ChuyenBay();
            resetNhapChuyenBay();            
        }
        private ObservableCollection<SanBayTrungGian> sanBayTrungGiansListCu;

        public ObservableCollection<SanBayTrungGian> SanBayTrungGiansListCu
        {
            get { return sanBayTrungGiansListCu; }
            set { sanBayTrungGiansListCu = value; OnPropertyChanged("SanBayTrungGiansListCu"); }
        }
        private bool isDangSua;
        public void suaChuyenBay()
        {
            LoadUIHangGheTheoChuyenBay(ChuyenBayDaChon.SoLoaiHangGhe.ToString());
            isDangSua = true;
            IsReadOnlyMaChuyenBay = "True";
            IsVisibleLuuChuyenBay = "Visible";
            IsVisibleSuaChuyenBay = "Hidden";
            IsVisibleXoaChuyenBay = "Hidden";
            IsVisibleNhanLichChuyenBay = "Hidden";
            SanBayDiDaChon = ChuyenBayDaChon.SanBayDi;
            SanBayDenDaChon = ChuyenBayDaChon.SanBayDen;
            SanBayTrungGiansList = ChuyenBayDaChon.SanBayTrungGian;
            SanBayTrungGiansListCu = new ObservableCollection<SanBayTrungGian>(SanBayTrungGiansList);
            MaChuyenBay = ChuyenBayDaChon.MaChuyenBay;
            GiaVeCoBan = ChuyenBayDaChon.GiaVeCoBan;
            NgayBay = ChuyenBayDaChon.NgayBay;
            GioBay = ChuyenBayDaChon.GioBay;
            ThoiGianBay = ChuyenBayDaChon.ThoiGianBay;
            try
            {
                SoGheHang1 = ChuyenBayDaChon.ChiTietHangGhesList[0].SoLuongGhe;
                SoGheHang2 = ChuyenBayDaChon.ChiTietHangGhesList[1].SoLuongGhe;
                SoGheHang3 = ChuyenBayDaChon.ChiTietHangGhesList[2].SoLuongGhe;
                SoGheHang4 = ChuyenBayDaChon.ChiTietHangGhesList[3].SoLuongGhe;
                SoGheHang5 = ChuyenBayDaChon.ChiTietHangGhesList[4].SoLuongGhe;
                SoGheHang6 = ChuyenBayDaChon.ChiTietHangGhesList[5].SoLuongGhe;
                SoGheHang7 = ChuyenBayDaChon.ChiTietHangGhesList[6].SoLuongGhe;
                SoGheHang8 = ChuyenBayDaChon.ChiTietHangGhesList[7].SoLuongGhe;
            }
            catch
            {

            }
            IsDaBay = ChuyenBayDaChon.IsDaBay;
        }
        private void luuSuaChuyenBay()
        {
            isDangSua = false;

            #region Bắt ex
            if (MaChuyenBay == "" || MaChuyenBay == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusMaChuyenBay = "True";
                return;
            }

            if (GiaVeCoBan == "" || GiaVeCoBan == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusGiaChuyenBay = "True";
                return;
            }

            if (ThoiGianBay == "" || ThoiGianBay == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusThoiGianBay = "True";
                return;
            }

            if (SanBayDiDaChon == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusSanBayDi = "True";
                return;
            }

            if (SanBayDenDaChon == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusSanBayDen = "True";
                return;
            }

            #endregion

            string maChuyenBayCu = ChuyenBayDaChon.MaChuyenBay;
            if (maChuyenBayCu == MaChuyenBay)
            {
                IsReadOnlyMaChuyenBay = "False";

                ChuyenBayDaChon.GiaVeCoBan = GiaVeCoBan;
                ChuyenBayDaChon.NgayBay = NgayBay;
                ChuyenBayDaChon.GioBay = GioBay;
                ChuyenBayDaChon.ThoiGianBay = ThoiGianBay;
                ChuyenBayDaChon.SanBayDi = SanBayDiDaChon;
                ChuyenBayDaChon.SanBayDen = SanBayDenDaChon;
                ChuyenBayDaChon.SanBayTrungGian = SanBayTrungGiansList;
                try
                {
                    ChuyenBayDaChon.ChiTietHangGhesList[0].SoLuongGhe = SoGheHang1;
                    ChuyenBayDaChon.ChiTietHangGhesList[1].SoLuongGhe = SoGheHang2;
                    ChuyenBayDaChon.ChiTietHangGhesList[2].SoLuongGhe = SoGheHang3;
                    ChuyenBayDaChon.ChiTietHangGhesList[3].SoLuongGhe = SoGheHang4;
                    ChuyenBayDaChon.ChiTietHangGhesList[4].SoLuongGhe = SoGheHang5;
                    ChuyenBayDaChon.ChiTietHangGhesList[5].SoLuongGhe = SoGheHang6;
                    ChuyenBayDaChon.ChiTietHangGhesList[6].SoLuongGhe = SoGheHang7;
                    ChuyenBayDaChon.ChiTietHangGhesList[7].SoLuongGhe = SoGheHang8;
                }
                catch
                { }

                #region Cập nhập dữ liệu
                LoadUIHangGheTheoQuyDinh();
                chuyenBayServices.Update(ChuyenBayDaChon);
                sanBayTrungGianService.ClearSpecializeSanBay(MaChuyenBay);
                sanBayTrungGianService.ThemListSanBayTrungGian(SanBayTrungGiansList);
                chiTietHangGheServices.Delete(MaChuyenBay);
                chiTietHangGheServices.ThemListChiTietHangGhe(ChiTietHangGhesList);
                #endregion
            }
            //else if (chuyenBayServices.IsEditable(MaChuyenBay))
            //{

            //    foreach (SanBayTrungGian sanBayTrungGian in SanBayTrungGiansList)
            //        sanBayTrungGian.MaChuyenBay = MaChuyenBay;

            //    ChuyenBayDaChon.MaChuyenBay = MaChuyenBay;
            //    ChuyenBayDaChon.GiaVeCoBan = GiaVeCoBan;
            //    ChuyenBayDaChon.NgayBay = NgayBay;
            //    ChuyenBayDaChon.GioBay = GioBay;
            //    ChuyenBayDaChon.ThoiGianBay = ThoiGianBay;
            //    ChuyenBayDaChon.SanBayDi = SanBayDiDaChon;
            //    ChuyenBayDaChon.SanBayDen = SanBayDenDaChon;
            //    ChuyenBayDaChon.SanBayTrungGian = SanBayTrungGiansList;
            //    ChuyenBayDaChon.SoGheHang1 = SoGheHang1;
            //    ChuyenBayDaChon.SoGheHang2 = SoGheHang2;
            //    ChuyenBayDaChon.SoGheHang3 = SoGheHang3;
            //    ChuyenBayDaChon.SoGheHang4 = SoGheHang4;

            //    #region Cập nhật dữ liệu
            //    chuyenBayServices.XoaChuyenBaySql(maChuyenBayCu,sanBayTrungGianService);
            //    chuyenBayServices.ThemChuyenBaySQL(ChuyenBayDaChon);

            //    sanBayTrungGianService.ThemListSanBayTrungGian(SanBayTrungGiansList);
            //    #endregion
            //}
            //else
            //{

            //}    
            IsVisibleLuuChuyenBay = "Hidden";
            IsVisibleNhanLichChuyenBay = "Visible";
            IsVisibleSuaChuyenBay = "Hidden";
            ChuyenBayDaChon = null;
            resetNhapChuyenBay();
            SanBayTrungGiansListCu = null;

        }
        private void xoaChuyenBay()
        {
            MessageBoxResult rs = CustomMessageBox.Show("Bạn chắc chắn muốn xóa", "Cảnh báo", System.Windows.MessageBoxButton.OKCancel);
            if (rs == MessageBoxResult.OK && chuyenBayServices.Delete(ChuyenBayDaChon.MaChuyenBay, sanBayTrungGianService,chiTietHangGheServices))
            {
                ChuyenBaysList.Remove(ChuyenBayDaChon);
                ChuyenBayDaChon = null;
            }    
        }
        public void resetNhapChuyenBay()
        {
            SanBayDenDaChon = null;
            SanBayDiDaChon = null;
            SanBayTrungGiansList = null;
            ChiTietHangGhesList = null;
            MaChuyenBay = null;
            GiaVeCoBan = null;
            GioBay = new DateTime(1, 1, 1, 0, 0, 0);
            NgayBay = DateTime.UtcNow.AddDays(1);
            ThoiGianBay = null;
            SoGheHang1 = null;
            SoGheHang2 = null;
            SoGheHang3 = null;
            SoGheHang4 = null;
            SoGheHang5 = null;
            SoGheHang6 = null;
            SoGheHang7 = null;
            SoGheHang8 = null;
        }
        public void huyThemVaSuaChuyenBay()
        {
            MessageBoxResult rs = CustomMessageBox.Show("Bạn chắc chắn muốn hủy", "Cảnh báo", System.Windows.MessageBoxButton.OKCancel);
            if (rs == MessageBoxResult.OK)
            {
                if (isDangSua)
                {
                    ChuyenBayDaChon.SanBayTrungGian = SanBayTrungGiansListCu;
                    isDangSua = false;
                }
                IsVisibleNhanLichChuyenBay = "Visible";
                IsVisibleSuaChuyenBay = "Hidden";
                IsVisibleLuuChuyenBay = "Hidden";
                ChuyenBayDaChon = null;
                resetNhapChuyenBay();
            }
        }
        #endregion
        #region Load giao diện nhập hạng ghế
        private string isVisibleHang1 = "Visible";

        public string IsVisibleHang1
        {
            get { return isVisibleHang1; }
            set { isVisibleHang1 = value; OnPropertyChanged("IsVisibleHang1"); }
        }

        private string isVisibleHang2 = "Visible";

        public string IsVisibleHang2
        {
            get { return isVisibleHang2; }
            set { isVisibleHang2 = value; OnPropertyChanged("IsVisibleHang2"); }
        }

        private string isVisibleHang3 = "Visible";

        public string IsVisibleHang3
        {
            get { return isVisibleHang3; }
            set { isVisibleHang3 = value; OnPropertyChanged("IsVisibleHang3"); }
        }

        private string isVisibleHang4 = "Visible";

        public string IsVisibleHang4
        {
            get { return isVisibleHang4; }
            set { isVisibleHang4 = value; OnPropertyChanged("IsVisibleHang4"); }
        }

        private string isVisibleHang5 = "Visible";

        public string IsVisibleHang5
        {
            get { return isVisibleHang5; }
            set { isVisibleHang5 = value; OnPropertyChanged("IsVisibleHang5"); }
        }


        private string isVisibleHang6 = "Visible";

        public string IsVisibleHang6
        {
            get { return isVisibleHang6; }
            set { isVisibleHang6 = value; OnPropertyChanged("IsVisibleHang6"); }
        }

        private string isVisibleHang7 = "Visible";

        public string IsVisibleHang7
        {
            get { return isVisibleHang7; }
            set { isVisibleHang7 = value; OnPropertyChanged("IsVisibleHang7"); }
        }

        private string isVisibleHang8 = "Visible";

        public string IsVisibleHang8
        {
            get { return isVisibleHang8; }
            set { isVisibleHang8 = value; OnPropertyChanged("IsVisibleHang8"); }
        }
        public void LoadUIHangGheTheoQuyDinh()
        {
            if(ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE == "1")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Hidden";
                IsVisibleHang3 = "Hidden";
                IsVisibleHang4 = "Hidden";
                IsVisibleHang5 = "Hidden";
                IsVisibleHang6 = "Hidden";
                IsVisibleHang7 = "Hidden";
                IsVisibleHang8 = "Hidden";
            } 
            else if (ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE == "2")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Hidden";
                IsVisibleHang4 = "Hidden";
                IsVisibleHang5 = "Hidden";
                IsVisibleHang6 = "Hidden";
                IsVisibleHang7 = "Hidden";
                IsVisibleHang8 = "Hidden";
            }    
            else if (ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE == "3")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Visible";
                IsVisibleHang4 = "Hidden";
                IsVisibleHang5 = "Hidden";
                IsVisibleHang6 = "Hidden";
                IsVisibleHang7 = "Hidden";
                IsVisibleHang8 = "Hidden";
            }    
            else if (ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE == "4")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Visible";
                IsVisibleHang4 = "Visible";
                IsVisibleHang5 = "Hidden";
                IsVisibleHang6 = "Hidden";
                IsVisibleHang7 = "Hidden";
                IsVisibleHang8 = "Hidden";
            }
            else if (ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE == "5")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Visible";
                IsVisibleHang4 = "Visible";
                IsVisibleHang5 = "Visible";
                IsVisibleHang6 = "Hidden";
                IsVisibleHang7 = "Hidden";
                IsVisibleHang8 = "Hidden";
            }
            else if (ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE == "6")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Visible";
                IsVisibleHang4 = "Visible";
                IsVisibleHang5 = "Visible";
                IsVisibleHang6 = "Visible";
                IsVisibleHang7 = "Hidden";
                IsVisibleHang8 = "Hidden";
            }
            else if (ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE == "7")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Visible";
                IsVisibleHang4 = "Visible";
                IsVisibleHang5 = "Visible";
                IsVisibleHang6 = "Visible";
                IsVisibleHang7 = "Visible";
                IsVisibleHang8 = "Hidden";
            }
            else
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Visible";
                IsVisibleHang4 = "Visible";
                IsVisibleHang5 = "Visible";
                IsVisibleHang6 = "Visible";
                IsVisibleHang7 = "Visible";
                IsVisibleHang8 = "Visible";
            }
        }

        public void LoadUIHangGheTheoChuyenBay(string SoLuongCacHangVe)
        {
            if (SoLuongCacHangVe == "1")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Hidden";
                IsVisibleHang3 = "Hidden";
                IsVisibleHang4 = "Hidden";
                IsVisibleHang5 = "Hidden";
                IsVisibleHang6 = "Hidden";
                IsVisibleHang7 = "Hidden";
                IsVisibleHang8 = "Hidden";
            }
            else if (SoLuongCacHangVe == "2")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Hidden";
                IsVisibleHang4 = "Hidden";
                IsVisibleHang5 = "Hidden";
                IsVisibleHang6 = "Hidden";
                IsVisibleHang7 = "Hidden";
                IsVisibleHang8 = "Hidden";
            }
            else if (SoLuongCacHangVe == "3")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Visible";
                IsVisibleHang4 = "Hidden";
                IsVisibleHang5 = "Hidden";
                IsVisibleHang6 = "Hidden";
                IsVisibleHang7 = "Hidden";
                IsVisibleHang8 = "Hidden";
            }
            else if (SoLuongCacHangVe == "4")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Visible";
                IsVisibleHang4 = "Visible";
                IsVisibleHang5 = "Hidden";
                IsVisibleHang6 = "Hidden";
                IsVisibleHang7 = "Hidden";
                IsVisibleHang8 = "Hidden";
            }
            else if (SoLuongCacHangVe == "5")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Visible";
                IsVisibleHang4 = "Visible";
                IsVisibleHang5 = "Visible";
                IsVisibleHang6 = "Hidden";
                IsVisibleHang7 = "Hidden";
                IsVisibleHang8 = "Hidden";
            }
            else if (SoLuongCacHangVe == "6")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Visible";
                IsVisibleHang4 = "Visible";
                IsVisibleHang5 = "Visible";
                IsVisibleHang6 = "Visible";
                IsVisibleHang7 = "Hidden";
                IsVisibleHang8 = "Hidden";
            }
            else if (SoLuongCacHangVe == "7")
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Visible";
                IsVisibleHang4 = "Visible";
                IsVisibleHang5 = "Visible";
                IsVisibleHang6 = "Visible";
                IsVisibleHang7 = "Visible";
                IsVisibleHang8 = "Hidden";
            }
            else
            {
                IsVisibleHang1 = "Visible";
                IsVisibleHang2 = "Visible";
                IsVisibleHang3 = "Visible";
                IsVisibleHang4 = "Visible";
                IsVisibleHang5 = "Visible";
                IsVisibleHang6 = "Visible";
                IsVisibleHang7 = "Visible";
                IsVisibleHang8 = "Visible";
            }
        }
        #endregion
    }
}

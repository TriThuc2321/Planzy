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

namespace Planzy.ViewModels
{
    class PlanzyViewModel : INotifyPropertyChanged,ITextBoxController
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
        public PlanzyViewModel()
        {
            sanBayServices = new SanBayService();
            sanBayTrungGianService = new SanBayTrungGianService();
            chuyenBayServices = new ChuyenBayServices();

            LoadData();
            doiViTriSanBayCommand = new RelayCommand(DoiViTriSanBay);
            xoaSanBayTrungGianCommand = new RelayCommand(xoaSanBayTrungGian);
            themSanBayTrungGianCommand = new RelayCommand(themSanBayTrungGian);
            huyThemCommand = new RelayCommand(huyThemSanBayTrungGian);
            xacNhanThemCommand = new RelayCommand(xacNhanThemSanBayTrungGian);
            themChuyenBayCommand = new RelayCommand(themChuyenBay);
            #region Xử lý giao diện ban đầu
            chonLayoutCommand1 = new RelayCommand(Button1);
            chonLayoutCommand2 = new RelayCommand(Button2);
            chonLayoutCommand3 = new RelayCommand(Button3);
            chonLayoutCommand4 = new RelayCommand(Button4);
            chonLayoutCommand5 = new RelayCommand(Button5);
            #endregion
            SelectAllCommand = new RelayCommand(p =>
            {
                if (SelectAll != null)
                    SelectAll(this);
            });
        }
        public RelayCommand SelectAllCommand { get; private set; }
        public RelayCommand SelectAllCommand2 { get; private set; }

        public event SelectAllEventHandler SelectAll;
        #region Xử lý chung
        private void LoadData()
        {
            SanbaysList = new List<SanBay>(sanBayServices.GetAll());
            SanBayDensList = new ObservableCollection<SanBay>(sanBayServices.GetAll());
            SanBayDisList = new ObservableCollection<SanBay>(sanBayServices.GetAll());
            
            SanBayTrungGianSapThemsList = new ObservableCollection<SanBay>(sanBayServices.GetAll());
            SanBayTrungGiansList = new ObservableCollection<SanBayTrungGian>(sanBayTrungGianService.GetAll());
            chuyenBaysList = new ObservableCollection<ChuyenBay>(chuyenBayServices.GetAll());

            
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
                SanBayDensList.Remove(SanBayDiDaChon);
                SanBayTrungGianSapThemsList.Remove(SanBayDiDaChon);
                if(SanBayTrungGiansList.Count != 0)
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
                SanBayDisList.Remove(SanBayDenDaChon);
                SanBayTrungGianSapThemsList.Remove(SanBayDenDaChon);
                if (SanBayTrungGiansList.Count != 0)
                    xoaSanBayTrungGian( new SanBayTrungGian(SanBayDenDaChon));
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
        private ObservableCollection<SanBayTrungGian> sanBayTrungGiansList;

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
                if (isCheck)
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
            IsDropDown = "True";
            IsVisible = "Visible";
            this.sanBayBenDuoiSanBaySapThem = sanBayBenDuoiSanBaySapThem;
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
            int ViTriSanBayTrungGianDuocThem = 0;
            SanBayTrungGian SanBayTrungGianDuocChon = new SanBayTrungGian();
            int index;
            if (sanBayBenDuoiSanBaySapThem == null)
            {
                if (SanBayTrungGiansList.Count == 0)
                {
                    SanBayTrungGian newSanBay = new SanBayTrungGian();
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
            set { thoiGianDungSapThem = value; OnPropertyChanged("ThoiGianDungSapThem"); }
        }

        #endregion
        #region Xử lý focus textbox
        private string isFocusGiaChuyenBay = "False";

        public string IsFocusGiaChuyenBay
        {
            get { return isFocusGiaChuyenBay; }
            set 
            { 
                if (value != null && value == "True")
                {
                    IsFocusMaChuyenBay = "False";
                    IsFocusSoGheHang1 = "False";
                    IsFocusSoGheHang2 = "False";
                    IsFocusSoGheHang3 = "False";
                    IsFocusSoGheHang4 = "False";
                    IsFocusThoiGianDung = "False";
                    IsFocusThoiGianBay = "False";
                }    
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
                if (value != null && value == "True")
                {
                    IsFocusGiaChuyenBay = "False";
                    IsFocusSoGheHang1 = "False";
                    IsFocusSoGheHang2 = "False";
                    IsFocusSoGheHang3 = "False";
                    IsFocusSoGheHang4 = "False";
                    IsFocusThoiGianDung = "False";
                    IsFocusThoiGianBay = "False";
                }
                isFocusMaChuyenBay = value; 
                OnPropertyChanged("IsFocusMaChuyenBay"); 
            }
        }
        private string isFocusThoiGianDung = "False";

        public string IsFocusThoiGianDung
        {
            get { return isFocusThoiGianDung; }
            set 
            {
                if (value != null && value == "True")
                {
                    IsFocusMaChuyenBay = "False";
                    IsFocusSoGheHang1 = "False";
                    IsFocusSoGheHang2 = "False";
                    IsFocusSoGheHang3 = "False";
                    IsFocusSoGheHang4 = "False";
                    isFocusGiaChuyenBay = "False";
                    IsFocusThoiGianBay = "False";
                }
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
                if (value != null && value == "True")
                {
                    IsFocusMaChuyenBay = "False";
                    IsFocusSoGheHang1 = "False";
                    IsFocusSoGheHang2 = "False";
                    IsFocusSoGheHang3 = "False";
                    IsFocusSoGheHang4 = "False";
                    IsFocusThoiGianDung = "False";
                    IsFocusGiaChuyenBay = "False";
                }
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
                if (value != null && value == "True")
                {
                    IsFocusMaChuyenBay = "False";
                    IsFocusSoGheHang1 = "False";
                    IsFocusSoGheHang2 = "False";
                    IsFocusSoGheHang3 = "False";
                    IsFocusGiaChuyenBay = "False";
                    IsFocusThoiGianDung = "False";
                    IsFocusThoiGianBay = "False";
                }
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
                if (value != null && value == "True")
                {
                    IsFocusMaChuyenBay = "False";
                    IsFocusSoGheHang1 = "False";
                    IsFocusSoGheHang2 = "False";
                    IsFocusGiaChuyenBay = "False";
                    IsFocusSoGheHang4 = "False";
                    IsFocusThoiGianDung = "False";
                    IsFocusThoiGianBay = "False";
                }
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
                if (value != null && value == "True")
                {
                    IsFocusMaChuyenBay = "False";
                    IsFocusSoGheHang1 = "False";
                    IsFocusGiaChuyenBay = "False";
                    IsFocusSoGheHang3 = "False";
                    IsFocusSoGheHang4 = "False";
                    IsFocusThoiGianDung = "False";
                    IsFocusThoiGianBay = "False";
                }
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
                if (value != null && value == "True")
                {
                    IsFocusMaChuyenBay = "False";
                    IsFocusGiaChuyenBay = "False";
                    IsFocusSoGheHang2 = "False";
                    IsFocusSoGheHang3 = "False";
                    IsFocusSoGheHang4 = "False";
                    IsFocusThoiGianDung = "False";
                    IsFocusThoiGianBay = "False";
                }
                isFocusSoGheHang1 = value; 
                OnPropertyChanged("IsFocusSoGheHang1"); 
            }
        }

        #endregion
        #region Xử lý chuyến bay
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
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        giaVeCoBan = value;
                    }
                    else
                    {
                        //IsFocusGiaChuyenBay = "True";
                    }
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
            get { return SuaChuyenBayCommand; }
        }
        private RelayCommand huyThemVaSuaChuyenBay;

        public RelayCommand HuyThemVaSuaChuyenBay
        {
            get { return huyThemVaSuaChuyenBay; }
        }
        public void themChuyenBay()
        {
            ChuyenBayHienTai.SanBayDi = SanBayDiDaChon;
            ChuyenBayHienTai.SanBayDen = SanBayDenDaChon;
            ChuyenBayHienTai.SanBayTrungGian = SanBayTrungGiansList;
            ChuyenBayHienTai.MaChuyenBay = MaChuyenBay;
            ChuyenBayHienTai.GiaVeCoBan = GiaVeCoBan;
            ChuyenBayHienTai.GioBay = GioBay;
            ChuyenBayHienTai.NgayBay = NgayBay;
            ChuyenBayHienTai.ThoiGianBay = ThoiGianBay;
            ChuyenBayHienTai.SoGheHang1 = SoGheHang1;
            ChuyenBayHienTai.SoGheHang2 = SoGheHang2;
            ChuyenBayHienTai.SoGheHang3 = SoGheHang3;
            ChuyenBayHienTai.SoGheHang4 = SoGheHang4;
            ChuyenBaysList.Add(ChuyenBayHienTai);
            ChuyenBayHienTai = new ChuyenBay();
        }
        #endregion
    }
}

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
using System.Windows.Input;
using System.Collections;
using Planzy.Models.BookingSticketModel;
using System.Windows.Controls;

namespace Planzy.ViewModels
{
    class PlanzyViewModel : INotifyPropertyChanged, ITextBoxController
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
            searchFlightCommand = new RelayCommand(searchFlight);
            searchFlightCommand_FlightBooking = new RelayCommand(searchFlight_FlightBooking);
            resetCommand = new RelayCommand(resetSearchList);
            showAllFightsCommand_FlightBooking = new RelayCommand(showAllFlights);
            choosePayButtonCommand = new RelayCommand2<object>(checkPassangerInfor, ButtonPay);
            #region Xử lý giao diện ban đầu
            chonLayoutCommand1 = new RelayCommand(Button1);
            chonLayoutCommand2 = new RelayCommand(Button2);
            chonLayoutCommand3 = new RelayCommand(Button3);
            chonLayoutCommand4 = new RelayCommand(Button4);
            chonLayoutCommand5 = new RelayCommand(Button5);
            //Thien
            chooseContinueButtonCommand = new RelayCommand2<object>((p) => p != null, ButtonContinue);
            chooseBackButtonCommand = new RelayCommand(ButtonBack);
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

            // Thiên
            FlightSearchList = new ObservableCollection<ChuyenBay>(chuyenBayServices.GetAll());
            FlightSearchList_FlightBooking = new ObservableCollection<ChuyenBay>(chuyenBayServices.GetFlightBookingList(flightSearchList_FlightBooking));
            backupList_FlightBooking = new ObservableCollection<ChuyenBay>(chuyenBayServices.GetFlightBookingList(flightSearchList_FlightBooking));
            Departure = new ObservableCollection<SanBay>(sanBayServices.GetAll());
            Destination = new ObservableCollection<SanBay>(sanBayServices.GetAll());
            DepartureList_FlightBooking = new ObservableCollection<SanBay>(sanBayServices.GetAll());
            DestinationList_FlightBooking = new ObservableCollection<SanBay>(sanBayServices.GetAll());
            listSticketType = new ObservableCollection<string>();
            hashtable_AmountSticketType = new Hashtable();
            hashtable_SticketID = new Hashtable();
            bookingSticket = new BookingSticket();








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
                if (SanBayTrungGiansList.Count != 0)
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
                    xoaSanBayTrungGian(new SanBayTrungGian(SanBayDenDaChon));
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
        }
        #endregion
        #region Xử lý giao diện ban đầu
        private ButtonDuocChon duocChon = new ButtonDuocChon(true);

        public ButtonDuocChon DuocChon
        {
            get { return duocChon; }
            set { duocChon = value; }
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
            IsContinueButton = KhongDuocChon;
        }
        public void Button2()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = DuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsContinueButton = KhongDuocChon;

        }
        public void Button3()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = DuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsContinueButton = KhongDuocChon;

        }
        public void Button4()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = DuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsContinueButton = KhongDuocChon;

        }
        public void Button5()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = DuocChon;
            IsContinueButton = KhongDuocChon;

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
            for (int i = 0; i < SanBayTrungGiansList.Count; i++)
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
                    else if (i != SanBayTrungGiansList.Count - 1)
                    {
                        SanBayTrungGiansList[i - 1].MaSanBaySau = SanBayTrungGiansList[i + 1].MaSanBay;
                        SanBayTrungGiansList[i + 1].MaSanBayTruoc = SanBayTrungGiansList[i - 1].MaSanBay;
                        SanBayTrungGiansList.RemoveAt(i);
                        return;
                    }
                    else
                    {
                        SanBayTrungGiansList[i - 1].MaSanBaySau = SanBayDenDaChon.Id;
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
                if (SanBayTrungGiansList == null)
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
                    for (int i = 0; i < sanBayTrungGiansList.Count; i++)
                    {
                        for (int j = 0; j < SanBayTrungGianSapThemsList.Count; j++)
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
        private string tes = "True";

        public string test
        {
            get { return tes; }
            set { tes = value; OnPropertyChanged("test"); }
        }
        #region Xử lý chuyến bay
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
            ChuyenBaysList.Add(ChuyenBayHienTai);
            ChuyenBayHienTai = new ChuyenBay();

        }
        #endregion

        #region Flight Searching Processing
        private bool isSearchingWithDate = false;
        public bool IsSearchingWithDate
        {
            get { return isSearchingWithDate; }
            set
            {
                isSearchingWithDate = value;
                if (isSearchingWithDate == true)
                    ComboboxVisibility = "Visible";
                else ComboboxVisibility = "Hidden";
                OnPropertyChanged("IsSearchingWithDate");
            }
        }

        private string comboboxVisibility = "Hidden";
        public string ComboboxVisibility
        {
            get { return comboboxVisibility; }
            set
            {
                comboboxVisibility = value;
                OnPropertyChanged("ComboboxVisibility");
            }
        }

        private ObservableCollection<SanBay> departure;
        public ObservableCollection<SanBay> Departure
        {
            get { return departure; }
            set { departure = value; OnPropertyChanged("Departure"); }
        }

        private ObservableCollection<SanBay> destination;
        public ObservableCollection<SanBay> Destination
        {
            get { return destination; }
            set { destination = value; OnPropertyChanged("Destination"); }
        }

        private DateTime selectedDate = DateTime.UtcNow.AddDays(1);
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; OnPropertyChanged("SelectedDate"); }
        }

        private String searchString = "";
        public String SearchString
        {
            get { return searchString; }
            set { searchString = value; OnPropertyChanged("SearchString"); }
        }

        private SanBay selectedDeparture;
        public SanBay SelectedDeparture
        {
            get { return selectedDeparture; }
            set
            {
                if (departure != null && destination != null)
                {
                    if (SelectedDeparture != null)
                        destination.Add(SelectedDeparture);
                    selectedDeparture = value;
                    OnPropertyChanged("SelectedDeparture");
                    destination.Remove(SelectedDeparture);
                }
            }
        }

        private SanBay selectedDestination;
        public SanBay SelectedDestination
        {
            get { return selectedDestination; }
            set
            {
                if (departure != null && destination != null)
                {
                    if (SelectedDestination != null)
                        departure.Add(SelectedDestination);
                    selectedDestination = value;
                    OnPropertyChanged("SelectedDestination");
                    departure.Remove(SelectedDestination);
                }
            }
        }

        private ObservableCollection<ChuyenBay> flightSearchList;
        public ObservableCollection<ChuyenBay> FlightSearchList
        {
            get { return flightSearchList; }
            set { flightSearchList = value; OnPropertyChanged("FlightSearchList"); }
        }

        public RelayCommand searchFlightCommand { get; private set; }
        public RelayCommand resetCommand { get; private set; }


        private void searchFlight()
        {
            ObservableCollection<ChuyenBay> temp = new ObservableCollection<ChuyenBay>(chuyenBaysList);
            flightSearchList = temp;
            if (selectedDeparture != null || selectedDestination != null || searchString != "" || isSearchingWithDate == true)
            {
                if (selectedDeparture != null)
                    for (int i = 0; i < flightSearchList.Count; i++)
                    {
                        if (flightSearchList[i].SanBayDen.Id == selectedDeparture.Id) continue;
                        flightSearchList.RemoveAt(i);
                        i = -1;
                    }
                if (selectedDestination != null)
                    for (int i = 0; i < flightSearchList.Count; i++)
                    {
                        if (flightSearchList[i].SanBayDi.Id == selectedDestination.Id) continue;
                        flightSearchList.RemoveAt(i);
                        i = -1;
                    }
                if (searchString != "")
                    for (int i = 0; i < flightSearchList.Count; i++)
                    {
                        if (flightSearchList[i].MaChuyenBay.Contains(searchString.ToUpper())) continue;
                        flightSearchList.RemoveAt(i);
                        i = -1;
                    }
                if (isSearchingWithDate == true)
                    for (int i = 0; i < flightSearchList.Count; i++)
                    {
                        if (flightSearchList[i].NgayBay == selectedDate) continue;
                        flightSearchList.RemoveAt(i);
                        i = -1;
                    }
            }
            OnPropertyChanged("FlightSearchList");
        }
        private void resetSearchList()
        {
            SelectedDate = DateTime.UtcNow.AddDays(1);
            SearchString = "";
            SanBay temp1 = selectedDestination;
            SanBay temp2 = selectedDeparture;
            departure.Remove(selectedDeparture);
            destination.Remove(selectedDestination);
            if (temp2 != null)
                departure.Add(temp2);
            if (temp1 != null)
                destination.Add(temp1);
            selectedDeparture = null;
            selectedDestination = null;
            ObservableCollection<ChuyenBay> temp = new ObservableCollection<ChuyenBay>(chuyenBaysList);
            flightSearchList = temp;
            OnPropertyChanged("FlightSearchList");
        }
        #endregion

        #region Flight Booking Processing 
        private ObservableCollection<ChuyenBay> flightSearchList_FlightBooking;
        public ObservableCollection<ChuyenBay> FlightSearchList_FlightBooking
        {
            get { return flightSearchList_FlightBooking; }
            set { flightSearchList_FlightBooking = value; OnPropertyChanged("FlightSearchList_FlightBooking"); }
        }
        private ObservableCollection<ChuyenBay> backupList_FlightBooking;
        public ObservableCollection<ChuyenBay> BackupList_FlightBooking
        {
            get { return backupList_FlightBooking; }
            set { backupList_FlightBooking = value; OnPropertyChanged("BackupList_FlightBooking"); }
        }
        private ChuyenBay selectedFlight;
        public ChuyenBay SelectedFlight
        {
            get { return selectedFlight; }
            set
            {
                selectedFlight = value;
                DateofSelectedFlight = value.NgayBay.GetDateTimeFormats();
                OnPropertyChanged("SelectedFlight");
            }
        }
        private ObservableCollection<SanBay> departureList_FlightBooking;
        public ObservableCollection<SanBay> DepartureList_FlightBooking
        {
            get { return departureList_FlightBooking; }
            set { departureList_FlightBooking = value; OnPropertyChanged("DepartureList_FlightBooking"); }
        }

        private ObservableCollection<SanBay> destinationList_FlightBooking;
        public ObservableCollection<SanBay> DestinationList_FlightBooking
        {
            get { return destinationList_FlightBooking; }
            set { destinationList_FlightBooking = value; OnPropertyChanged("DestinationList_FlightBooking"); }
        }

        private SanBay selectedDeparture_FlightBooking;
        public SanBay SelectedDeparture_FlightBooking
        {
            get { return selectedDeparture_FlightBooking; }
            set
            {

                if (selectedDeparture_FlightBooking != null)
                    DestinationList_FlightBooking.Add(selectedDeparture_FlightBooking);
                selectedDeparture_FlightBooking = value;
                DestinationList_FlightBooking.Remove(selectedDeparture_FlightBooking);
                OnPropertyChanged("SelectedDeparture_FlightBooking");
            }
        }

        private SanBay selectedDestination_FlightBooking;
        public SanBay SelectedDestination_FlightBooking
        {
            get { return selectedDestination_FlightBooking; }
            set
            {

                if (selectedDestination_FlightBooking != null)
                    departureList_FlightBooking.Add(selectedDestination_FlightBooking);
                selectedDestination_FlightBooking = value;
                departureList_FlightBooking.Remove(selectedDestination_FlightBooking);
                OnPropertyChanged("SelectedDestination_FlightBooking");


            }
        }

        private DateTime selectedDate_FlightBooking = DateTime.UtcNow.AddDays(0);
        public DateTime SelectedDate_FlightBooking
        {
            get { return selectedDate_FlightBooking; }
            set { selectedDate_FlightBooking = value; OnPropertyChanged("SelectedDate_FlightBooking"); }
        }
        public RelayCommand searchFlightCommand_FlightBooking { get; private set; }
        private void searchFlight_FlightBooking(object obj)
        {
            selectedFlight = null;
            OnPropertyChanged("SelectedFlight");
            ObservableCollection<ChuyenBay> temp = new ObservableCollection<ChuyenBay>(BackupList_FlightBooking);
            flightSearchList_FlightBooking = temp;

            if (selectedDestination_FlightBooking != null) 
                for (int i = 0; i < flightSearchList_FlightBooking.Count; i++)
                {
                    if (flightSearchList_FlightBooking[i].SanBayDen.Id == selectedDestination_FlightBooking.Id) continue;
                    flightSearchList_FlightBooking.RemoveAt(i);
                    i = -1;
                }
            if (selectedDeparture_FlightBooking != null)
                for (int i = 0; i < flightSearchList_FlightBooking.Count; i++)
                {
                    if (flightSearchList_FlightBooking[i].SanBayDi.Id == selectedDeparture_FlightBooking.Id) continue;
                    flightSearchList_FlightBooking.RemoveAt(i);
                    i = -1;
                }


            for (int i = 0; i < flightSearchList_FlightBooking.Count; i++)
            {
                if (flightSearchList_FlightBooking[i].NgayBay == SelectedDate_FlightBooking) continue;
                flightSearchList_FlightBooking.RemoveAt(i);
                i = -1;
            }
            OnPropertyChanged("FlightSearchList_FlightBooking");

        }
        public RelayCommand showAllFightsCommand_FlightBooking { get; private set; }
        private void showAllFlights(object obj)
        {
            //selectedFlight = null;
            //OnPropertyChanged("SelectedFlight");
            selectedDate_FlightBooking = DateTime.UtcNow.AddDays(1);
            SanBay temp1 = selectedDestination_FlightBooking;
            SanBay temp2 = selectedDeparture_FlightBooking;
            departureList_FlightBooking.Remove(selectedDeparture_FlightBooking);
            destinationList_FlightBooking.Remove(selectedDestination_FlightBooking);
            if (temp2 != null)
                departureList_FlightBooking.Add(temp2);
            if (temp1 != null)
                destinationList_FlightBooking.Add(temp1);
            selectedDeparture_FlightBooking = null;
            selectedDestination_FlightBooking = null;
            ObservableCollection<ChuyenBay> temp = new ObservableCollection<ChuyenBay>(BackupList_FlightBooking);
            flightSearchList_FlightBooking = temp;
            OnPropertyChanged("FlightSearchList_FlightBooking");
        }
        private ICommand chooseContinueButtonCommand;

        public ICommand ChooseContinueButtonCommand
        {
            get { return chooseContinueButtonCommand; }
        }
        private ButtonDuocChon isContinueButton = new ButtonDuocChon(false);

        public ButtonDuocChon IsContinueButton
        {
            get { return isContinueButton; }
            set { isContinueButton = value; OnPropertyChanged("IsContinueButton"); }
        }
        public void ButtonContinue(object p)
        {
            IsContinueButton = DuocChon;
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;

            ///check sticket types
            ListSticketType.Clear();
            hashtable_AmountSticketType.Clear();
            hashtable_SticketID.Clear();
            if (selectedFlight.SoGheHang1 != "-1")
            {
                if (selectedFlight.SoGheHang1 == "0")
                {
                    hashtable_AmountSticketType.Add("Hạng nhất", "Hết vé");
                    ListSticketType.Add("Hạng nhất");
                }
                else
                {
                    hashtable_AmountSticketType.Add("Hạng nhất", selectedFlight.SoGheHang1);
                    ListSticketType.Add("Hạng nhất");
                }
                hashtable_SticketID.Add("Hạng nhất","A");
            }
            if (selectedFlight.SoGheHang2 != "-1")
            {
                if (selectedFlight.SoGheHang2 == "0")
                {
                    hashtable_AmountSticketType.Add("Thương gia", "Hết vé");
                    ListSticketType.Add("Thương gia");
                }
                else
                {
                    hashtable_AmountSticketType.Add("Thương gia", selectedFlight.SoGheHang2);
                    ListSticketType.Add("Thương gia");
                }
                hashtable_SticketID.Add("Thương gia", "B");
            }
            if (selectedFlight.SoGheHang3 != "-1")
            {
                if (selectedFlight.SoGheHang3 == "0")
                {
                    hashtable_AmountSticketType.Add("Phổ thông đặc biệt", "Hết vé");
                    ListSticketType.Add("Phổ thông đặc biệt");
                }
                else
                {
                    hashtable_AmountSticketType.Add("Phổ thông đặc biệt", selectedFlight.SoGheHang3);
                    ListSticketType.Add("Phổ thông đặc biệt");
                }
                hashtable_SticketID.Add("Phổ thông đặc biệt", "C");
            }
            if (selectedFlight.SoGheHang4 != "-1")
            {
                if (selectedFlight.SoGheHang4 == "0")
                {
                    hashtable_AmountSticketType.Add("Phổ thông", "Hết vé");
                    ListSticketType.Add("Phổ thông");
                }
                else
                {
                    hashtable_AmountSticketType.Add("Phổ thông", selectedFlight.SoGheHang4);
                    ListSticketType.Add("Phổ thông");
                }
                hashtable_SticketID.Add("Phổ thông", "D");
            }
            OnPropertyChanged("ListSticketType");
            SticketTypeAmount = null;
            BookingSticket.FlightID = selectedFlight.MaChuyenBay;



        }
        private BookingSticket bookingSticket;
        public BookingSticket BookingSticket
        {
            get { return bookingSticket; }
            set { bookingSticket = value; OnPropertyChanged("BookingSticket"); }
        }

        private RelayCommand chooseBackButtonCommand;

        public RelayCommand ChooseBackButtonCommand
        {
            get { return chooseBackButtonCommand; }
        }
        public void ButtonBack()
        {
            IsContinueButton = KhongDuocChon;
            IsDuocChon1 = DuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            bookingSticket = new BookingSticket();
            OnPropertyChanged("BookingSticket");
        }

        private Hashtable hashtable_AmountSticketType;
        public Hashtable Hashtable_AmountSticketType
        {
            get { return hashtable_AmountSticketType; }
            set { hashtable_AmountSticketType = value; OnPropertyChanged("Hashtable_AmountSticketType"); }
        }


        private Hashtable hashtable_SticketID;
        public Hashtable Hashtable_SticketID
        {
            get { return hashtable_SticketID; }
            set { hashtable_SticketID = value; OnPropertyChanged("Hashtable_SticketID"); }
        }


        private string sticketType;
        public string SticketType
        {
            get { return sticketType; }
            set
            {
                sticketType = value;
                if (sticketType != null)
                {
                    SticketTypeAmount = hashtable_AmountSticketType[sticketType].ToString();
                    if (hashtable_AmountSticketType[sticketType].ToString() != "0")
                    {
                        BookingSticket.SticketTypeID = hashtable_SticketID[sticketType].ToString();
                        if (SticketType == "Hạng nhất")
                            BookingSticket.Cost = (Int32.Parse(selectedFlight.GiaVeCoBan) * 1.5).ToString() + " VND";
                        else if (SticketType == "Thương gia")
                            BookingSticket.Cost = ((int)(Int32.Parse(selectedFlight.GiaVeCoBan) * 1.3)).ToString() + " VND";
                        else if (SticketType == "Phổ thông đặc biệt")
                            BookingSticket.Cost = ((int)(Int32.Parse(selectedFlight.GiaVeCoBan) * 1.15)).ToString() + " VND";
                        else if (SticketType == "Phổ thông")
                            BookingSticket.Cost = selectedFlight.GiaVeCoBan + " VND";
                    }
                }
                OnPropertyChanged("SticketType");
            }
        }

        private string sticketTypeAmount;
        public string SticketTypeAmount
        {
            get { return sticketTypeAmount; }
            set
            {
                sticketTypeAmount = value;
                OnPropertyChanged("SticketTypeAmount");
            }
        }

        private ObservableCollection<string> listSticketType;
        public ObservableCollection<string> ListSticketType
        {
            get { return listSticketType; }
            set { listSticketType = value; OnPropertyChanged("ListSticketType"); }
        }

        private string requestComboxBackground = "LightSlateGray";
        public string RequestComboxBackground
        {
            get { return requestComboxBackground; }
            set { requestComboxBackground = value; OnPropertyChanged("RequestComboxBackground"); }
        }
        private string[] dateofSelectedFlight;
        public string[] DateofSelectedFlight
        {
            get { return dateofSelectedFlight; }
            set
            {
                dateofSelectedFlight = value;
                OnPropertyChanged("DateofSelectedFlight");

            }
        }

        private bool isSetRequest = false;
        public bool IsSetRequest
        {
            get { return isSetRequest; }
            set
            {
                isSetRequest = value;
                if (isSetRequest == true)

                    RequestComboxBackground = "White";
                else RequestComboxBackground = "LightSlateGray";
                OnPropertyChanged("IsSetRequest");
            }
        }

        private RelayCommand2<object> choosePayButtonCommand;

        public RelayCommand2<object> ChoosePayButtonCommand
        {
            get { return choosePayButtonCommand; }
        }
        private void ButtonPay(object obj)
        {
       

            BookingSticket.BookingSticketID = selectedFlight.MaChuyenBay +"-" + BookingSticketServices.RandomString(6);
            BookingSticketServices.BookingSticketProcess(BookingSticket);
        }

        private bool checkPassangerInfor(object obj)
        {
            if (string.IsNullOrEmpty(bookingSticket.PassengerName) || string.IsNullOrEmpty(bookingSticket.CMND) || string.IsNullOrEmpty(bookingSticket.Address)
                || string.IsNullOrEmpty(bookingSticket.Contact) || SticketTypeAmount == "Hết vé" || string.IsNullOrEmpty(SticketTypeAmount)) return false;
            return true;
           
        }

        #endregion

    }
}

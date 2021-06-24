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
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Configuration;
using Planzy.Models.Users;
using System.Data;
using System.Windows.Media.Imaging;
using Planzy.Resources.Component.CustomMessageBox;
using static Planzy.ThamSoQuyDinh;
using Planzy.Models.LoaiHangGheModel;
using FootballFieldManagement.Views;
using System.Windows;
using Planzy.Models.ChiTietHangGheModel;
using System.Timers;
using LiveCharts;
using LiveCharts.Configurations;
using Planzy.Models.DoanhThuThangModel;
using Planzy.Models.DoanhThuModel;
using System.Windows.Media;
using Planzy.Models.Util;
using System.Printing;
using Planzy.Views;
using System.Windows.Threading;
using System.Net;
using Planzy.Models.BanVe;
using Caliburn.Micro;

namespace Planzy.ViewModels
{
    class PlanzyViewModel : INotifyPropertyChanged
    {
        private static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        Window mainWindow;

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

        #region Khai báo cho biểu đồ
        private DispatcherTimer chuyenBayTimer;

        public void denGioBay(object sender, EventArgs e)
        {
            chuyenBayTimer.Stop();
            foreach (ChuyenBay chuyenBay in ChuyenBaysList)
            {
                if (chuyenBay.NgayBay == DateTime.Now)
                {
                    if (chuyenBay.GioBay.Hour < DateTime.Now.Hour)
                    {
                        if (chuyenBay.IsDaBay == false)
                        {
                            chuyenBay.IsDaBay = true;
                            chuyenBayServices.SuaChuyenBaySql(chuyenBay);//cập nhật sql
                                                                         //tính toán lại báo cáo
                            if (DanhSachNamDaChon == chuyenBay.NgayBay.Year.ToString())
                            {
                                doanhThuThangServices.ThemDoanhThu(chuyenBay);
                            }
                            else
                            {
                                DoanhThuThangServices newDT = new DoanhThuThangServices(chuyenBay.NgayBay.Year.ToString());
                                newDT.ThemDoanhThu(chuyenBay);
                            }
                        }
                    }
                    else if (chuyenBay.GioBay.Hour == DateTime.Now.Hour)
                    {
                        if (chuyenBay.GioBay.Minute <= DateTime.Now.Minute)
                        {
                            if (chuyenBay.IsDaBay == false)
                            {
                                chuyenBay.IsDaBay = true;
                                chuyenBayServices.SuaChuyenBaySql(chuyenBay);//cập nhật sql
                                                                             //tính toán lại báo cáo
                                if (DanhSachNamDaChon == chuyenBay.NgayBay.Year.ToString())
                                {
                                    doanhThuThangServices.ThemDoanhThu(chuyenBay);
                                }
                                else
                                {
                                    DoanhThuThangServices newDT = new DoanhThuThangServices(chuyenBay.NgayBay.Year.ToString());
                                    newDT.ThemDoanhThu(chuyenBay);
                                }
                            }
                        }
                    }
                }
                else if (chuyenBay.NgayBay < DateTime.Now)
                {
                    if (chuyenBay.IsDaBay == false)
                    {
                        chuyenBay.IsDaBay = true;
                        chuyenBayServices.SuaChuyenBaySql(chuyenBay);//cập nhật sql
                                                                     //tính toán lại báo cáo
                        if (DanhSachNamDaChon == chuyenBay.NgayBay.Year.ToString())
                        {
                            doanhThuThangServices.ThemDoanhThu(chuyenBay);
                        }
                        else
                        {
                            DoanhThuThangServices newDT = new DoanhThuThangServices(chuyenBay.NgayBay.Year.ToString());
                            newDT.ThemDoanhThu(chuyenBay);
                        }
                    }
                }

            }
            chuyenBayTimer.Start();
        }
        private DoanhThuThangServices doanhThuThangServices;
        private ChartValues<DoanhThuThang> doanhThuThangs;

        public ChartValues<DoanhThuThang> DoanhThuThangs
        {
            get { return doanhThuThangs; }
            set { doanhThuThangs = value; OnPropertyChanged("DoanhThuThangs"); }
        }
        private List<string> LabelThangDaChons;
        public List<string> labelThangDaChons
        {
            get { return LabelThangDaChons; }
            set { LabelThangDaChons = value; OnPropertyChanged("labelThangDaChons"); }
        }
        private ChartValues<DoanhThu> DoanhThuThangDaChon;
        public ChartValues<DoanhThu> doanhThuThangDaChon
        {
            get { return DoanhThuThangDaChon; }
            set { DoanhThuThangDaChon = value; OnPropertyChanged("doanhThuThangDaChon"); }
        }
        private List<string> LabelThangDaChon;
        public List<string> labelThangDaChon
        {
            get { return LabelThangDaChon; }
            set { LabelThangDaChon = value; OnPropertyChanged("labelThangDaChon"); }
        }
        #endregion


        public PlanzyViewModel(string gmailUser, Window parentWindow)
        {

            sanBayServices = new SanBayService();
            sanBayTrungGianService = new SanBayTrungGianService();
            loaiHangGheServices = new LoaiHangGheServices();
            chiTietHangGheServices = new ChiTietHangGheServices();
            chuyenBayServices = new ChuyenBayServices(sanBayTrungGianService, sanBayServices, chiTietHangGheServices);

            #region biểu đồ
            //doanhThuThangServices = new DoanhThuThangServices(DanhSachNamDaChon,chuyenBayServices);
            doanhThuThangServices = new DoanhThuThangServices(DanhSachNamDaChon);
            DoanhThuThangs = doanhThuThangServices.doanhThuThangs;
            labelThangDaChons = doanhThuThangServices.labels;

            doanhThuThangDaChon = doanhThuThangServices.doanhThuThangs[0].doanhThuServices.doanhThus;
            labelThangDaChon = doanhThuThangServices.doanhThuThangs[0].doanhThuServices.labels;

            //map cho doanh thu nam
            var DoanhThuThangMapper = Mappers.Xy<DoanhThuThang>()
                .X((value, index) => index)
                .Y(value => Convert.ToInt32(value.DoanhThuTrieuDong));
            Charting.For<DoanhThuThang>(DoanhThuThangMapper);
            //map cho doanh thu thang
            var DoanhThuMapper = Mappers.Xy<DoanhThu>()
                .X((value, index) => index)
                .Y(value => Convert.ToInt32(value.DoanhThuTrieuDong));
            Charting.For<DoanhThu>(DoanhThuMapper);

            reviewBieuDoCommand = new RelayCommand(reviewBieuDo);
            troVeBieuDoCommand = new RelayCommand(troVeBieuDo);
            xuatPDFVisualCommand = new RelayCommand(xuatPDFVisual);

            chuyenBayTimer = new DispatcherTimer();
            chuyenBayTimer.Interval = TimeSpan.FromSeconds(2);
            chuyenBayTimer.Tick += denGioBay;
            chuyenBayTimer.Start();
            #endregion

            #region Tham số quy định
            ThamSoQuyDinh.LoadThamSoQuyDinhTuSQL();
            ThoiGianBayToiThieu = ThamSoQuyDinh.THOI_GIAN_BAY_TOI_THIEU;
            SoSanBayTrungGianToiDa = ThamSoQuyDinh.SO_SAN_BAY_TRUNG_GIAN_TOI_DA;
            ThoiGianDungToiThieu = ThamSoQuyDinh.THOI_GIAN_DUNG_TOI_THIEU;
            ThoiGianDungToiDa = ThamSoQuyDinh.THOI_GIAN_DUNG_TOI_DA;
            ThoiGianHuyVeTreNhat = ThamSoQuyDinh.THOI_GIAN_CHAM_NHAT_HUY_VE;
            ThoiGianDatVeTreNhat = ThamSoQuyDinh.THOI_GIAN_CHAM_NHAT_DAT_VE;
            #endregion

            //Thuc

            #region users

            mainWindow = parentWindow;
            LogOutCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { logOut(); });
            UpdateUserCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { updateUser(); });

            userServices = new UserServices();
            listUser = new List<User>(userServices.GetAll());
            user = userServices.getUserByEmail(gmailUser);

            setUI();

            #endregion

            #region sell ticket
            searchFlightCommand_SellTicket = new RelayCommand(searchFlight_SellTicket);
            showAllFightsCommand_SellTicket = new RelayCommand(showAllFlights_SellTicket);
            chooseContinueButton_SellTicketCommand = new RelayCommand2<object>((p) => p != null, ButtonContinue_SellTicket);
            chooseBackButtonCommand_SellTicket = new RelayCommand(ButtonBack_SellTicket);
            choosePayButtonCommand_SellTicket = new RelayCommand2<object>(checkPassangerInfor_SellTicket, ButtonPay_SellTicket);
            #endregion

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
            #region thay đổi quy định
            themSanBayQuyDinhCommand = new RelayCommand(themSanBayQuyDinh);
            xoaSanBayQuyDinhCommand = new RelayCommand(xoaSanBayQuyDinh);
            huyThemSanBayCommand = new RelayCommand(huyThemSanBay);
            xacNhanThemSanBayCommand = new RelayCommand(xacNhanThemSanBay);
            #endregion

            InsertTickTypeCommand_Setting = new RelayCommand2<Window>((p) => { return true; }, (p) => { insertTicket_Setting(); });
            DeleteTickTypeCommand_Setting = new RelayCommand2<Window>((p) => { return true; }, (p) => { deleteTicket_Setting(); });
            ResetTickTypeCommand_Setting = new RelayCommand2<Window>((p) => { return true; }, (p) => { resetTicket_Setting(); });
            SaveCommand_Setting = new RelayCommand2<Window>((p) => { return true; }, (p) => { save_Setting(); });

            chooseDetailFlight_SellTicket = new RelayCommand2<object>(CheckSelected_DetailFlight_SellTicket, ButtonDetailFlight_SellTicket);
            chooseBack_DetailFlightCommand_SellTicket = new RelayCommand(ButtonBack_DetailFlight_SellTicket);

            #region Xử lý giao diện ban đầu
            LoadUIHangGheTheoQuyDinh();
            chonLayoutCommand1 = new RelayCommand(Button1);
            chonLayoutCommand2 = new RelayCommand(Button2);
            chonLayoutCommand3 = new RelayCommand(Button3);
            chonLayoutCommand4 = new RelayCommand(Button4);
            chonLayoutCommand5 = new RelayCommand(Button5);
            chonLayoutCommand6 = new RelayCommand(Button6);
            chonLayoutCommand8 = new RelayCommand(Button8);


            #endregion
            //Thien

            searchFlightCommand = new RelayCommand(searchFlight);
            searchFlightCommand_FlightBooking = new RelayCommand(searchFlight_FlightBooking);
            resetCommand = new RelayCommand(resetSearchList);
            showAllFightsCommand_FlightBooking = new RelayCommand(showAllFlights);
            choosePayButtonCommand = new RelayCommand2<object>(checkPassangerInfor, ButtonPay);
            chooseContinueButtonCommand = new RelayCommand2<object>((p) => p != null, ButtonContinue);
            chooseBackButtonCommand = new RelayCommand(ButtonBack);
            chooseBookedSticketCommand = new RelayCommand(Button_BookedSticket);
            chooseChangeButtonCommand_FlightSearch = new RelayCommand2<object>((p) => p != null, ButtonChange);
            chooseDeleteButtonCommand_BookedSticket = new RelayCommand2<object>(CheckSelected, ButtonDelete_BookedSticket);
            chooseDetailFlight = new RelayCommand2<object>(CheckSelected_DetailFlight, ButtonDetailFlight);

            chooseBack_BookedStickedComamnd = new RelayCommand(ButtonBack_BookedSticket);
            chooseBack_DetailFlightCommand = new RelayCommand(ButtonBack_DetailFlight);



            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }



        public RelayCommand SelectAllCommand { get; private set; }
        public RelayCommand SelectAllCommand2 { get; private set; }



        #region Xử lý chung
        private void LoadData()
        {
            SanbaysList = new List<SanBay>(sanBayServices.GetSanBayHoatDong());
            SanBayDensList = new ObservableCollection<SanBay>(sanBayServices.GetSanBayHoatDong());
            SanBayDisList = new ObservableCollection<SanBay>(sanBayServices.GetSanBayHoatDong());
            SanBaysQuyDinh = new ObservableCollection<SanBay>(sanBayServices.GetSanBayHoatDong());

            SanBayTrungGianSapThemsList = new ObservableCollection<SanBay>(sanBayServices.GetSanBayHoatDong());
            LoaiHangGhesList = new List<LoaiHangGhe>( loaiHangGheServices.GetAll_KhaDung());
            #region tùy chỉnh binding giao diện loại hạng ghế
            for(int i = LoaiHangGhesList.Count;i< 8;i++)
            {
                LoaiHangGhesList.Add(new LoaiHangGhe());
            }    
            #endregion
            //SanBayTrungGiansList = new ObservableCollection<SanBayTrungGian>(sanBayTrungGianService.GetAll());
            ChuyenBaysList = new ObservableCollection<ChuyenBay>(chuyenBayServices.GetAll());

            // Thiên
            FlightSearchList = new ObservableCollection<ChuyenBay>(chuyenBayServices.GetAll());
            //FlightSearchList_FlightBooking = new ObservableCollection<ChuyenBay>(chuyenBayServices.GetFlightBookingList(flightSearchList_FlightBooking));
            //backupList_FlightBooking = new ObservableCollection<ChuyenBay>(chuyenBayServices.GetFlightBookingList(flightSearchList_FlightBooking));
            backupList_FlightBooking = new ObservableCollection<ChuyenBay>(chuyenBayServices.GetFlownFlightList());
            FlightSearchList_FlightBooking = new ObservableCollection<ChuyenBay>(backupList_FlightBooking);
            Departure = new ObservableCollection<SanBay>(sanBayServices.GetSanBayHoatDong());
            Destination = new ObservableCollection<SanBay>(sanBayServices.GetSanBayHoatDong());
            DepartureList_FlightBooking = new ObservableCollection<SanBay>(sanBayServices.GetSanBayHoatDong());
            DestinationList_FlightBooking = new ObservableCollection<SanBay>(sanBayServices.GetSanBayHoatDong());
            listSticketType = new ObservableCollection<string>();
            hashtable_AmountSticketType = new Hashtable();
            hashtable_SticketID = new Hashtable();
            bookingSticket = new BookingSticket();
            ListBookedSticket = new ObservableCollection<BookingSticket>(BookingSticketServices.GetFlightBookingList(user.ID));
            if (ListBookedSticket.Count != 0)
            {
                foreach (BookingSticket ite in ListBookedSticket)
                {
                    string temp1;
                    string temp2;
                    DateTime temp3;
                    ChuyenBayServices.GetFlight(ite.FlightID, out temp1, out temp2, out temp3);
                    ite.Departure = temp1;
                    ite.Destination = temp2;
                    ite.FlownDate = temp3;
                }
                for (int i = 0; i < ListBookedSticket.Count; i++)
                {
                    DateTime bookingDate_check = DateTime.Now.AddDays(0);

                    TimeSpan interval = ListBookedSticket[i].FlownDate.Subtract(bookingDate_check);
                    double count = interval.Days * 24 + interval.Hours + ((interval.Minutes * 100) / 60) * 0.01;
                    if (count < 0)
                    {
                        ListBookedSticket.RemoveAt(i);
                        i--;
                    }
                }
            }




            //Thuc
            DepartureList_SellTicket = new ObservableCollection<SanBay>(sanBayServices.GetSanBayHoatDong());
            DestinationList_SellTicket = new ObservableCollection<SanBay>(sanBayServices.GetSanBayHoatDong());
            backupList_SellTicket = new ObservableCollection<ChuyenBay>(chuyenBayServices.GetFlownFlightList());
            FlightSearchList_SellTicket = new ObservableCollection<ChuyenBay>(backupList_SellTicket);
            listTicketType_SellTicket = new ObservableCollection<string>();
            hashtable_AmountTicketType_SellTicket = new Hashtable();
            hashtable_TicketId_SellTicket = new Hashtable();
            SellTicket = new FlightTicket();
            ListSellTicket = new ObservableCollection<FlightTicket>(FlightTicketServices.GetFlightBookingList(user.ID));
            if (ListSellTicket.Count != 0)
            {
                foreach (FlightTicket ite in ListSellTicket)
                {
                    string temp1;
                    string temp2;
                    DateTime temp3;
                    ChuyenBayServices.GetFlight(ite.FlightId, out temp1, out temp2, out temp3);
                    ite.Departure = temp1;
                    ite.Destination = temp2;
                }
            }

            backupListTicketType_Setting = new List<LoaiHangGhe>();
            LoadSQL();
            ListTicketType_Setting = new ObservableCollection<LoaiHangGhe>(loaiHangGheServices.GetAll_KhaDung());

            /*ListTicketType_Setting = new ObservableCollection<LoaiHangGhe>();
            for(int i = 0; i<backupListTicketType_Setting.Count; i ++)
            {
                ListTicketType_Setting.Add(backupListTicketType_Setting[i]);
            }*/



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
                if (SanBayTrungGiansList != null && SanBayTrungGiansList.Count != 0)
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

                if (SanBayTrungGiansList != null && SanBayTrungGiansList.Count != 0)
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
            IsFocusGiaChuyenBay = "True";
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
        private ButtonDuocChon isDuocChon6 = new ButtonDuocChon(false);
        public ButtonDuocChon IsDuocChon6
        {
            get { return isDuocChon6; }
            set { isDuocChon6 = value; OnPropertyChanged("IsDuocChon6"); }
        }
        private ButtonDuocChon isDuocChon7 = new ButtonDuocChon(false);
        public ButtonDuocChon IsDuocChon7
        {
            get { return isDuocChon7; }
            set { isDuocChon7 = value; OnPropertyChanged("IsDuocChon7"); }
        }

        private ButtonDuocChon isDuocChon8 = new ButtonDuocChon(false);
        public ButtonDuocChon IsDuocChon8
        {
            get { return isDuocChon8; }
            set { isDuocChon8 = value; OnPropertyChanged("IsDuocChon8"); }
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
        private RelayCommand chonLayoutCommand6;

        public RelayCommand ChonLayoutCommand6
        {
            get { return chonLayoutCommand6; }
        }
        private RelayCommand chonLayoutCommand7;

        public RelayCommand ChonLayoutCommand7
        {
            get { return chonLayoutCommand7; }
        }
        private RelayCommand chonLayoutCommand8;

        public RelayCommand ChonLayoutCommand8
        {
            get { return chonLayoutCommand8; }
        }
        public void Button1()
        {
            IsDuocChon1 = DuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;
            IsDuocChon7 = KhongDuocChon;
            IsDetailFlight = KhongDuocChon;
            IsDuocChon8 = KhongDuocChon;
        }
        public void Button2()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = DuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;
            IsDuocChon7 = KhongDuocChon;
            IsDetailFlight = KhongDuocChon;
            IsDuocChon8 = KhongDuocChon;
        }
        public void Button3()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = DuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;
            IsDuocChon7 = KhongDuocChon;
            IsDetailFlight = KhongDuocChon;
            IsDuocChon8 = KhongDuocChon;
        }
        public void Button4()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = DuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;
            IsDuocChon7 = KhongDuocChon;
            IsDetailFlight = KhongDuocChon;
            IsDuocChon8 = KhongDuocChon;
        }
        public void Button5()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = DuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;
            IsDuocChon7 = KhongDuocChon;
            IsDetailFlight = KhongDuocChon;
            IsDuocChon8 = KhongDuocChon;
        }
        public void Button6()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon6 = DuocChon;
            IsDuocChon7 = KhongDuocChon;
            IsDetailFlight = KhongDuocChon;
            IsDuocChon8 = KhongDuocChon;
        }
        public void Button7()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;
            IsDuocChon7 = DuocChon;
            IsDetailFlight = KhongDuocChon;
            IsDuocChon8 = KhongDuocChon;
        }
        public void Button8()
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;
            IsDuocChon7 = KhongDuocChon;
            IsDuocChon8 = DuocChon;
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
        private ObservableCollection<SanBayTrungGian> sanBayTrungGiansList = new ObservableCollection<SanBayTrungGian>();

        public ObservableCollection<SanBayTrungGian> SanBayTrungGiansList
        {
            get { return sanBayTrungGiansList; }
            set
            {
                sanBayTrungGiansList = value;
                if (sanBayTrungGiansList != null)
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
            if (SanBayDenDaChon != null && SanBayDiDaChon != null)
            {
                #region Kiểm tra quy định
                if (SanBayTrungGiansList != null && SanBayTrungGiansList.Count == Convert.ToInt32(ThamSoQuyDinh.SO_SAN_BAY_TRUNG_GIAN_TOI_DA))
                {
                    CustomMessageBox.Show("Tối đa " + ThamSoQuyDinh.SO_SAN_BAY_TRUNG_GIAN_TOI_DA + " sân bay trung gian", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            if (ThoiGianDungSapThem == null || ThoiGianDungSapThem == "")
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                        if (Convert.ToInt32(value) < Convert.ToInt32(ThamSoQuyDinh.THOI_GIAN_DUNG_TOI_THIEU) || Convert.ToInt32(value) > Convert.ToInt32(ThamSoQuyDinh.THOI_GIAN_DUNG_TOI_DA))
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
                        CustomMessageBox.Show("Mã Chuyến Bay không được chứa ký tự đặc biệt", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        public DateTime BlackoutCollection
        {
            get { return blackoutCollection; }
            set { blackoutCollection = value; OnPropertyChanged("BlackoutCollection"); }
        }

        private DateTime ngayBay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);

        public DateTime NgayBay
        {
            get { return ngayBay; }
            set {
                if(value<=DateTime.Now)
                {
                    CustomMessageBox.Show("Ngày bay sớm nhất là: " + ngayBay.ToShortDateString(), "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }    
                ngayBay = value; OnPropertyChanged("NgayBay"); }
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
                        CustomMessageBox.Show("Giá Vé phải là số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        #region Kiểm tra quy định
                        if (Convert.ToInt32(value) < Convert.ToInt32(ThamSoQuyDinh.THOI_GIAN_BAY_TOI_THIEU))
                        {
                            CustomMessageBox.Show("Thời Gian Bay tối thiểu là " + ThamSoQuyDinh.THOI_GIAN_BAY_TOI_THIEU + " phút", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                            thoiGianBay = value;
                        #endregion
                    }
                    else
                    {
                        CustomMessageBox.Show("Thời Gian Bay phải là số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        #region Kiểm tra quy định
                        soGheHang1 = value;
                        #endregion
                    }
                    else
                    {
                        CustomMessageBox.Show("Số ghế là số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                    soGheHang1 = value;
                OnPropertyChanged("SoGheHang1");
            }
        }

        private string soGheHang2;

        public string SoGheHang2
        {
            get { return soGheHang2; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        #region Kiểm tra quy định
                        soGheHang2 = value;
                        #endregion
                    }
                    else
                    {
                        CustomMessageBox.Show("Số ghế là số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                    soGheHang2 = value; OnPropertyChanged("SoGheHang2");
            }
        }
        private string soGheHang3;

        public string SoGheHang3
        {
            get { return soGheHang3; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        #region Kiểm tra quy định
                        soGheHang3 = value;
                        #endregion
                    }
                    else
                    {
                        CustomMessageBox.Show("Số ghế là số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                    soGheHang3 = value; OnPropertyChanged("SoGheHang3");
            }
        }
        private string soGheHang4;

        public string SoGheHang4
        {
            get { return soGheHang4; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        #region Kiểm tra quy định
                        soGheHang4 = value;
                        #endregion
                    }
                    else
                    {
                        CustomMessageBox.Show("Số ghế là số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                    soGheHang4 = value; OnPropertyChanged("SoGheHang4");
            }
        }

        private string soGheHang5;

        public string SoGheHang5
        {
            get { return soGheHang5; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        #region Kiểm tra quy định
                        soGheHang5 = value;
                        #endregion
                    }
                    else
                    {
                        CustomMessageBox.Show("Số ghế là số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                    soGheHang5 = value; OnPropertyChanged("SoGheHang5");
            }
        }

        private string soGheHang6;

        public string SoGheHang6
        {
            get { return soGheHang6; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        #region Kiểm tra quy định
                        soGheHang6 = value;
                        #endregion
                    }
                    else
                    {
                        CustomMessageBox.Show("Số ghế là số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                    soGheHang6 = value; OnPropertyChanged("SoGheHang6");
            }
        }

        private string soGheHang7;

        public string SoGheHang7
        {
            get { return soGheHang7; }
            set
            {

                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        #region Kiểm tra quy định
                        soGheHang7 = value;
                        #endregion
                    }
                    else
                    {
                        CustomMessageBox.Show("Số ghế là số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                    soGheHang7 = value; OnPropertyChanged("SoGheHang7");
            }
        }

        private string soGheHang8;

        public string SoGheHang8
        {
            get { return soGheHang8; }
            set
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        #region Kiểm tra quy định
                        soGheHang1 = value;
                        #endregion
                    }
                    else
                    {
                        CustomMessageBox.Show("Số ghế là số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                    soGheHang8 = value; OnPropertyChanged("SoGheHang8");
            }
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
                else if (chuyenBayDaChon != null && value == null)
                {
                    chuyenBayDaChon = value;
                    IsVisibleSuaChuyenBay = "Hidden";
                    IsVisibleXoaChuyenBay = "Hidden";
                }
                else if (chuyenBayDaChon != null && value != null)
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
        private string isVisibleXoaChuyenBay = "Hidden";

        public string IsVisibleXoaChuyenBay
        {
            get { return isVisibleXoaChuyenBay; }
            set { isVisibleXoaChuyenBay = value; OnPropertyChanged("IsVisibleXoaChuyenBay"); }
        }
        private string isVisibleNhanLichChuyenBay = "Visible";

        public string IsVisibleNhanLichChuyenBay
        {
            get { return isVisibleNhanLichChuyenBay; }
            set { isVisibleNhanLichChuyenBay = value; OnPropertyChanged("IsVisibleNhanLichChuyenBay"); }
        }
        private string isVisibleHuyThaoTac = "Visible";

        public string IsVisibleHuyThaoTac
        {
            get { return isVisibleHuyThaoTac; }
            set { isVisibleHuyThaoTac = value; OnPropertyChanged("IsVisibleHuyThaoTac"); }
        }
        private string isReadOnlyMaChuyenBay = "True";

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
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusMaChuyenBay = "True";
                return;
            }

            if (GiaVeCoBan == "" || GiaVeCoBan == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusGiaChuyenBay = "True";
                return;
            }

            if (ThoiGianBay == "" || ThoiGianBay == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusThoiGianBay = "True";
                return;
            }

            if (SanBayDiDaChon == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusSanBayDi = "True";
                return;
            }

            if (SanBayDenDaChon == null)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusSanBayDen = "True";
                return;
            }

            if ((SoGheHang1 == null || SoGheHang1 == "") && Convert.ToInt32(ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE) == 1)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusSoGheHang1 = "True";
                return;
            }

            if ((SoGheHang2 == null || SoGheHang2 == "") && Convert.ToInt32(ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE) > 1)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusSoGheHang2 = "True";
                return;
            }

            if ((SoGheHang3 == null || SoGheHang3 == "") && Convert.ToInt32(ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE) > 2)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusSoGheHang3 = "True";
                return;
            }

            if ((SoGheHang4 == null || SoGheHang4 == "") && Convert.ToInt32(ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE) > 3)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusSoGheHang4 = "True";
                return;
            }

            if ((SoGheHang5 == null || SoGheHang5 == "") && Convert.ToInt32(ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE) > 4)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusSoGheHang5 = "True";
                return;
            }

            if ((SoGheHang6 == null || SoGheHang6 == "") && Convert.ToInt32(ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE) > 5)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusSoGheHang6 = "True";
                return;
            }

            if ((SoGheHang7 == null || SoGheHang7 == "") && Convert.ToInt32(ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE) > 6)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusSoGheHang6 = "True";
                return;
            }

            if ((SoGheHang8 == null || SoGheHang8 == "") && Convert.ToInt32(ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE) > 7)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsFocusSoGheHang8 = "True";
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
            ChuyenBayHienTai.NgayBay = ChuyenBayHienTai.NgayBay.AddHours(GioBay.Hour);
            ChuyenBayHienTai.NgayBay = ChuyenBayHienTai.NgayBay.AddMinutes(GioBay.Minute);
            ChuyenBayHienTai.ThoiGianBay = ThoiGianBay;
            ChuyenBayHienTai.SoLoaiHangGhe = Convert.ToInt32(ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE);

            for (int i = 0; i < ChuyenBayHienTai.SoLoaiHangGhe; i++)
            {
                switch (i)
                {
                    case 0: ChiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang1, loaiHangGhesList[i].TenLoaiHangGhe, loaiHangGhesList[i].TyLe)); break;
                    case 1: ChiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang2, loaiHangGhesList[i].TenLoaiHangGhe, loaiHangGhesList[i].TyLe)); break;
                    case 2: ChiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang3, loaiHangGhesList[i].TenLoaiHangGhe, loaiHangGhesList[i].TyLe)); break;
                    case 3: ChiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang4, loaiHangGhesList[i].TenLoaiHangGhe, loaiHangGhesList[i].TyLe)); break;
                    case 4: ChiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang5, loaiHangGhesList[i].TenLoaiHangGhe, loaiHangGhesList[i].TyLe)); break;
                    case 5: ChiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang6, loaiHangGhesList[i].TenLoaiHangGhe, loaiHangGhesList[i].TyLe)); break;
                    case 6: ChiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang7, loaiHangGhesList[i].TenLoaiHangGhe, loaiHangGhesList[i].TyLe)); break;
                    default: ChiTietHangGhesList.Add(new ChiTietHangGhe(ChuyenBayHienTai.MaChuyenBay, loaiHangGhesList[i].MaLoaiHangGhe, SoGheHang8, loaiHangGhesList[i].TenLoaiHangGhe, loaiHangGhesList[i].TyLe)); break;
                }
            }
            ChuyenBayHienTai.ChiTietHangGhesList = ChiTietHangGhesList;

            #region Cập nhật dữ liệu
            if (chuyenBayServices.Add(ChuyenBayHienTai))
            {
                ChuyenBaysList.Add(ChuyenBayHienTai);
                FlightSearchList_FlightBooking.Add(ChuyenBayHienTai);
                FlightSearchList_SellTicket.Add(ChuyenBayHienTai);
                FlightSearchList.Add(ChuyenBayHienTai);
            }
            else
            {
                CustomMessageBox.Show("Mã Chuyến Bay đã tồn tại", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                MaChuyenBay = "";
                IsFocusMaChuyenBay = "True";
                //resetNhapChuyenBay();
                return;
            }
            sanBayTrungGianService.ThemListSanBayTrungGian(SanBayTrungGiansList);
            chiTietHangGheServices.ThemListChiTietHangGhe(ChiTietHangGhesList);
            #endregion

            ChuyenBayHienTai = new ChuyenBay();
            resetNhapChuyenBay();
            CustomMessageBox.Show("Nhận lịch bay thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        private ObservableCollection<SanBayTrungGian> sanBayTrungGiansListCu;

        public ObservableCollection<SanBayTrungGian> SanBayTrungGiansListCu
        {
            get { return sanBayTrungGiansListCu; }
            set { sanBayTrungGiansListCu = value; OnPropertyChanged("SanBayTrungGiansListCu"); }
        }
        private bool isDangSua = false;
        private string maChuyenBayDaChonTrongList;
        public void suaChuyenBay()
        {
            //chuyến bay đã bán vé không thể chỉnh sửa
            foreach (ChiTietHangGhe chiTietHangGhe in ChuyenBayDaChon.ChiTietHangGhesList)
            {
                if (Convert.ToInt32(chiTietHangGhe.SoLuongGheConLai) < Convert.ToInt32(chiTietHangGhe.SoLuongGhe))
                {
                    CustomMessageBox.Show("Chuyến bay đã bán vé, không thể sửa chữa", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            LoadUIHangGheTheoChuyenBay(ChuyenBayDaChon.SoLoaiHangGhe.ToString());
            for(int i = 0;i<ChuyenBayDaChon.ChiTietHangGhesList.Count;i++)
            {
                LoaiHangGhesList[i].TenLoaiHangGhe = ChuyenBayDaChon.ChiTietHangGhesList[i].TenLoaiHangGhe;
            }    
            // kiểm tra rằng chuyến bay đang sửa chửa để hiện thông báo
            maChuyenBayDaChonTrongList = ChuyenBayDaChon.MaChuyenBay;
            isDangSua = true;
            IsReadOnlyMaChuyenBay = "False";
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

            if (SoGheHang1 == null || SoGheHang1 == "")
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusSoGheHang1 = "True";
                return;
            }

            if ((SoGheHang2 == null || SoGheHang2 == "") && ChuyenBayDaChon.SoLoaiHangGhe > 1)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusSoGheHang2 = "True";
                return;
            }

            if ((SoGheHang3 == null || SoGheHang3 == "") && ChuyenBayDaChon.SoLoaiHangGhe > 2)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusSoGheHang3 = "True";
                return;
            }

            if ((SoGheHang4 == null || SoGheHang4 == "") && ChuyenBayDaChon.SoLoaiHangGhe > 3)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusSoGheHang4 = "True";
                return;
            }

            if ((SoGheHang5 == null || SoGheHang5 == "") && ChuyenBayDaChon.SoLoaiHangGhe > 4)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusSoGheHang5 = "True";
                return;
            }

            if ((SoGheHang6 == null || SoGheHang6 == "") && ChuyenBayDaChon.SoLoaiHangGhe > 5)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusSoGheHang6 = "True";
                return;
            }

            if ((SoGheHang7 == null || SoGheHang7 == "") && ChuyenBayDaChon.SoLoaiHangGhe > 6)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusSoGheHang6 = "True";
                return;
            }

            if ((SoGheHang8 == null || SoGheHang8 == "") && ChuyenBayDaChon.SoLoaiHangGhe > 7)
            {
                CustomMessageBox.Show("Vui lòng điền đầy đủ", "Nhắc Nhở");
                IsFocusSoGheHang8 = "True";
                return;
            }
            #endregion
            foreach (ChuyenBay chuyenBay in ChuyenBaysList)
            {
                if (chuyenBay.MaChuyenBay == maChuyenBayDaChonTrongList)
                    ChuyenBayDaChon = chuyenBay;
            }
            string maChuyenBayCu = ChuyenBayDaChon.MaChuyenBay;
            if (maChuyenBayCu == MaChuyenBay)
            {
                IsReadOnlyMaChuyenBay = "True";

                ChuyenBayDaChon.GiaVeCoBan = GiaVeCoBan;
                ChuyenBayDaChon.NgayBay = NgayBay;
                ChuyenBayDaChon.GioBay = GioBay;
                ChuyenBayHienTai.NgayBay = ChuyenBayHienTai.NgayBay.AddHours(GioBay.Hour);
                ChuyenBayHienTai.NgayBay = ChuyenBayHienTai.NgayBay.AddMinutes(GioBay.Minute);
                ChuyenBayDaChon.ThoiGianBay = ThoiGianBay;
                ChuyenBayDaChon.SanBayDi = SanBayDiDaChon;
                ChuyenBayDaChon.SanBayDen = SanBayDenDaChon;
                if (SanBayTrungGiansList.Count != 0)
                {
                    SanBayTrungGiansList[0].MaSanBayTruoc = SanBayDiDaChon.Id;
                    SanBayTrungGiansList[SanBayTrungGiansList.Count - 1].MaSanBaySau = SanBayDenDaChon.Id;
                }
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
                if (ChuyenBayDaChon.ChiTietHangGhesList != null)
                    foreach (ChiTietHangGhe chiTietHangGhe in ChuyenBayDaChon.ChiTietHangGhesList)
                    {
                        chiTietHangGhe.SoLuongGheConLai = chiTietHangGhe.SoLuongGhe;
                    }
                #region
                for (int i = 0; i < FlightSearchList_FlightBooking.Count; i++)
                {
                    if (FlightSearchList_FlightBooking[i].MaChuyenBay == ChuyenBayDaChon.MaChuyenBay)
                    {
                        FlightSearchList_FlightBooking.RemoveAt(i);
                        FlightSearchList_FlightBooking.Add(ChuyenBayDaChon);
                    }
                }
                for (int i = 0; i < FlightSearchList_SellTicket.Count; i++)
                {
                    if (FlightSearchList_SellTicket[i].MaChuyenBay == ChuyenBayDaChon.MaChuyenBay)
                    {
                        FlightSearchList_SellTicket.RemoveAt(i);
                        FlightSearchList_SellTicket.Add(ChuyenBayDaChon);
                    }
                }
                for (int i = 0; i < FlightSearchList.Count; i++)
                {
                    if (FlightSearchList[i].MaChuyenBay == ChuyenBayDaChon.MaChuyenBay)
                    {
                        FlightSearchList.RemoveAt(i);
                        FlightSearchList.Add(ChuyenBayDaChon);
                    }
                }
                #endregion
                LoaiHangGhesList = new List<LoaiHangGhe>(loaiHangGheServices.GetAll_KhaDung());
                #region tùy chỉnh binding giao diện loại hạng ghế
                for (int i = LoaiHangGhesList.Count; i < 8; i++)
                {
                    LoaiHangGhesList.Add(new LoaiHangGhe());
                }
                #endregion
                #region Cập nhập dữ liệu
                LoadUIHangGheTheoQuyDinh();
                chuyenBayServices.Update(ChuyenBayDaChon);
                sanBayTrungGianService.ClearSpecializeSanBay(MaChuyenBay);
                sanBayTrungGianService.ThemListSanBayTrungGian(SanBayTrungGiansList);
                chiTietHangGheServices.Delete(MaChuyenBay);
                chiTietHangGheServices.ThemListChiTietHangGhe(ChuyenBayDaChon.ChiTietHangGhesList);
                #endregion

                CustomMessageBox.Show("Lưu thành công", "Thông báo", System.Windows.MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
            IsVisible = "Hidden";
            IsDropDown = "False";
            ChuyenBayDaChon = null;
            resetNhapChuyenBay();
            SanBayTrungGiansListCu = null;
            
        }
        private void xoaChuyenBay()
        {
            MessageBoxResult rs = CustomMessageBox.Show("Bạn chắc chắn muốn xóa", "Cảnh báo", System.Windows.MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            foreach (ChiTietHangGhe chiTietHangGhe in ChuyenBayDaChon.ChiTietHangGhesList)
            {
                if (Convert.ToInt32(chiTietHangGhe.SoLuongGheConLai) < Convert.ToInt32(chiTietHangGhe.SoLuongGhe) && ChuyenBayDaChon.IsDaBay == false)
                {
                    CustomMessageBox.Show("Chuyến bay đã bán vé và chưa bay, không thể xóa", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                    #region reset giao diện
                    if (isDangSua)
                    {
                        ChuyenBayDaChon.SanBayTrungGian = SanBayTrungGiansListCu;
                        isDangSua = false;
                        IsReadOnlyMaChuyenBay = "True";
                    }
                    LoadUIHangGheTheoQuyDinh();
                    IsVisibleNhanLichChuyenBay = "Visible";
                    IsVisibleSuaChuyenBay = "Hidden";
                    IsVisibleLuuChuyenBay = "Hidden";
                    ChuyenBayDaChon = null;
                    resetNhapChuyenBay();
                    #endregion
                    return;
                }
            }
            if (rs == MessageBoxResult.OK && chuyenBayServices.Delete(ChuyenBayDaChon.MaChuyenBay, sanBayTrungGianService,chiTietHangGheServices,ListBookedSticket))
            {
                ChuyenBay chuyenBay = ChuyenBayDaChon;
                ChuyenBaysList.Remove(ChuyenBayDaChon);
                FlightSearchList_FlightBooking.Remove(chuyenBay);
                FlightSearchList_SellTicket.Remove(chuyenBay);
                FlightSearchList.Remove(chuyenBay);
                ChuyenBayDaChon = null;
                #region reset giao diện
                if (isDangSua)
                {
                    ChuyenBayDaChon.SanBayTrungGian = SanBayTrungGiansListCu;
                    isDangSua = false;
                    IsReadOnlyMaChuyenBay = "True";
                }
                LoadUIHangGheTheoQuyDinh();
                IsVisibleNhanLichChuyenBay = "Visible";
                IsVisibleSuaChuyenBay = "Hidden";
                IsVisibleLuuChuyenBay = "Hidden";
                ChuyenBayDaChon = null;
                resetNhapChuyenBay();
                #endregion
            }

        }
        public void resetNhapChuyenBay()
        {
            SanBayDenDaChon = null;
            SanBayDiDaChon = null;
            SanBayTrungGiansList = new ObservableCollection<SanBayTrungGian>();
            ChiTietHangGhesList = new List<ChiTietHangGhe>();
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
            MessageBoxResult rs = CustomMessageBox.Show("Bạn chắc chắn muốn hủy", "Cảnh báo", System.Windows.MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (rs == MessageBoxResult.OK)
            {
                if (isDangSua)
                {
                    ChuyenBayDaChon.SanBayTrungGian = SanBayTrungGiansListCu;
                    isDangSua = false;
                    IsReadOnlyMaChuyenBay = "True";
                }
                LoadUIHangGheTheoQuyDinh();
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
            if (ThamSoQuyDinh.SO_LUONG_CAC_HANG_VE == "1")
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
            LoaiHangGhesList = new List<LoaiHangGhe>(loaiHangGheServices.GetAll_KhaDung());
            #region tùy chỉnh binding giao diện loại hạng ghế
            for (int i = LoaiHangGhesList.Count; i < 8; i++)
            {
                LoaiHangGhesList.Add(new LoaiHangGhe());
            }
            #endregion
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
            IsDuocChon1 = DuocChon;
            
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;
            IsDetailFlight = KhongDuocChon;

            ListSticketType.Clear();
            hashtable_AmountSticketType.Clear(); // Dictionary from Name to Amount
            hashtable_SticketID.Clear(); /// Dictionary from Name to ID
            foreach (ChiTietHangGhe ite in selectedFlight.ChiTietHangGhesList)
            {
                if (ite.SoLuongGheConLai == "0")
                {
                    hashtable_AmountSticketType.Add(ite.TenLoaiHangGhe, "Hết vé");
                    hashtable_SticketID.Add(ite.TenLoaiHangGhe, ite.MaLoaiHangGhe);
                }
                else
                {
                    hashtable_AmountSticketType.Add(ite.TenLoaiHangGhe, ite.SoLuongGheConLai);
                    hashtable_SticketID.Add(ite.TenLoaiHangGhe, ite.MaLoaiHangGhe);
                }
                ListSticketType.Add(ite.TenLoaiHangGhe);
            }
            OnPropertyChanged("ListSticketType");
            SticketTypeAmount = null;
            BookingSticket.FlightID = selectedFlight.MaChuyenBay;
            BookingSticket.FlownDate = selectedFlight.NgayBay;

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
            IsDuocChon6 = KhongDuocChon;
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
                        //if (SticketType == "Hạng nhất")
                        //    BookingSticket.Cost = (Int32.Parse(selectedFlight.GiaVeCoBan) * 1.5).ToString() + " VND";
                        //else if (SticketType == "Thương gia")
                        //    BookingSticket.Cost = ((int)(Int32.Parse(selectedFlight.GiaVeCoBan) * 1.3)).ToString() + " VND";
                        //else if (SticketType == "Phổ thông đặc biệt")
                        //    BookingSticket.Cost = ((int)(Int32.Parse(selectedFlight.GiaVeCoBan) * 1.15)).ToString() + " VND";
                        //else if (SticketType == "Phổ thông")
                        //    BookingSticket.Cost = selectedFlight.GiaVeCoBan + " VND";
                        foreach (ChiTietHangGhe temp in selectedFlight.ChiTietHangGhesList)
                        {
                            if (temp.TenLoaiHangGhe == sticketType)
                            {
                                BookingSticket.Cost = (Int32.Parse(selectedFlight.GiaVeCoBan) * Int32.Parse(temp.TyLe)).ToString() + " VND";
                            }
                        }

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

            DateTime bookingDate_check = DateTime.Now.AddDays(0);
            TimeSpan interval = selectedFlight.NgayBay.Subtract(bookingDate_check);
            double count = interval.Days * 24 + interval.Hours + ((interval.Minutes * 100) / 60) * 0.01;
            if (count < float.Parse(ThamSoQuyDinh.THOI_GIAN_CHAM_NHAT_DAT_VE))
            {
                CustomMessageBox.Show("Đã quá hạn đặt vé cho chuyến bay này !", "Thông báo");
                return;
            }
            BookingSticket.BookingSticketID = selectedFlight.MaChuyenBay + "-" + BookingSticketServices.RandomString(6);
            BookingSticket.FlownDate = selectedFlight.NgayBay;
            BookingSticketServices.BookingSticketProcess(BookingSticket, user.ID);
            hashtable_AmountSticketType.Remove(sticketType);
            if (SticketTypeAmount != "0")
                hashtable_AmountSticketType.Add(sticketType, (int.Parse(SticketTypeAmount) - 1).ToString());
            for (int i = 0; i < selectedFlight.ChiTietHangGhesList.Count; i++)
            {
                if (SelectedFlight.ChiTietHangGhesList[i].TenLoaiHangGhe == sticketType)
                    SelectedFlight.ChiTietHangGhesList[i].SoLuongGheConLai = (Int32.Parse(SelectedFlight.ChiTietHangGhesList[i].SoLuongGheConLai) - 1).ToString();
            }
            OnPropertyChanged("SelectedFlight");
            SticketTypeAmount = hashtable_AmountSticketType[sticketType].ToString();
            OnPropertyChanged("SticketTypeAmount");
            OnPropertyChanged("Hashtable_AmountSticketType");
            CustomMessageBox.Show("Đặt vé thành công !", "Thông báo", MessageBoxButton.OK);
            string temp1;
            string temp2;
            DateTime temp3;
            ChuyenBayServices.GetFlight(BookingSticket.FlightID, out temp1, out temp2, out temp3);
            BookingSticket.Departure = temp1;
            BookingSticket.Destination = temp2;
            BookingSticket.FlownDate = temp3;
            ListBookedSticket.Add(BookingSticket);
            //// Set lai ve moi
            BookingSticket = new BookingSticket();
            BookingSticket.Departure = selectedFlight.SanBayDi.ToString();
            BookingSticket.Destination = selectedFlight.SanBayDen.ToString();
            BookingSticket.FlightID = selectedFlight.MaChuyenBay;
            BookingSticket.FlownDate = selectedFlight.NgayBay;
            //if (SticketType == "Hạng nhất")
            //    BookingSticket.Cost = (Int32.Parse(selectedFlight.GiaVeCoBan) * 1.5).ToString() + " VND";
            //else if (SticketType == "Thương gia")
            //    BookingSticket.Cost = ((int)(Int32.Parse(selectedFlight.GiaVeCoBan) * 1.3)).ToString() + " VND";
            //else if (SticketType == "Phổ thông đặc biệt")
            //    BookingSticket.Cost = ((int)(Int32.Parse(selectedFlight.GiaVeCoBan) * 1.15)).ToString() + " VND";
            //else if (SticketType == "Phổ thông")
            //    BookingSticket.Cost = selectedFlight.GiaVeCoBan + " VND";
            foreach (ChiTietHangGhe temp in selectedFlight.ChiTietHangGhesList)
            {
                BookingSticket.SticketTypeID = hashtable_SticketID[sticketType].ToString();
                if (temp.TenLoaiHangGhe == sticketType)
                {
                    BookingSticket.Cost = (Int32.Parse(selectedFlight.GiaVeCoBan) * Int32.Parse(temp.TyLe)).ToString() + " VND";
                }
            }
            BookingSticket.SticketTypeID = hashtable_SticketID[sticketType].ToString();
            ////////

            OnPropertyChanged("ListBookedSticket");

        }

        private bool checkPassangerInfor(object obj)
        {
            if (string.IsNullOrEmpty(bookingSticket.PassengerName) || string.IsNullOrEmpty(bookingSticket.CMND) || string.IsNullOrEmpty(bookingSticket.Address)
                || string.IsNullOrEmpty(bookingSticket.Contact) || SticketTypeAmount == "Hết vé" || string.IsNullOrEmpty(SticketTypeAmount)) return false;
            return true;
        }

        private RelayCommand2<object> chooseChangeButtonCommand_FlightSearch;

        public RelayCommand2<object> ChooseChangeButtonCommand_FlightSearch
        {
            get { return chooseChangeButtonCommand_FlightSearch; }
        }
        private void ButtonChange(object obj)
        {
            ChuyenBay selectedFlightToChange = obj as ChuyenBay;
            if (selectedFlightToChange != null || selectedFlightToChange.IsDaBay == false)
            {

                Button4();
                foreach (ChuyenBay chuyenBay in ChuyenBaysList)
                {
                    if (chuyenBay.MaChuyenBay == selectedFlightToChange.MaChuyenBay)
                    {
                        chuyenBayDaChon = chuyenBay;
                    }
                }
                suaChuyenBay();
                if (isDangSua == false)
                    Button3();
            }
            else
            {
                CustomMessageBox.Show("Không Thể Sửa Chuyến Bay Đã Bay !", "Thông báo", MessageBoxButton.OK);
            }
        }




        #endregion

        #region Booked Sticket Manager 
        public RelayCommand chooseBookedSticketCommand { get; private set; }
        private void Button_BookedSticket(object obj)
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;
            IsDuocChon7 = DuocChon;
        }
        public RelayCommand chooseBack_BookedStickedComamnd { get; private set; }

        private void ButtonBack_BookedSticket(object obj)
        {
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon6 = DuocChon;
            IsDuocChon7 = KhongDuocChon;
        }


        private ObservableCollection<BookingSticket> listBookedSticket;
        public ObservableCollection<BookingSticket> ListBookedSticket
        {
            get { return listBookedSticket; }
            set { listBookedSticket = value; OnPropertyChanged("ListBookedSticket"); }
        }

        private RelayCommand2<object> chooseDeleteButtonCommand_BookedSticket;

        public RelayCommand2<object> ChooseDeleteButtonCommand_BookedSticket
        {
            get { return chooseDeleteButtonCommand_BookedSticket; }
        }

        private bool CheckSelected(object obj)
        {
            BookingSticket selected = obj as BookingSticket;
            if (selected != null) return true;
            return false;
        }
        private void UpdateList(string FlightID, string stickerType)
        {
            for (int i = 0; i < FlightSearchList_FlightBooking.Count; i++)
            {
                if (FlightSearchList_FlightBooking[i].MaChuyenBay == FlightID)
                {
                    for (int j = 0; j < FlightSearchList_FlightBooking[i].ChiTietHangGhesList.Count; j++)
                    {
                        if (FlightSearchList_FlightBooking[i].ChiTietHangGhesList[j].MaLoaiHangGhe == stickerType)
                            FlightSearchList_FlightBooking[i].ChiTietHangGhesList[j].SoLuongGheConLai = (Int32.Parse(FlightSearchList_FlightBooking[i].ChiTietHangGhesList[j].SoLuongGheConLai) + 1).ToString();
                        OnPropertyChanged("FlightSearchList_FlightBooking");
                        OnPropertyChanged("FlightSearchList");
                    }
                    break;
                }
            }
            //for (int i = 0;i < FlightSearchList.Count;i++)
            //{
            //    if (FlightSearchList[i].MaChuyenBay == FlightID)
            //    {
            //        for (int j = 0; j < FlightSearchList[i].ChiTietHangGhesList.Count; j++)
            //        {
            //            if (FlightSearchList[i].ChiTietHangGhesList[j].MaLoaiHangGhe == stickerType)
            //                FlightSearchList[i].ChiTietHangGhesList[j].SoLuongGheConLai = (Int32.Parse(FlightSearchList[i].ChiTietHangGhesList[j].SoLuongGheConLai) + 1).ToString();
            //            OnPropertyChanged("FlightSearchList");
            //        }
            //        break;
            //    }
            //}
        }

        private void ButtonDelete_BookedSticket(object obj)
        {
            BookingSticket selected = obj as BookingSticket;
            DateTime bookingDate_check = DateTime.Now.AddDays(0);

            TimeSpan interval = selected.FlownDate.Subtract(bookingDate_check);
            double count = interval.Days * 24 + interval.Hours + ((interval.Minutes * 100) / 60) * 0.01;
            if (count < float.Parse(ThamSoQuyDinh.THOI_GIAN_CHAM_NHAT_HUY_VE))
            {
                CustomMessageBox.Show("Đã quá hạn hủy vé cho chuyến bay này !", "Thông báo");
                return;
            }

            string temp = String.Format("Bạn có muốn hủy vé  \"{0}\"?", selected.BookingSticketID);
            MessageBoxResult result = MessageBox.Show(temp, "Thông báo", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        if (selected != null)
                            ListBookedSticket.Remove(selected);
                        OnPropertyChanged("ListBookedSticket");
                        UpdateList(selected.FlightID, selected.SticketTypeID);
                        BookingSticketServices.DeletingSticketProcess(selected);
                        CustomMessageBox.Show("Xóa Thành Công", "Planzy Thông Báo !");
                        break;
                    }
                case MessageBoxResult.No:
                    {
                        CustomMessageBox.Show("Oh well, too bad!", "My App");
                        break;
                    }

            }



        }

        #endregion

        #region Flight Detail

        private ButtonDuocChon isDetailFlight = new ButtonDuocChon(false);

        public ButtonDuocChon IsDetailFlight
        {
            get { return isDetailFlight; }
            set { isDetailFlight = value; OnPropertyChanged("IsDetailFlight"); }
        }
        public void ButtonDetailFlight()
        {
            IsDetailFlight = DuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon1 = DuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;
            IsDuocChon7 = KhongDuocChon;
        }
        private ButtonDuocChon isBackButton_DetailFlight = new ButtonDuocChon(false);

        public ButtonDuocChon IsBackButton_DetailFlight
        {
            get { return isBackButton_DetailFlight; }
            set { isBackButton_DetailFlight = value; OnPropertyChanged("IsBackButton_DetailFlight"); }
        }
        public RelayCommand chooseBack_DetailFlightCommand { get; private set; }
        public void ButtonBack_DetailFlight()
        {
            if (IsDuocChon3.NewVisibility == "Visible")
            {
                IsContinueButton = KhongDuocChon;
                IsDuocChon1 = KhongDuocChon;
                IsDetailFlight = KhongDuocChon;
                IsDuocChon2 = KhongDuocChon;
                IsDuocChon3 = DuocChon;
                IsDuocChon4 = KhongDuocChon;
                IsDuocChon5 = KhongDuocChon;
                IsDuocChon6 = KhongDuocChon;
                IsDuocChon7 = KhongDuocChon;
            }
            else
            {
                IsContinueButton = KhongDuocChon;
                IsDuocChon1 = DuocChon;
                IsDetailFlight = KhongDuocChon;
                IsDuocChon2 = KhongDuocChon;
                IsDuocChon3 = KhongDuocChon;
                IsDuocChon4 = KhongDuocChon;
                IsDuocChon5 = KhongDuocChon;
                IsDuocChon6 = KhongDuocChon;
                IsDuocChon7 = KhongDuocChon;
            }
        }


        public BindableCollection<SanBayTrungGian> intermediaryAirport;
        public BindableCollection<SanBayTrungGian> IntermediaryAirport
        {
            get { return intermediaryAirport; }
            set { intermediaryAirport = value; OnPropertyChanged("IntermediaryAirport"); }
        }
        public BindableCollection<ChiTietHangGhe> detailTypeSticket_DetailFlight;
        public BindableCollection<ChiTietHangGhe> DetailTypeSticket_DetailFlight
        {
            get { return detailTypeSticket_DetailFlight; }
            set { detailTypeSticket_DetailFlight = value; OnPropertyChanged("DetailTypeSticket_DetailFlight"); }
        }
        private RelayCommand2<object> chooseDetailFlight;

        public RelayCommand2<object> ChooseDetailFlight
        {
            get { return chooseDetailFlight; }
        }
        private bool CheckSelected_DetailFlight(object obj)
        {
            return true;
            ChuyenBay selected = obj as ChuyenBay;
            if (selected != null) return true;
            return false;

        }

        private void ButtonDetailFlight(object obj)
        {
            if (IsDuocChon3.NewVisibility == "Visible")
            {
                IsDetailFlight = DuocChon;
                IsContinueButton = KhongDuocChon;
                IsDuocChon1 = KhongDuocChon;
                IsDuocChon2 = KhongDuocChon;
                IsDuocChon3 = DuocChon;
                IsDuocChon4 = KhongDuocChon;
                IsDuocChon5 = KhongDuocChon;
                IsDuocChon6 = KhongDuocChon;
                IsDuocChon7 = KhongDuocChon;
                ChuyenBay selected = obj as ChuyenBay;
                SelectedFlight = selected;
                intermediaryAirport = new BindableCollection<SanBayTrungGian>(selectedFlight.SanBayTrungGian);
                DetailTypeSticket_DetailFlight = new BindableCollection<ChiTietHangGhe>(selectedFlight.ChiTietHangGhesList);
                OnPropertyChanged("IntermediaryAirport");
                OnPropertyChanged("DetailTypeSticket_DetailFlight");
            }
            else
            {
                ButtonDetailFlight();
                ChuyenBay selected = obj as ChuyenBay;
                SelectedFlight = selected;
                intermediaryAirport = new BindableCollection<SanBayTrungGian>(selectedFlight.SanBayTrungGian);
                DetailTypeSticket_DetailFlight = new BindableCollection<ChiTietHangGhe>(selectedFlight.ChiTietHangGhesList);
                OnPropertyChanged("IntermediaryAirport");
                OnPropertyChanged("DetailTypeSticket_DetailFlight");
            }
        }

        #endregion

        #region user



        private User user;

        private UserServices userServices;
        private List<User> listUser;
        public ICommand UpdateUserCommand { get; set; }
        public ICommand LogOutCommand { get; set; }

        void updateUser()
        {
            timer.Stop();
            UpdateInforUser updateInforUser = new UpdateInforUser(user, mainWindow);
            updateInforUser.ShowDialog();
            timer.Start();
            userServices.updateUserServices();
            user = userServices.getUserByEmail(user.Gmail);
            setUI();
        }
        void logOut()
        {
            Login login = new Login();
            login.Show();
            mainWindow.Close();
        }

        private Image profilePic;
        public Image ProfillePic
        {
            get { return profilePic; }
            set
            {
                profilePic = value;
                OnPropertyChanged("ProfilePic");
            }
        }
        public void loadProfilePic()
        {

            BitmapImage src = new BitmapImage();
            src.BeginInit();

            src.UriSource = new Uri(@"D:\profilePic.png", UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            profilePic = new Image();
            profilePic.Source = src;

        }


        public void setUI()
        {
            UserName = user.Name;
            Gmail = user.Gmail;
            CMND = user.CMND;
            PhoneNumber = user.PhoneNumer;
            Address = user.Address;
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }
        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }
        private string cmnd;
        public string CMND
        {
            get { return cmnd; }
            set
            {
                cmnd = value;
                OnPropertyChanged("CMND");
            }
        }

        private string gmail;
        public string Gmail
        {
            get { return gmail; }
            set
            {
                gmail = value;
                OnPropertyChanged("CMND");
            }
        }
        private string address;
        private DispatcherTimer timer;

        public string Address
        {
            get { return address; }
            set { address = value; OnPropertyChanged("Address"); }
        }


        #endregion

        #region check internet
        private void timer_Tick(object sender, EventArgs e)
        {
            if (!IsConnectedToInternet())
            {
                timer.Stop();
                InternetCheckingView internetCheckingView = new InternetCheckingView(mainWindow, null);
                internetCheckingView.ShowDialog();
                timer.Start();
            }
        }

        public bool IsConnectedToInternet()
        {
            try
            {
                IPHostEntry i = Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region Xử lý biểu đồ

        private List<string> danhSachThang = new List<string>()
        {
            "Tất cả","1","2","3","4","5","6","7","8","9","10","11","12"
        };

        public List<string> DanhSachThang
        {
            get { return danhSachThang; }
            set { danhSachThang = value; }
        }

        private string thangTrongDoanhThuDaChon = "1";

        public string ThangTrongDoanhThuDaChon
        {
            get { return thangTrongDoanhThuDaChon; }
            set
            {
                thangTrongDoanhThuDaChon = value;
                if (value == "Tất cả")
                    IsVisibleBieuDoThang = "Hidden";
                else
                {
                    IsVisibleBieuDoThang = "Visible";
                    doanhThuThangDaChon = doanhThuThangServices.doanhThuThangs[Convert.ToInt32(value) - 1].doanhThuServices.doanhThus;
                    labelThangDaChon = doanhThuThangServices.doanhThuThangs[Convert.ToInt32(value) - 1].doanhThuServices.labels;
                }
                OnPropertyChanged("ThangTrongDoanhThuDaChon");
            }
        }

        private List<string> danhSachNam = new List<string>()
        {
           "2010","2011","2012","2013","2014","2015","2016","2017","2018","2019","2020","2021"
        };

        public List<string> DanhSachNam
        {
            get { return danhSachNam; }
            set { danhSachNam = value; }
        }

        private string danhSachNamDaChon = "2021";

        public string DanhSachNamDaChon
        {
            get { return danhSachNamDaChon; }
            set
            {
                danhSachNamDaChon = value;
                doanhThuThangServices = new DoanhThuThangServices(value);
                //doanhThuThangServices = new DoanhThuThangServices(value,chuyenBayServices);
                DoanhThuThangs = doanhThuThangServices.doanhThuThangs;
                labelThangDaChons = doanhThuThangServices.labels;

                if (ThangTrongDoanhThuDaChon != "Tất cả")
                {
                    doanhThuThangDaChon = doanhThuThangServices.doanhThuThangs[Convert.ToInt32(ThangTrongDoanhThuDaChon) - 1].doanhThuServices.doanhThus;
                    labelThangDaChon = doanhThuThangServices.doanhThuThangs[Convert.ToInt32(ThangTrongDoanhThuDaChon) - 1].doanhThuServices.labels;
                }
                OnPropertyChanged("DanhSachNamDaChon");
            }
        }

        private string isVisibleBieuDoThang = "Visible";

        public string IsVisibleBieuDoThang
        {
            get { return isVisibleBieuDoThang; }
            set
            {
                isVisibleBieuDoThang = value;
                if (value == "Hidden")
                    IsVisibleBieuDoNam = "Visible";
                else
                    IsVisibleBieuDoNam = "Hidden";
                OnPropertyChanged("IsVisibleBieuDoThang");
            }
        }
        private string isVisibleBieuDoNam = "Hidden";

        public string IsVisibleBieuDoNam
        {
            get { return isVisibleBieuDoNam; }
            set { isVisibleBieuDoNam = value; OnPropertyChanged("IsVisibleBieuDoNam"); }
        }
        #region In báo cáo doanh thu
        private ObservableCollection<DoanhThu> listDoanhThu;

        public ObservableCollection<DoanhThu> ListDoanhThu
        {
            get { return listDoanhThu; }
            set { listDoanhThu = value; OnPropertyChanged("ListDoanhThu"); }
        }
        private ObservableCollection<DoanhThu> listDoanhThuThang;

        public ObservableCollection<DoanhThu> ListDoanhThuThang
        {
            get { return listDoanhThuThang; }
            set { listDoanhThuThang = value; OnPropertyChanged("ListDoanhThuThang"); }
        }
        private string isVisibleBieuDo = "Visible";

        public string IsVisibleBieuDo
        {
            get { return isVisibleBieuDo; }
            set { isVisibleBieuDo = value; OnPropertyChanged("IsVisibleBieuDo"); }
        }
        private string isVisibleExportBaoCaoThang = "Hidden";

        public string IsVisibleExportBaoCaoThang
        {
            get { return isVisibleExportBaoCaoThang; }
            set { isVisibleExportBaoCaoThang = value; OnPropertyChanged("isVisibleExportBaoCaoThang"); }
        }
        private string isVisibleExportBaoCaoNam = "Hidden";

        public string IsVisibleExportBaoCaoNam
        {
            get { return isVisibleExportBaoCaoNam; }
            set { isVisibleExportBaoCaoNam = value; OnPropertyChanged("isVisibleExportBaoCaoNam"); }
        }
        private string isVisibleExportBaoCao = "Hidden";
        public string IsVisibleExportBaoCao
        {
            get { return isVisibleExportBaoCao; }
            set { isVisibleExportBaoCao = value; OnPropertyChanged("IsVisibleExportBaoCao"); }
        }
        private RelayCommand reviewBieuDoCommand;
        public RelayCommand ReviewBieuDoCommand
        {
            get { return reviewBieuDoCommand; }
        }
        public RelayCommand troVeBieuDoCommand;
        public RelayCommand TroVeBieuDoCommand
        {
            get { return troVeBieuDoCommand; }
        }
        public RelayCommand xuatPDFVisualCommand;
        public RelayCommand XuatPDFVisualCommand
        {
            get { return xuatPDFVisualCommand; }
        }
        public void reviewBieuDo()
        {
            if (ThangTrongDoanhThuDaChon == "Tất cả")
            {
                IsVisibleBieuDo = "Hidden";
                IsVisibleExportBaoCaoNam = "Visible";
                IsVisibleExportBaoCaoThang = "Hidden";
            }
            else
            {
                IsVisibleBieuDo = "Hidden";
                IsVisibleExportBaoCaoThang = "Visible";
                IsVisibleExportBaoCaoNam = "Hidden";
            }

            #region set thời gian
            NgayLapBaoCao.Ngay = DateTime.UtcNow.Day;
            NgayLapBaoCao.Thang = DateTime.UtcNow.Month;
            NgayLapBaoCao.Nam = DateTime.UtcNow.Year;
            switch (DateTime.UtcNow.DayOfWeek.ToString())
            {
                case "Monday": NgayLapBaoCao.Thu = "Thứ hai"; break;
                case "Tuesday": NgayLapBaoCao.Thu = "Thứ ba"; break;
                case "Wednesday": NgayLapBaoCao.Thu = "Thứ tư"; break;
                case "Thursday": NgayLapBaoCao.Thu = "Thứ năm"; break;
                case "Friday": NgayLapBaoCao.Thu = "Thứ sáu"; break;
                case "Saturday": NgayLapBaoCao.Thu = "Thứ bảy"; break;
                default: NgayLapBaoCao.Thu = "Chủ nhật"; break;
            }
            OnPropertyChanged("NgayLapBaoCao");
            #endregion
        }
        public void troVeBieuDo()
        {
            IsVisibleBieuDo = "Visible";
            IsVisibleExportBaoCaoThang = "Hidden";
            IsVisibleExportBaoCaoNam = "Hidden";
        }
        public void xuatPDFVisual(object view)
        {
            PrintDialog printDialog = new PrintDialog();
            PageMediaSize pageSize = new PageMediaSize(PageMediaSizeName.NorthAmericaLetter);
            printDialog.PrintTicket.PageMediaSize = pageSize;
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual((Grid)view, "test");
                CustomMessageBox.Show("Xuất file thành công", "Thông báo");
            }
        }
        private MyDatetime ngayLapBaoCao = new MyDatetime();
        public MyDatetime NgayLapBaoCao
        {
            get { return ngayLapBaoCao; }
            set { ngayLapBaoCao = value; OnPropertyChanged("NgayLapBaoCao"); }
        }
        #endregion
        #endregion

        #region Sell ticket
        public RelayCommand searchFlightCommand_SellTicket { get; private set; }
        private void searchFlight_SellTicket(object obj)
        {
            selectedFlight_SellTicket = null;
            OnPropertyChanged("SelectedFlight_SellTicket");
            ObservableCollection<ChuyenBay> temp = new ObservableCollection<ChuyenBay>(BackupList_SellTicket);
            flightSearchList_SellTicket = temp;

            if (selectedDestination_SellTicket != null)
                for (int i = 0; i < flightSearchList_SellTicket.Count; i++)
                {
                    if (flightSearchList_SellTicket[i].SanBayDen.Id == selectedDestination_SellTicket.Id) continue;
                    flightSearchList_SellTicket.RemoveAt(i);
                    i = -1;
                }
            if (selectedDeparture_SellTicket != null)
                for (int i = 0; i < flightSearchList_SellTicket.Count; i++)
                {
                    if (flightSearchList_SellTicket[i].SanBayDi.Id == selectedDeparture_SellTicket.Id) continue;
                    flightSearchList_SellTicket.RemoveAt(i);
                    i = -1;
                }

            for (int i = 0; i < flightSearchList_SellTicket.Count; i++)
            {
                if (flightSearchList_SellTicket[i].NgayBay == SelectedDate_SellTicket) continue;
                flightSearchList_SellTicket.RemoveAt(i);
                i = -1;
            }
            OnPropertyChanged("FlightSearchList_SellTicket");

        }

        public RelayCommand showAllFightsCommand_SellTicket { get; private set; }
        private void showAllFlights_SellTicket(object obj)
        {
            //selectedFlight = null;
            //OnPropertyChanged("SelectedFlight");
            selectedDate_SellTicket = DateTime.UtcNow.AddDays(1);
            SanBay temp1 = selectedDestination_SellTicket;
            SanBay temp2 = selectedDeparture_SellTicket;
            departureList_SellTicket.Remove(selectedDeparture_SellTicket);
            destinationList_SellTicket.Remove(selectedDestination_SellTicket);
            if (temp2 != null)
                departureList_SellTicket.Add(temp2);
            if (temp1 != null)
                destinationList_SellTicket.Add(temp1);
            selectedDeparture_SellTicket = null;
            selectedDestination_SellTicket = null;
            ObservableCollection<ChuyenBay> temp = new ObservableCollection<ChuyenBay>(BackupList_SellTicket);
            flightSearchList_SellTicket = temp;
            OnPropertyChanged("FlightSearchList_SellTicket");
        }

        private ICommand chooseContinueButton_SellTicketCommand;

        public ICommand ChooseContinueButton_SellTicketCommand
        {
            get { return chooseContinueButton_SellTicketCommand; }
        }
        private ButtonDuocChon isContinueButton_SellTicket = new ButtonDuocChon(false);

        public ButtonDuocChon IsContinueButton_SellTicket
        {
            get { return isContinueButton_SellTicket; }
            set { isContinueButton_SellTicket = value; OnPropertyChanged("IsContinueButton_SellTicket"); }
        }

        public void ButtonContinue_SellTicket(object p)
        {
            IsContinueButton_SellTicket = DuocChon;
            IsContinueButton = KhongDuocChon;
            IsDetailFlight_SellTicket = KhongDuocChon;
            IsDetailFlight = KhongDuocChon;
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = KhongDuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;

            ListTicketType_SellTicket.Clear();
            hashtable_AmountTicketType_SellTicket.Clear(); // Dictionary from Name to Amount
            hashtable_TicketId_SellTicket.Clear(); /// Dictionary from Name to ID
            foreach (ChiTietHangGhe ite in selectedFlight_SellTicket.ChiTietHangGhesList)
            {
                if (ite.SoLuongGheConLai == "0")
                {
                    hashtable_AmountTicketType_SellTicket.Add(ite.TenLoaiHangGhe, "Hết vé");
                    hashtable_TicketId_SellTicket.Add(ite.TenLoaiHangGhe, ite.MaLoaiHangGhe);
                }
                else
                {
                    hashtable_AmountTicketType_SellTicket.Add(ite.TenLoaiHangGhe, ite.SoLuongGheConLai);
                    hashtable_TicketId_SellTicket.Add(ite.TenLoaiHangGhe, ite.MaLoaiHangGhe);
                }
                ListTicketType_SellTicket.Add(ite.TenLoaiHangGhe);
            }
            OnPropertyChanged("ListTicketType_SellTicket");
            TicketTypeAmount_SellTicket = null;
            SellTicket.FlightId = selectedFlight_SellTicket.MaChuyenBay;

        }

        private RelayCommand chooseBackButtonCommand_SellTicket;

        public RelayCommand ChooseBackButtonCommand_SellTicket
        {
            get { return chooseBackButtonCommand_SellTicket; }
        }
        public void ButtonBack_SellTicket()
        {
            IsContinueButton_SellTicket = KhongDuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = DuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;
            sellTicket = new FlightTicket();
            OnPropertyChanged("SellTicket");
        }
        private bool checkPassangerInfor_SellTicket(object obj)
        {
            if (string.IsNullOrEmpty(SellTicket.Passenger) || string.IsNullOrEmpty(SellTicket.CMND) || string.IsNullOrEmpty(SellTicket.Address)
                || string.IsNullOrEmpty(SellTicket.PhoneNumber) || TicketTypeAmount_SellTicket == "Hết vé" || string.IsNullOrEmpty(TicketTypeAmount_SellTicket)) return false;
            return true;
        }








        private ObservableCollection<FlightTicket> listSellTicket;
        public ObservableCollection<FlightTicket> ListSellTicket
        {
            get { return listSellTicket; }
            set { listSellTicket = value; OnPropertyChanged("ListSellTicket"); }
        }
        private RelayCommand2<object> choosePayButtonCommand_SellTicket;

        public RelayCommand2<object> ChoosePayButtonCommand_SellTicket
        {
            get { return choosePayButtonCommand_SellTicket; }
        }
        private void ButtonPay_SellTicket(object obj)
        {
            DateTime bookingDate_check = DateTime.Now.AddDays(0);
            TimeSpan interval = selectedFlight_SellTicket.NgayBay.Subtract(bookingDate_check);
            double count = interval.Days * 24 + interval.Hours + ((interval.Minutes * 100) / 60) * 0.01;
            if (count < float.Parse(ThamSoQuyDinh.THOI_GIAN_CHAM_NHAT_DAT_VE))
            {
                CustomMessageBox.Show("Đã quá hạn bán vé cho chuyến bay này !", "Thông báo");
                return;
            }
            SellTicket.TicketId = selectedFlight_SellTicket.MaChuyenBay + "-" + FlightTicketServices.RandomString(6);
            SellTicket.FlownDate = selectedFlight_SellTicket.NgayBay;
            FlightTicketServices.Add(SellTicket, user.ID);
            hashtable_AmountTicketType_SellTicket.Remove(ticketType_SellTicket);
            if (TicketTypeAmount_SellTicket != "0")
                hashtable_AmountTicketType_SellTicket.Add(ticketType_SellTicket, (int.Parse(TicketTypeAmount_SellTicket) - 1).ToString());
            for (int i = 0; i < selectedFlight_SellTicket.ChiTietHangGhesList.Count; i++)
            {
                if (SelectedFlight_SellTicket.ChiTietHangGhesList[i].TenLoaiHangGhe == sticketType)
                    SelectedFlight_SellTicket.ChiTietHangGhesList[i].SoLuongGheConLai = (Int32.Parse(SelectedFlight_SellTicket.ChiTietHangGhesList[i].SoLuongGheConLai) - 1).ToString();
            }
            OnPropertyChanged("SelectedFlight_SellTicket");
            TicketTypeAmount_SellTicket = hashtable_AmountTicketType_SellTicket[ticketType_SellTicket].ToString();
            OnPropertyChanged("TicketTypeAmount_SellTicket");
            OnPropertyChanged("Hashtable_AmountTicketType_SellTicket");
            CustomMessageBox.Show("Bán vé thành công !", "Thông báo", MessageBoxButton.OK);
            string temp1;
            string temp2;
            DateTime temp3;
            ChuyenBayServices.GetFlight(SellTicket.FlightId, out temp1, out temp2, out temp3);
            SellTicket.Departure = temp1;
            SellTicket.Destination = temp2;
            SellTicket.FlownDate = temp3;
            ListSellTicket.Add(SellTicket);
            //// Set lai ve moi
            SellTicket = new FlightTicket();
            SellTicket.Departure = selectedFlight_SellTicket.SanBayDi.ToString();
            SellTicket.Destination = selectedFlight_SellTicket.SanBayDen.ToString();
            SellTicket.FlightId = selectedFlight_SellTicket.MaChuyenBay;
            SellTicket.FlownDate = selectedFlight_SellTicket.NgayBay;
            foreach (ChiTietHangGhe temp in selectedFlight_SellTicket.ChiTietHangGhesList)
            {
                SellTicket.TicketId = hashtable_TicketId_SellTicket[ticketType_SellTicket].ToString();
                if (temp.TenLoaiHangGhe == ticketType_SellTicket)
                {
                    SellTicket.Cost = (Int32.Parse(selectedFlight_SellTicket.GiaVeCoBan) * Int32.Parse(temp.TyLe)).ToString() + " VND";
                }
            }
            SellTicket.TicketId = hashtable_TicketId_SellTicket[ticketType_SellTicket].ToString();
            ////////

            OnPropertyChanged("ListSellTicket");









/*
            SellTicket.TicketId = selectedFlight_SellTicket.MaChuyenBay + "-" + FlightTicketServices.RandomString(6);
            FlightTicketServices.Add(SellTicket, user.ID);
            hashtable_AmountTicketType_SellTicket.Remove(TicketType_SellTicket);
            if (TicketTypeAmount_SellTicket != "0")
                hashtable_AmountTicketType_SellTicket.Add(TicketType_SellTicket, (int.Parse(TicketTypeAmount_SellTicket) - 1).ToString());
            TicketTypeAmount_SellTicket = hashtable_AmountTicketType_SellTicket[TicketType_SellTicket].ToString();
            OnPropertyChanged("TicketTypeAmount_SellTicket");
            OnPropertyChanged("Hashtable_AmountTicketType_SellTicket");
            MessageBox.Show("Ban vé roi nghen !", "Thông báo", MessageBoxButton.OK);
            string temp1;
            string temp2;
            DateTime temp3;
            ChuyenBayServices.GetFlight(SellTicket.FlightId, out temp1, out temp2, out temp3);
            SellTicket.Departure = temp1;
            SellTicket.Destination = temp2;
            ListSellTicket.Add(sellTicket);
            OnPropertyChanged("ListSellTicket");*/

        }
        private FlightTicket sellTicket;
        public FlightTicket SellTicket
        {
            get { return sellTicket; }
            set { sellTicket = value; OnPropertyChanged("SellTicket"); }

        }
        private string requestComboxBackground_SellTicket = "LightSlateGray";
        public string RequestComboxBackground_SellTicket
        {
            get { return requestComboxBackground_SellTicket; }
            set { requestComboxBackground_SellTicket = value; OnPropertyChanged("RequestComboxBackground_SellTicket"); }
        }
        private bool isSetRequest_SellTicket = false;
        public bool IsSetRequest_SellTicket
        {
            get { return isSetRequest_SellTicket; }
            set
            {
                isSetRequest_SellTicket = value;
                if (isSetRequest_SellTicket == true)
                    RequestComboxBackground_SellTicket = "White";
                else RequestComboxBackground_SellTicket = "LightSlateGray";
                OnPropertyChanged("IsSetRequest_SellTicket");
            }
        }
        private Hashtable hashtable_TicketId_SellTicket;
        public Hashtable Hashtable_TicketId_SellTicket
        {
            get { return hashtable_TicketId_SellTicket; }
            set { hashtable_TicketId_SellTicket = value; OnPropertyChanged("Hashtable_TicketId_SellTicket"); }
        }
        private string ticketType_SellTicket;
        public string TicketType_SellTicket
        {
            get { return ticketType_SellTicket; }
            set
            {
                ticketType_SellTicket = value;
                if (ticketType_SellTicket != null)
                {
                    TicketTypeAmount_SellTicket = hashtable_AmountTicketType_SellTicket[ticketType_SellTicket].ToString();
                    if (hashtable_AmountTicketType_SellTicket[ticketType_SellTicket].ToString() != "0")
                    {
                        SellTicket.TicketTypeId = hashtable_TicketId_SellTicket[ticketType_SellTicket].ToString();
                        if (ticketType_SellTicket == "Hạng nhất")
                            SellTicket.Cost = (Int32.Parse(selectedFlight_SellTicket.GiaVeCoBan) * 1.5).ToString() + " VND";
                        else if (ticketType_SellTicket == "Thương gia")
                            SellTicket.Cost = ((int)(Int32.Parse(selectedFlight_SellTicket.GiaVeCoBan) * 1.3)).ToString() + " VND";
                        else if (ticketType_SellTicket == "Phổ thông đặc biệt")
                            SellTicket.Cost = ((int)(Int32.Parse(selectedFlight_SellTicket.GiaVeCoBan) * 1.15)).ToString() + " VND";
                        else if (ticketType_SellTicket == "Phổ thông")
                            SellTicket.Cost = selectedFlight_SellTicket.GiaVeCoBan + " VND";
                    }
                }
                OnPropertyChanged("TicketType_SellTicket");
            }
        }

        private string ticketTypeAmount_SellTicket;
        public string TicketTypeAmount_SellTicket
        {
            get { return ticketTypeAmount_SellTicket; }
            set
            {
                ticketTypeAmount_SellTicket = value;
                OnPropertyChanged("TicketTypeAmount_SellTicket");
            }
        }
        private Hashtable hashtable_AmountTicketType_SellTicket;
        public Hashtable Hashtable_AmountTicketType_SellTicket
        {
            get { return hashtable_AmountTicketType_SellTicket; }
            set { hashtable_AmountTicketType_SellTicket = value; OnPropertyChanged("Hashtable_AmountTicketType_SellTicket"); }
        }
        private ObservableCollection<string> listTicketType_SellTicket;
        public ObservableCollection<string> ListTicketType_SellTicket
        {
            get { return listTicketType_SellTicket; }
            set { listTicketType_SellTicket = value; OnPropertyChanged("ListTicketType_SellTicket"); }
        }
        private DateTime selectedDate_SellTicket = DateTime.UtcNow.AddDays(0);
        public DateTime SelectedDate_SellTicket
        {
            get { return selectedDate_SellTicket; }
            set { selectedDate_SellTicket = value; OnPropertyChanged("SelectedDate_SellTicket"); }
        }

        private ObservableCollection<SanBay> departureList_SellTicket;
        public ObservableCollection<SanBay> DepartureList_SellTicket
        {
            get { return departureList_SellTicket; }
            set { departureList_SellTicket = value; OnPropertyChanged("DepartureList_SellTicket"); }
        }

        private SanBay selectedDeparture_SellTicket;
        public SanBay SelectedDeparture_SellTicket
        {
            get { return selectedDeparture_SellTicket; }
            set
            {
                if (selectedDeparture_SellTicket != null)
                    DestinationList_SellTicket.Add(selectedDeparture_SellTicket);
                selectedDeparture_SellTicket = value;
                DestinationList_SellTicket.Remove(selectedDeparture_SellTicket);
                OnPropertyChanged("SelectedDeparture_SellTicket");
            }
        }

        private ObservableCollection<SanBay> destinationList_SellTicket;
        public ObservableCollection<SanBay> DestinationList_SellTicket
        {
            get { return destinationList_SellTicket; }
            set { destinationList_SellTicket = value; OnPropertyChanged("DestinationList_SellTicket"); }
        }

        private SanBay selectedDestination_SellTicket;
        public SanBay SelectedDestination_SellTicket
        {
            get { return selectedDestination_SellTicket; }
            set
            {
                if (selectedDestination_SellTicket != null)
                    DepartureList_SellTicket.Add(selectedDestination_SellTicket);
                selectedDestination_SellTicket = value;
                DepartureList_SellTicket.Remove(selectedDestination_SellTicket);
                OnPropertyChanged("SelectedDestination_SellTicket");
            }
        }

        private ObservableCollection<ChuyenBay> flightSearchList_SellTicket;
        public ObservableCollection<ChuyenBay> FlightSearchList_SellTicket
        {
            get { return flightSearchList_SellTicket; }
            set { flightSearchList_SellTicket = value; OnPropertyChanged("FlightSearchList_SellTicket"); }
        }


        private ObservableCollection<ChuyenBay> backupList_SellTicket;
        public ObservableCollection<ChuyenBay> BackupList_SellTicket
        {
            get { return backupList_SellTicket; }
            set { backupList_SellTicket = value; OnPropertyChanged("BackupList_SellTicket"); }
        }
        private ChuyenBay selectedFlight_SellTicket;
        public ChuyenBay SelectedFlight_SellTicket
        {
            get { return selectedFlight_SellTicket; }
            set
            {
                selectedFlight_SellTicket = value;
                DateofSelectedFlight_SellTicket = value.NgayBay.GetDateTimeFormats();
                OnPropertyChanged("SelectedFlight_SellTicket");
            }
        }
        private string[] dateofSelectedFlight_SellTicket;
        public string[] DateofSelectedFlight_SellTicket
        {
            get { return dateofSelectedFlight_SellTicket; }
            set
            {
                dateofSelectedFlight_SellTicket = value;
                OnPropertyChanged("DateofSelectedFlight_SellTicket");

            }
        }
        #endregion

        #region chi tiết chuyến bay bán vé

        private ButtonDuocChon isDetailFlight_SellTicket = new ButtonDuocChon(false);

        public ButtonDuocChon IsDetailFlight_SellTicket
        {
            get { return isDetailFlight_SellTicket; }
            set { isDetailFlight_SellTicket = value; OnPropertyChanged("IsDetailFlight_SellTicket"); }
        }
        public void ButtonDetailFlight_SellTicket()
        {
            IsDetailFlight = KhongDuocChon;
            IsDetailFlight_SellTicket = DuocChon;
            IsContinueButton = KhongDuocChon;
            IsDuocChon1 = KhongDuocChon;
            IsDuocChon2 = DuocChon;
            IsDuocChon3 = KhongDuocChon;
            IsDuocChon4 = KhongDuocChon;
            IsDuocChon5 = KhongDuocChon;
            IsDuocChon6 = KhongDuocChon;
            IsDuocChon7 = KhongDuocChon;
        }
        private ButtonDuocChon isBackButton_DetailFlight_SellTicket = new ButtonDuocChon(false);

        public ButtonDuocChon IsBackButton_DetailFlight_SellTicket
        {
            get { return isBackButton_DetailFlight_SellTicket; }
            set { isBackButton_DetailFlight_SellTicket = value; OnPropertyChanged("IsBackButton_DetailFlight_SellTicket"); }
        }
        public RelayCommand chooseBack_DetailFlightCommand_SellTicket { get; private set; }
        public void ButtonBack_DetailFlight_SellTicket()
        {
            if (IsDuocChon3.NewVisibility == "Visible")
            {
                IsContinueButton = KhongDuocChon;
                IsDuocChon1 = KhongDuocChon;
                IsDetailFlight = KhongDuocChon;
                IsDetailFlight_SellTicket = KhongDuocChon;
                IsDuocChon2 = KhongDuocChon;
                IsDuocChon3 = DuocChon;
                IsDuocChon4 = KhongDuocChon;
                IsDuocChon5 = KhongDuocChon;
                IsDuocChon6 = KhongDuocChon;
                IsDuocChon7 = KhongDuocChon;
            }
            else
            {
                IsContinueButton = KhongDuocChon;
                IsDuocChon1 = KhongDuocChon;
                IsDetailFlight = KhongDuocChon;
                IsDetailFlight_SellTicket = KhongDuocChon;
                IsDuocChon2 = DuocChon;
                IsDuocChon3 = KhongDuocChon;
                IsDuocChon4 = KhongDuocChon;
                IsDuocChon5 = KhongDuocChon;
                IsDuocChon6 = KhongDuocChon;
                IsDuocChon7 = KhongDuocChon;
            }
        }


        public BindableCollection<SanBayTrungGian> intermediaryAirport_SellTicket;
        public BindableCollection<SanBayTrungGian> IntermediaryAirport_SellTicket
        {
            get { return intermediaryAirport_SellTicket; }
            set { intermediaryAirport_SellTicket = value; OnPropertyChanged("IntermediaryAirport_SellTicket"); }
        }
        public BindableCollection<ChiTietHangGhe> detailTypeSticket_DetailFlight_SellTicket;
        public BindableCollection<ChiTietHangGhe> DetailTypeSticket_DetailFlight_SellTicket
        {
            get { return detailTypeSticket_DetailFlight_SellTicket; }
            set { detailTypeSticket_DetailFlight_SellTicket = value; OnPropertyChanged("DetailTypeSticket_DetailFlight_SellTicket"); }
        }
        private RelayCommand2<object> chooseDetailFlight_SellTicket;

        public RelayCommand2<object> ChooseDetailFlight_SellTicket
        {
            get { return chooseDetailFlight_SellTicket; }
        }
        private bool CheckSelected_DetailFlight_SellTicket(object obj)
        {
            return true;
            ChuyenBay selected = obj as ChuyenBay;
            if (selected != null) return true;
            return false;

        }

        private void ButtonDetailFlight_SellTicket(object obj)
        {
            if (IsDuocChon3.NewVisibility == "Visible")
            {
                IsDetailFlight = DuocChon;
                IsContinueButton = KhongDuocChon;
                IsDuocChon1 = KhongDuocChon;
                IsDuocChon2 = KhongDuocChon;
                IsDuocChon3 = DuocChon;
                IsDuocChon4 = KhongDuocChon;
                IsDuocChon5 = KhongDuocChon;
                IsDuocChon6 = KhongDuocChon;
                IsDuocChon7 = KhongDuocChon;
                ChuyenBay selected = obj as ChuyenBay;
                SelectedFlight_SellTicket = selected;
                intermediaryAirport_SellTicket = new BindableCollection<SanBayTrungGian>(selectedFlight_SellTicket.SanBayTrungGian);
                DetailTypeSticket_DetailFlight_SellTicket = new BindableCollection<ChiTietHangGhe>(selectedFlight_SellTicket.ChiTietHangGhesList);
                OnPropertyChanged("IntermediaryAirport_SellTicket");
                OnPropertyChanged("DetailTypeSticket_DetailFlight_SellTicket");
            }
            else
            {
                ButtonDetailFlight_SellTicket();
                ChuyenBay selected = obj as ChuyenBay;
                SelectedFlight_SellTicket = selected;
                intermediaryAirport_SellTicket = new BindableCollection<SanBayTrungGian>(selectedFlight_SellTicket.SanBayTrungGian);
                DetailTypeSticket_DetailFlight_SellTicket = new BindableCollection<ChiTietHangGhe>(selectedFlight_SellTicket.ChiTietHangGhesList);
                OnPropertyChanged("IntermediaryAirport_SellTicket");
                OnPropertyChanged("DetailTypeSticket_DetailFlight_SellTicket");
            }
        }
        #endregion

        #region Loai hang ve

        public ICommand InsertTickTypeCommand_Setting { get; set; }
        void insertTicket_Setting()
        {
            InsertTicketDialog insertTicketDialog = new InsertTicketDialog();
            insertTicketDialog.ShowDialog();
            ListTicketType_Setting.Add(insertTicketDialog.viewModel.GetResult());

        }
        public ICommand DeleteTickTypeCommand_Setting { get; set; }
        void deleteTicket_Setting()
        {
            if (TicketType_Setting != null)
                ListTicketType_Setting.Remove(TicketType_Setting);
        }
        public ICommand ResetTickTypeCommand_Setting { get; set; }
        void resetTicket_Setting()
        {
            MessageBoxResult rs = CustomMessageBox.Show("Xác nhận đặt lại?", "Cảnh báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (rs == MessageBoxResult.OK)
            {
                ListTicketType_Setting.Clear();
                backupListTicketType_Setting.Clear();
                LoadSQL();
                ListTicketType_Setting = new ObservableCollection<LoaiHangGhe>(backupListTicketType_Setting);
                #region hủy lưu quy định chuyến bay
                SanBaysQuyDinh = new ObservableCollection<SanBay>(sanBayServices.GetSanBayHoatDong());
                sanBaysDaXoa = new List<SanBay>();
                sanBayMoiThem = new List<SanBay>();
                ThoiGianBayToiThieu = ThamSoQuyDinh.THOI_GIAN_BAY_TOI_THIEU;
                SoSanBayTrungGianToiDa = ThamSoQuyDinh.SO_SAN_BAY_TRUNG_GIAN_TOI_DA;
                ThoiGianDungToiThieu = ThamSoQuyDinh.THOI_GIAN_DUNG_TOI_THIEU;
                ThoiGianDungToiDa = ThamSoQuyDinh.THOI_GIAN_DUNG_TOI_DA;
                #endregion
                #region huy lưu quy định đặt vé
                ThoiGianHuyVeTreNhat = ThamSoQuyDinh.THOI_GIAN_CHAM_NHAT_HUY_VE;
                ThoiGianDatVeTreNhat = ThamSoQuyDinh.THOI_GIAN_CHAM_NHAT_DAT_VE;
                #endregion
            }

        }
        public ICommand SaveCommand_Setting { get; set; }
        void save_Setting()
        {
            MessageBoxResult rs = CustomMessageBox.Show("Xác nhận lưu và khởi động lại ứng dụng?", "Cảnh báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (rs == MessageBoxResult.OK)
            {
                #region Lưu quy định chuyến bay và đặt vé
                foreach (SanBay sanBay in sanBaysDaXoa)
                {
                    sanBayServices.DungHoatDongSanBay(sanBay.Id);
                }
                foreach (SanBay sanBay in sanBayMoiThem)
                {
                    sanBayServices.Add(sanBay);
                }

                #endregion

                List<LoaiHangGhe> temp = new List<LoaiHangGhe>(ListTicketType_Setting);
                ThamSoQuyDinh.updateTicketTypeToSql(temp);
                ThamSoQuyDinh.LoadThamSoQuyDinhXuongSQL(ThoiGianBayToiThieu, SoSanBayTrungGianToiDa, ThoiGianDungToiDa, ThoiGianDungToiThieu, temp.Count.ToString(), ThoiGianHuyVeTreNhat, ThoiGianDatVeTreNhat);
                #region restart app

                logOut();
                #endregion
            }
            else 
            { }
        }



        private LoaiHangGhe ticketType_Setting;
        public LoaiHangGhe TicketType_Setting
        {
            get { return ticketType_Setting; }
            set
            {               
                ticketType_Setting = value;
                OnPropertyChanged("TicketType_Setting");

            }
        }
        private ObservableCollection<LoaiHangGhe> listTicketType_Setting;
        public ObservableCollection<LoaiHangGhe> ListTicketType_Setting
        {
            get { return listTicketType_Setting; }
            set { listTicketType_Setting = value; OnPropertyChanged("ListTicketType_Setting"); }
        }
        private List<LoaiHangGhe> backupListTicketType_Setting;

        private SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        public void LoadSQL()
        {
            try
            {
                Connection.Open();
                #region Truy vấn dữ liệu từ sql
                SqlCommand command = new SqlCommand("SELECT * FROM LOAI_HANG_GHE WHERE KHA_DUNG = '1'", Connection);
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                #endregion
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        LoaiHangGhe hangGhe = new LoaiHangGhe();
                        hangGhe.MaLoaiHangGhe = row["MA_LOAI_HANG_GHE"].ToString();
                        hangGhe.TenLoaiHangGhe = row["TEN_LOAI_HANG_GHE"].ToString();
                        hangGhe.TyLe = row["TY_LE"].ToString();
                        backupListTicketType_Setting.Add(hangGhe);
                    }
                }

                var newList = backupListTicketType_Setting.OrderByDescending(e => Convert.ToInt32(e.TyLe));
                backupListTicketType_Setting = new List<LoaiHangGhe>(newList);
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
        }
        



        #endregion
        #region Quy định chuyến bay
        private string thoiGianBayToiThieu = "";

        public string ThoiGianBayToiThieu
        {
            get { return thoiGianBayToiThieu; }
            set 
            {
                if (value != null)
                {
                    if(value != "")
                    {
                        if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                        {
                            #region Kiểm tra quy định
                            thoiGianBayToiThieu = value;
                            #endregion
                        }
                        else
                        {
                            CustomMessageBox.Show("Cần nhập số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                    else
                    {

                    }    
                }
                OnPropertyChanged("ThoiGianBayToiThieu"); }
        }
        private string soSanBayTrungGianToiDa= "";

        public string SoSanBayTrungGianToiDa
        {
            get { return soSanBayTrungGianToiDa; }
            set {
                if (value != null)
                {
                    if (value != "")
                    {
                        if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                        {
                            #region Kiểm tra quy định
                            soSanBayTrungGianToiDa = value;
                            #endregion
                        }
                        else
                        {
                            CustomMessageBox.Show("Cần nhập số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                    else
                    {

                    }
                }
                OnPropertyChanged("SoSanBayTrungGianToiDa"); }
        }
        private string thoiGianDungToiThieu = "";

        public string ThoiGianDungToiThieu
        {
            get { return thoiGianDungToiThieu; }
            set {
                if (value != "")
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        if (ThoiGianDungToiDa != "")
                        {
                            if (Convert.ToInt32(value) >= Convert.ToInt32(ThoiGianDungToiDa))
                            {
                                CustomMessageBox.Show("Thời gian dừng tối thiếu cần nhỏ hơn thời gian dừng tối đa", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                            else
                            {
                                thoiGianDungToiThieu = value;
                            }
                        }
                        else
                        {
                            thoiGianDungToiThieu = value;
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show("Cần nhập số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {

                }
                OnPropertyChanged("ThoiGianDungToiThieu"); }
        }
        private string thoiGianDungToiDa = "";

        public string ThoiGianDungToiDa
        {
            get { return thoiGianDungToiDa; }
            set {
                if (value != "")
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        if (ThoiGianDungToiThieu != "")
                        {
                            if (Convert.ToInt32(value) <= Convert.ToInt32(ThoiGianDungToiThieu))
                            {
                                CustomMessageBox.Show("Thời gian dừng tối thiếu cần nhỏ hơn thời gian dừng tối đa", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                            else
                            {
                                thoiGianDungToiDa = value;
                            }
                        }
                        else
                        {
                            thoiGianDungToiDa = value;
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show("Cần nhập số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {

                }
                OnPropertyChanged("ThoiGianDungToiDa"); }
        }
        private ObservableCollection<SanBay> sanBaysQuyDinh;
        public ObservableCollection<SanBay> SanBaysQuyDinh
        {
            get { return sanBaysQuyDinh; }
            set { sanBaysQuyDinh = value;OnPropertyChanged("SanBaysQuyDinh"); }
        }
        private SanBay sanBayQuyDinhDaChon;
        public SanBay SanBayQuyDinhDaChon
        {
            get { return sanBayQuyDinhDaChon; }
            set { sanBayQuyDinhDaChon = value; OnPropertyChanged("SanBayQuyDinhDaChon"); }
        }
        public List<SanBay> sanBaysDaXoa = new List<SanBay>();
        private RelayCommand xoaSanBayQuyDinhCommand;

        public RelayCommand XoaSanBayQuyDinhCommand
        {
            get { return xoaSanBayQuyDinhCommand; }
        }
        private RelayCommand themSanBayQuyDinhCommand;

        public RelayCommand ThemSanBayQuyDinhCommand
        {
            get { return themSanBayQuyDinhCommand; }
        }
        private RelayCommand xacNhanThemSanBayCommand;

        public RelayCommand XacNhanThemSanBayCommand
        {
            get { return xacNhanThemSanBayCommand; }
        }
        private RelayCommand huyThemSanBayCommand;

        public RelayCommand HuyThemSanBayCommand
        {
            get { return huyThemSanBayCommand; }
        }
        public void xoaSanBayQuyDinh()
        {
            if (SanBayQuyDinhDaChon == null)
            {
                CustomMessageBox.Show("Vui lòng chọn sân bay cần xóa.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }   
            else
            {
                sanBaysDaXoa.Add(SanBayQuyDinhDaChon);
                SanBaysQuyDinh.Remove(SanBayQuyDinhDaChon);
            }    
        }
        ThemSanBay themSanBay;
        List<SanBay> sanBayMoiThem = new List<SanBay>();
        public void themSanBayQuyDinh()
        {
            themSanBay = new ThemSanBay();
            themSanBay.DataContext = this;
            themSanBay.Show();

        }
        private string canhBaoNhapDayDuVisible = "Hidden";

        public string CanhBaoNhapDayDuVisible
        {
            get { return canhBaoNhapDayDuVisible; }
            set { canhBaoNhapDayDuVisible = value; OnPropertyChanged("CanhBaoNhapDayDuVisible"); }
        }
        private string thongBaoNhapThanhCongVisible = "Hidden";

        public string ThongBaoNhapThanhCongVisible
        {
            get { return thongBaoNhapThanhCongVisible; }
            set { thongBaoNhapThanhCongVisible = value; OnPropertyChanged("ThongBaoNhapThanhCongVisible"); }
        }
        private string maSanBay = "";

        public string MaSanBay
        {
            get { return maSanBay; }
            set 
            {
                if (value != null)
                {
                    if (KiemTraHopLeInput.KiemTraMa(value))
                    {
                        maSanBay = value.ToUpper();
                        CanhBaoSaiDinhDangVisible = "Hidden";
                        CanhBaoMaDaTonTaiVisible = "Hidden";
                    }
                    else
                    {
                        CanhBaoSaiDinhDangVisible = "Visible";
                        CanhBaoMaDaTonTaiVisible = "Hidden";
                    }
                }
                else
                {
                    maSanBay = value;
                    CanhBaoSaiDinhDangVisible = "Hidden";
                    CanhBaoMaDaTonTaiVisible = "Hidden";
                }
                 OnPropertyChanged("MaSanBay");
            }
        }
        private string tenSanBay = "";

        public string TenSanBay
        {
            get { return tenSanBay; }
            set
            {
                tenSanBay = value; OnPropertyChanged("TenSanBay");   
            }
        }
        private string canhBaoSaiDinhDangVisible = "Hidden";

        public string CanhBaoSaiDinhDangVisible
        {
            get { return canhBaoSaiDinhDangVisible; }
            set { canhBaoSaiDinhDangVisible = value; OnPropertyChanged("CanhBaoSaiDinhDangVisible"); }
        }
        private string canhBaoMaDaTonTaiVisible = "Hidden";

        public string CanhBaoMaDaTonTaiVisible
        {
            get { return canhBaoMaDaTonTaiVisible; }
            set { canhBaoMaDaTonTaiVisible = value; OnPropertyChanged("CanhBaoMaDaTonTaiVisible"); }
        }

        public void huyThemSanBay()
        {
            MaSanBay = "";
            TenSanBay = "";
            CanhBaoNhapDayDuVisible = "Hidden";
            ThongBaoNhapThanhCongVisible = "Hidden";
            themSanBay.Close();
        }
        public void xacNhanThemSanBay()
        {
            if ((TenSanBay == "" || MaSanBay == ""))
            {
                CanhBaoNhapDayDuVisible = "Visible";

            }
            else
            {
                foreach (SanBay sanBay in SanbaysList)
                {
                    if (MaSanBay == sanBay.Id)
                    {
                        CanhBaoMaDaTonTaiVisible = "Visible";
                        CanhBaoSaiDinhDangVisible = "Hidden";
                        return;
                    }
                }
                CanhBaoMaDaTonTaiVisible = "Hidden";
                CanhBaoSaiDinhDangVisible = "Hidden";
                CanhBaoNhapDayDuVisible = "Hidden";
                ThongBaoNhapThanhCongVisible = "Visible";

                SanBay newSanBay = new SanBay();
                newSanBay.Id = MaSanBay;
                newSanBay.TenSanBay = TenSanBay;
                sanBayMoiThem.Add(newSanBay);
                SanBaysQuyDinh.Add(newSanBay);

                xacNhanTimer = new DispatcherTimer();
                xacNhanTimer.Interval = TimeSpan.FromSeconds(1);
                xacNhanTimer.Tick += xacNhanTimerTick;
                xacNhanTimer.Start();
            }    
        }
        DispatcherTimer xacNhanTimer;
        private void xacNhanTimerTick(object sender, EventArgs e)
        {
            xacNhanTimer.Stop();
            themSanBay.Close();
            MaSanBay = "";
            TenSanBay = "";
            CanhBaoNhapDayDuVisible = "Hidden";
            ThongBaoNhapThanhCongVisible = "Hidden";
        }
        #endregion
        #region Quy định đặt vé
        private string thoiGianHuyVeTreNhat = "";

        public string ThoiGianHuyVeTreNhat
        {
            get { return thoiGianHuyVeTreNhat; }
            set 
            {
                if (value != "")
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        if (ThoiGianDatVeTreNhat != "")
                        {
                            if (Convert.ToInt32(value) <= Convert.ToInt32(ThoiGianDatVeTreNhat))
                            {
                                CustomMessageBox.Show("Thời gian hủy cần trước thời gian đặt", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                            else
                            {
                                thoiGianHuyVeTreNhat = value;
                            }
                        }
                        else
                        {
                            thoiGianHuyVeTreNhat = value;
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show("Cần nhập số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {

                }
                OnPropertyChanged("ThoiGianHuyVeTreNhat"); }
        }
        private string thoiGianDatVeTreNhat = "";

        public string ThoiGianDatVeTreNhat
        {
            get { return thoiGianDatVeTreNhat; }
            set 
            {
                if (value != "")
                {
                    if (KiemTraHopLeInput.KiemTraChuoiSoNguyen(value))
                    {
                        if (ThoiGianHuyVeTreNhat != "")
                        {
                            if (Convert.ToInt32(value) >= Convert.ToInt32(ThoiGianHuyVeTreNhat))
                            {
                                CustomMessageBox.Show("Thời gian hủy cần trước thời gian đặt", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                            else
                            {
                                thoiGianDatVeTreNhat = value;
                            }
                        }
                        else
                        {
                            thoiGianDatVeTreNhat = value;
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show("Cần nhập số nguyên dương", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {

                }
                OnPropertyChanged("ThoiGianDatVeTreNhat"); }
        }

        #endregion
    }
}

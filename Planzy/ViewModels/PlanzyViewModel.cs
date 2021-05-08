using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planzy.Models.SanBayModel;
using Planzy.Models.SupportUI;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Planzy.Commands;
namespace Planzy.ViewModels
{
    class PlanzyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        SanBayServices sanBayServices;
        
        public PlanzyViewModel()
        {
            sanBayServices = new SanBayServices();
            LoadData();
            doiViTriSanBayCommand = new RelayCommand(DoiViTriSanBay);
            #region Xử lý giao diện ban đầu
            chonLayoutCommand1 = new RelayCommand(Button1);
            chonLayoutCommand2 = new RelayCommand(Button2);
            chonLayoutCommand3 = new RelayCommand(Button3);
            chonLayoutCommand4 = new RelayCommand(Button4);
            chonLayoutCommand5 = new RelayCommand(Button5);
            #endregion
        }
        #region Xử lý chọn sân bay
        private ObservableCollection<SanBay> sanbaysList;

        public ObservableCollection<SanBay> SanbaysList
        {
            get { return sanbaysList; }
            set { sanbaysList = value; OnPropertyChanged("SanbaysList"); }
        }

        private void LoadData()
        {
            SanbaysList = new ObservableCollection<SanBay>(sanBayServices.GetAll());
        }

        private SanBay sanBayDiDaChon;
        public SanBay SanBayDiDaChon
        {
            get { return sanBayDiDaChon; }
            set 
            {
                sanBayDiDaChon = value;
                OnPropertyChanged("SanBayDiDaChon");
                if (SanBayDenDaChon != null)
                {
                    if (sanBayDiDaChon.Id == sanBayDenDaChon.Id)
                    {
                        MyProperty = 22;
                    }
                }
            }
        }
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; OnPropertyChanged("MyProperty"); }
        }


        private SanBay sanBayDenDaChon;
        public SanBay SanBayDenDaChon
        {
            get { return sanBayDenDaChon; }
            set
            {
                sanBayDenDaChon = value;
                OnPropertyChanged("SanBayDenDaChon");
                if (SanBayDiDaChon != null)
                {
                    if (sanBayDiDaChon.Id == sanBayDenDaChon.Id)
                    {
                        MyProperty = 11;
                    }
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
                temp = SanBayDenDaChon;
                SanBayDenDaChon = SanBayDiDaChon;
                SanBayDiDaChon = temp;
            }
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
    }
}

using Planzy.Commands;
using Planzy.Models.LoaiHangGheModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Planzy.ViewModels
{
    public class InsertTicketDialogViewModel : INotifyPropertyChanged
    {
        #region onpropertychange
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public ICommand InsertCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        Window window;

        LoaiHangGhe TicketType;
        LoaiHangGheServices loaiHangGheServices;
        DispatcherTimer timer;
        Random r = new Random();

        public InsertTicketDialogViewModel()
        {
            InsertCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { insertCommand(p); });
            CancelCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { p.Close(); });

            loaiHangGheServices = new LoaiHangGheServices();

            Id = loaiHangGheServices.GetId(2);

            ErrorMessageVisibility = "Collapsed";
            SucessMessageVisibility = "Collapsed";

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += timer_Tick;

        }

        private void timer_Tick(object sender, EventArgs e)
        {

            timer.Stop();
            if(window!=null)
            window.Close();
        }

        private void insertCommand(Window p)
        {
            if(checkResult())
            {
                TicketType = new LoaiHangGhe();
                TicketType.MaLoaiHangGhe = Id;
                TicketType.TenLoaiHangGhe = Name;
                TicketType.TyLe = Ratio;
                window = p;

                ErrorMessageVisibility = "Collapsed";
                SucessMessageVisibility = "Visible";

                timer.Start();
            }
            else
            {               
                TicketType = null;
            }
        }
        private bool checkResult()
        {
            if (Id == null || Id == "" || Name == null || Name == "" || Ratio == null || Ratio == "")
            {
                ErrorMessage = "Vui lòng nhập đủ thông tin";
                ErrorMessageVisibility = "Visible";
                return false;
            }
            if (!IsNumber(Ratio))
            {
                ErrorMessage = "Tỷ lệ thuộc số nguyên dương";
                ErrorMessageVisibility = "Visible";
                return false;
            }


            return true;

        }

        bool IsNumber(string number)
        {
            for (int i = 0; i < number.Length; i++)
            {
                if (number[i] < 48 || number[i] > 57) return false;
            }
            return true;
        }




        public LoaiHangGhe GetResult()
        {
            return TicketType;
        }
        private string errorMessageVisibility;
        public string ErrorMessageVisibility
        {
            get { return errorMessageVisibility; }
            set
            {
                errorMessageVisibility = value;
                OnPropertyChanged("ErrorMessageVisibility");
            }
        }
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }
        private string sucessMessageVisibility;
        public string SucessMessageVisibility
        {
            get { return sucessMessageVisibility; }
            set
            {
                sucessMessageVisibility = value;
                OnPropertyChanged("SucessMessageVisibility");
            }
        }
        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        private string ratio;
        public string Ratio
        {
            get { return ratio; }
            set
            {
                ratio = value;
                OnPropertyChanged("Ratio");
            }
        }
    }
}

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

         LoaiHangGhe TicketType;

        public InsertTicketDialogViewModel()
        {
            InsertCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { insertCommand(p); });
            CancelCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { p.Close(); });


            ErrorMessageVisibility = "Collapsed";
            SucessMessageVisibility = "Collapsed";
        }

        private void insertCommand(Window p)
        {
            if(Id != null || Id != "" || Name != null || Name != ""|| Ratio != null || Ratio != "")
            {
                TicketType = new LoaiHangGhe();
                TicketType.MaLoaiHangGhe = Id;
                TicketType.TenLoaiHangGhe = Name;
                TicketType.TyLe = Ratio;
                p.Close();
            }
            else
            {
                ErrorMessageVisibility = "Visible";
                TicketType = null;
            }
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

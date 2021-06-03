using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace Planzy.Models.SupportUI
{
    class ButtonDuocChon : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string newForeground;

        public string NewForeground
        {
            get { return newForeground; }
            set { newForeground = value; OnPropertyChanged("NewForeground"); }
        }
        private string newBackground;

        public string NewBackground
        {
            get { return newBackground; }
            set { newBackground = value; OnPropertyChanged("NewBackground"); }
        }

        private string newVisibility;

        public string NewVisibility
        {
            get { return newVisibility; }
            set { newVisibility = value; OnPropertyChanged("NewVisibility"); }
        }
        public ButtonDuocChon(bool isDuocChon)
        {
            if (isDuocChon)
            {
                newBackground = "#FF5867AC";
                newForeground = "#ffffffff";
                newVisibility = "Visible";
            }    
            else
            {
                newBackground = "#ffffffff";
                newForeground = "#FF5867AC";
                newVisibility = "Hidden";
            }    
        }
    }
}

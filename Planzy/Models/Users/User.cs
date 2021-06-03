using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.Users
{
    class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public User()
        {

        }

        public User(string id, string gmail, string password, string name, string phoneNumber, string cmnd)
        {
            this.id = id;
            this.gmail = gmail;
            this.password = password;
            this.name = name;
            this.phoneNumber = phoneNumber;
            this.cmnd = cmnd;
        }

        private string id;
        public string ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("ID");
            }
        }
        private string gmail;
        public string Gmail
        {
            get { return gmail; }
            set
            {
                gmail = value;
                OnPropertyChanged("Gmail");
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
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
        private string phoneNumber;
        public string PhoneNumer
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


    }
}

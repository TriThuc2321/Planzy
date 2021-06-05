using Planzy.Commands;
using Planzy.Models.Users;
using Planzy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Planzy.LoginRegister
{
    class RegisterViewModel: INotifyPropertyChanged
    {
        #region onpropertychange
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        Window loginWindow;
        UserServices userServices;
        User user;
        List<User> listUsers;

        public ICommand LoginXamlCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public RegisterViewModel()
        {
            userServices = new UserServices();
            listUsers = new List<User>(userServices.GetAll());


            LoginXamlCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { OpenLoginWindow(p); });
            ExitCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { p.Close(); });
        }
        void OpenLoginWindow(Window p)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            p.Close();
        }
    }
    
}

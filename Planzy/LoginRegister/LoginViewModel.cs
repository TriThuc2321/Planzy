using Planzy.LoginRegister;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using Condition = System.Windows.Automation.Condition;
using System.Windows.Threading;
using System.Windows.Interop;
using System.IO;
using System.Text.RegularExpressions;
using Planzy.Commands;
using Planzy.Models.Users;
using Microsoft.Xaml.Behaviors;
using Newtonsoft.Json;

namespace Planzy.LoginRegister
{
    class LoginViewModel: INotifyPropertyChanged
    {
        #region onpropertychange
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        ProfileResponse profileResponse;
        Window loginWindow;
        UserServices userServices;
        User user;
        List<User> listUsers;


        public ICommand LoginGoogleCommand { get; set; }
        public ICommand PasswordChangCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public LoginViewModel()
        {
            userServices = new UserServices();
            listUsers = new List<User>(userServices.GetAll());

            LoginGoogleCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { LoginGoogleClick(p); });
            LoginCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { LoginClick(p); });
            PasswordChangCommand = new RelayCommand2<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            ExitCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { Exit(p); });

            NonExistAccountVisibility = "Hidden";
            IncorrectPasswordVisibility = "Hidden";
            LoginSuccessVisibility = " Hidden";
        }
        void LoginClick(Window p)
        {
            int i = 0;
            for(i =0; i< listUsers.Count(); i++)
            {
                if(listUsers[i].ID == Account)
                {
                    if (listUsers[i].Password == Password)
                    {
                        
                        NonExistAccountVisibility = "Hidden";
                        IncorrectPasswordVisibility = "Hidden";
                        LoginSuccessVisibility = "Visible";
                        MainWindow mainForm = new MainWindow(listUsers[i].ID);
                        mainForm.Show();
                        p.Close();
                    }
                    else
                    {
                        NonExistAccountVisibility = "Hidden";
                        IncorrectPasswordVisibility = "Visible";
                        LoginSuccessVisibility = " Hidden";
                        break;
                    }
                   
                }
            }
            if (i == listUsers.Count())
            {
                NonExistAccountVisibility = "Visible";
                IncorrectPasswordVisibility = "Hidden";
                LoginSuccessVisibility = " Hidden";
            }
        }
        private void Exit(Window p)
        {
            p.Close();
        }
        private void LoginGoogleClick(Window p)
        {
            this.loginWindow = p;
            var url = UserResponse.GetAutenticationURI(clientId, redirectURI).ToString();
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
            //Process.Start(url);
            //
            Thread.Sleep(1000);
            DisplayMemoryUsageInTitleAsync();
        }

        public const string clientId = "147215247319-dgmnt8q8l4rf4lu22hl7njqg7jo29l94.apps.googleusercontent.com";
        public const string clientSecret = "ogfkpR8xxt6fkP2TQw7QNJCU";
        public const string redirectURI = "urn:ietf:wg:oauth:2.0:oob";
        public bool flag = true;

        UserResponse access;       
        private void GetProfile(string approveCode)
        {
            access = UserResponse.Exchange(approveCode, clientId, clientSecret, redirectURI);
            var url = $"https://www.googleapis.com/oauth2/v3/userinfo?access_token={access.Access_token}";
            var wc = new WebClient();

            wc.Encoding = Encoding.UTF8;
            var jsonProfile = wc.DownloadString(url);

            profileResponse = JsonConvert.DeserializeObject<ProfileResponse>(jsonProfile);

            setNewUser();

            MainWindow mainForm = new MainWindow(user.ID);
            mainForm.Show();          
            loginWindow.Close();

        }
        void setNewUser()
        {
            user = new User();
            user.ID = userServices.getIdUserDefault();
            user.Name = profileResponse.family_name + " " + profileResponse.given_name;
            user.Gmail = profileResponse.email;
            user.Password = "1";
            user.PhoneNumer = "";
            user.CMND = "";

            userServices.pushUserToSql(user);
        }
        public class ProfileResponse
        {
            public string sub { get; set; }
            public string name { get; set; }
            public string given_name { get; set; }
            public string family_name { get; set; }
            public string picture { get; set; }
            public string email { get; set; }
            public bool email_verified { get; set; }
            public string locale { get; set; }
        }

        private void DisplayMemoryUsageInTitleAsync()
        {
            BackgroundWorker wrkr = new BackgroundWorker();
            wrkr.WorkerReportsProgress = true;

            wrkr.DoWork += (object sender, DoWorkEventArgs e) => {

                bool result;
                while (result = GetAppoveCodeGoogle())
                {
                    wrkr.ReportProgress(0, result);
                    Thread.Sleep(100);
                }

                wrkr.Dispose();
                Process[] procsChrome = Process.GetProcessesByName("chrome");
                foreach (Process chrome in procsChrome)
                {
                    if (chrome.MainWindowHandle == IntPtr.Zero)
                        continue;

                    AutomationElement element = AutomationElement.FromHandle(chrome.MainWindowHandle);
                    if (element != null)
                    {
                        Condition conditions = new AndCondition(
                       new PropertyCondition(AutomationElement.ProcessIdProperty, chrome.Id),
                       new PropertyCondition(AutomationElement.IsControlElementProperty, true),
                       new PropertyCondition(AutomationElement.IsContentElementProperty, true),
                       new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

                        AutomationElement elementx = element.FindFirst(TreeScope.Descendants, conditions);
                        var url = ((ValuePattern)elementx.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
                        if (url.Contains("accounts.google.com/o/oauth2/approval/v2/approvalnativeap"))
                        {
                            ((ValuePattern)elementx.GetCurrentPattern(ValuePattern.Pattern)).SetValue("https://google.com");
                            ChromeWrapper chromes = new ChromeWrapper();

                            chromes.SendKey((char)13, chrome);//enter
                            var arr = url.Split('&');
                            var approvalCode = WebUtility.HtmlDecode(arr[arr.Length - 1].Replace("approvalCode=", ""));
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                GetProfile(approvalCode);
                            }));
                        }
                    }
                }
                wrkr.Dispose();
            };
            wrkr.RunWorkerAsync();
        }

        public bool GetAppoveCodeGoogle()
        {
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            foreach (Process chrome in procsChrome)
            {
                if (chrome.MainWindowHandle == IntPtr.Zero)
                    continue;

                AutomationElement element = AutomationElement.FromHandle(chrome.MainWindowHandle);
                if (element != null)
                {
                    Condition conditions = new AndCondition(
                    new PropertyCondition(AutomationElement.ProcessIdProperty, chrome.Id),
                    new PropertyCondition(AutomationElement.IsControlElementProperty, true),
                    new PropertyCondition(AutomationElement.IsContentElementProperty, true),
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

                    AutomationElement elementx = element.FindFirst(TreeScope.Descendants, conditions);
                    var url = ((ValuePattern)elementx.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
                    if (url.Contains("accounts.google.com/o/oauth2/approval/v2/approvalnativeap"))
                    {
                        var arr = url.Split('&');
                        var approvalCode = WebUtility.HtmlDecode(arr[arr.Length - 1].Replace("approvalCode=", ""));
                        return false;
                    }

                }
            }
            return true;
        }






        private string account;
        public string Account
        {
            get { return account; }
            set
            {
                account = value;
                OnPropertyChanged("Account");
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
        private string nonExistAccountVisibility;
        public string NonExistAccountVisibility
        {
            get { return nonExistAccountVisibility; }
            set
            {
                nonExistAccountVisibility = value;
                OnPropertyChanged("NonExistAccountVisibility");
            }
        }

        private string incorrectPasswordVisibility;
        public string IncorrectPasswordVisibility
        {
            get { return incorrectPasswordVisibility; }
            set
            {
                incorrectPasswordVisibility = value;
                OnPropertyChanged("IncorrectPasswordVisibility");
            }
        }
        private string loginSuccessVisibility;
        public string LoginSuccessVisibility
        {
            get { return loginSuccessVisibility; }
            set { loginSuccessVisibility = value; OnPropertyChanged("LoginSuccessVisibility"); }
        }

        
    }
}


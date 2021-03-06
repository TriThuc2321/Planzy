using Newtonsoft.Json;
using Planzy.Commands;
using Planzy.Models.Users;
using Planzy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Condition = System.Windows.Automation.Condition;

namespace Planzy.LoginRegister
{
    class LoginViewModel : INotifyPropertyChanged
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
        Window parentView;
        DispatcherTimer timerUI;

        private DispatcherTimer timer;
        private string accountRemember;
        private string passwordRemember;
        private string temp_email;

        public ICommand LoginGoogleCommand { get; set; }
        public ICommand PasswordChangCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }
        public ICommand CheckBoxCommand { get; set; }

        public LoginViewModel()
        {
            if (IsConnectedToInternet())
            {
                userServices = new UserServices();
                listUsers = new List<User>(userServices.GetAll());
            }
            

            LoginGoogleCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { LoginGoogleClick(p); });
            LoginCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { LoginClick(p); });
            PasswordChangCommand = new RelayCommand2<PasswordBox>((p) => { return true; }, (p) => { Password = userServices.Encode(p.Password); });
            LoadWindowCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { LoadWindow(p); });
            ExitCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { timer.Stop(); p.Close(); checkBoxClick(); CreateTxt(); });
            RegisterCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { OpenRegisterWindow(p); });
            ForgotPasswordCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { OpenForgotPasswordWindow(p); });
            CheckBoxCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { checkBoxClick(); });

            NonExistAccountVisibility = "Collapsed";
            IncorrectPasswordVisibility = "Collapsed";
            LoginSuccessVisibility = "Hidden";
            EnterEmailVisibility = "Collapsed";
            AccountNotNullVisibility = "Collapsed";


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            timerUI = new DispatcherTimer();
            timerUI.Interval = TimeSpan.FromSeconds(0.5);
            timerUI.Tick += timerUI_Tick;
        }

        private void checkBoxClick()
        {
            if (RememberAccount == true)
            {
                accountRemember = Account;
                passwordRemember = Password;
            }
            else
            {
                accountRemember = "";
                passwordRemember = "";
            }
        }

        private void CreateTxt()
        {
            string path = "./RemeberAccount.txt";
            if (RememberAccount == true)
            {               
                if (accountRemember != null && accountRemember != "")
                {
                    StreamWriter sw = new StreamWriter(path);
                    sw.WriteLine(accountRemember);
                    sw.Close();
                }
                
            }
            else
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            
            
        }
        private void LoadWindow(Window p)
        {
            this.parentView = p;
            string path = "./RemeberAccount.txt";
            

            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                Account = sr.ReadLine();
                RememberAccount = true;
                sr.Close();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!IsConnectedToInternet())
            {
                timer.Stop();

                InternetCheckingView internetCheckingView = new InternetCheckingView(parentView, null);
                internetCheckingView.ShowDialog();
                userServices = new UserServices();
                listUsers = new List<User>(userServices.GetAll());
                timer.Start();
            }
        }
        private void timerUI_Tick(object sender, EventArgs e)
        {
            timerUI.Stop();
            MainWindow mainForm = new MainWindow(temp_email);
            mainForm.Show();
            parentView.Close(); 
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

        void LoginClick(Window p)
        {
            int i = 0;
            if(Account == null || Account == "")
            {
                NonExistAccountVisibility = "Collapsed";
                IncorrectPasswordVisibility = "Collapsed";
                LoginSuccessVisibility = "Hidden";
                EnterEmailVisibility = "Collapsed";
                AccountNotNullVisibility = "Visible";
                return;
            }
            for(i =0; i< listUsers.Count(); i++)
            {
                if(listUsers[i].ID == Account || listUsers[i].Gmail == Account)
                {
                    if (listUsers[i].Password == Password)
                    {
                        
                        NonExistAccountVisibility = "Collapsed";
                        IncorrectPasswordVisibility = "Collapsed";
                        LoginSuccessVisibility = "Visible";
                        EnterEmailVisibility = "Collapsed";
                        AccountNotNullVisibility = "Collapsed";
                        temp_email = listUsers[i].Gmail;
                        timer.Stop();
                        checkBoxClick();
                        CreateTxt();                       
                        timerUI.Start();
                        break;
                        
                    }
                    else
                    {
                        NonExistAccountVisibility = "Collapsed";
                        IncorrectPasswordVisibility = "Visible";
                        LoginSuccessVisibility = "Hidden";
                        EnterEmailVisibility = "Collapsed";
                        AccountNotNullVisibility = "Collapsed";
                        break;
                    }
                   
                }
            }
            if (i == listUsers.Count())
            {
                NonExistAccountVisibility = "Visible";
                IncorrectPasswordVisibility = "Collapsed";
                LoginSuccessVisibility = "Hidden";
                EnterEmailVisibility = "Collapsed";
                AccountNotNullVisibility = "Collapsed";
            }
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

        void OpenRegisterWindow(Window p)
        {
            Register registerWindow = new Register();
            registerWindow.Show();
            timer.Stop();
            p.Close();
        }
        void OpenForgotPasswordWindow(Window p)
        {

            if (Account == null || Account == "")
            {
                AccountNotNullVisibility = "Visible";
                return;
            }

            if (!checkEmail(Account))
            {
                NonExistAccountVisibility = "Collapsed";
                IncorrectPasswordVisibility = "Collapsed";
                LoginSuccessVisibility = "Hidden";
                EnterEmailVisibility = "Visible";                
            }
            
            else if (!userServices.ExistEmail(Account))
            {
                NonExistAccountVisibility = "Visible";
                IncorrectPasswordVisibility = "Collapsed";
                LoginSuccessVisibility = "Hidden";
                EnterEmailVisibility = "Collapsed";

            }

            if (checkEmail(Account) && userServices.ExistEmail(Account))
            {
                NonExistAccountVisibility = "Collapsed";
                IncorrectPasswordVisibility = "Collapsed";
                LoginSuccessVisibility = "Hidden";
                EnterEmailVisibility = "Collapsed";
                sendEmail(Account, p);
            }

        }
        void sendEmail(string email, Window p)
        {
            string from, pass, messageBody;
            Random rand = new Random();
            string randomCode = (rand.Next(999999)).ToString();
            MailMessage message = new MailMessage();
            string to = email;
            from = "planzyapplycation@gmail.com";
            pass = "ThucThienThang123";
            messageBody = "    Cám ơn bạn đã sử dụng Planzy, mã xác thực của bạn là: " + randomCode + "\n\n" + "____________________________________\n"
                 + "   Mọi thắc mắc xin liên lạc với chúng tôi qua địa chỉ email: planzyapplication@gmail.com hoặc qua số hotline : (+84) 834344655";
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Body = messageBody;
            message.Subject = "Đặt lại mật khẩu";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);
            try
            {
                smtp.Send(message);

                ForgotPassword forgotPassword = new ForgotPassword(Account, randomCode);
                forgotPassword.Show();
                timer.Stop();
                p.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void sendEmail_FirstLogin(string email)
        {
            string from, pass, messageBody;
            Random rand = new Random();
            string randomCode = (rand.Next(999999)).ToString();
            MailMessage message = new MailMessage();
            string to = email;
            from = "planzyapplycation@gmail.com";
            pass = "ThucThienThang123";
            messageBody = "    Bạn đã đăng nhập thành công vào Planzy, mật khẩu mặc định của bạn là: " + tempPassword + "\n\n" + "____________________________________\n"
                 + "   Mọi thắc mắc xin liên lạc với chúng tôi qua địa chỉ email: planzyapplication@gmail.com hoặc qua số hotline : (+84) 834344655";
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Body = messageBody;
            message.Subject = "Đăng nhập thành công";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);
            try
            {
                smtp.Send(message);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        string tempPassword;
        void setNewUser()
        {
            user = new User();
            user.ID = userServices.getIdUserDefault();
            user.Name = profileResponse.family_name + " " + profileResponse.given_name;
            user.Gmail = profileResponse.email;
            tempPassword = RandomString(6);
            user.Password = userServices.Encode(tempPassword);
            user.PhoneNumer = "";
            user.CMND = "";
            user.Address = "";
            user.Rank = "Customer";
        }
        bool checkEmail(string inputEmail)
        {
            if (inputEmail == null) return false;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return true;
            else
                return false;
        }

        private  Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #region Login google
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

            if (!userServices.ExistEmail(profileResponse.email))
            {
                setNewUser();
                sendEmail_FirstLogin(profileResponse.email);
                userServices.pushUserToSql(user);
            }

            MainWindow mainForm = new MainWindow(profileResponse.email);
            mainForm.Show();
            timer.Stop();
            loginWindow.Close();

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
                    if (elementx != null)
                    {
                        var url = ((ValuePattern)elementx.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
                        if (url.Contains("accounts.google.com/o/oauth2/approval/v2/approvalnativeap"))
                        {
                            var arr = url.Split('&');
                            var approvalCode = WebUtility.HtmlDecode(arr[arr.Length - 1].Replace("approvalCode=", ""));
                            return false;
                        }
                    }
                    

                }
            }
            return true;
        }

        #endregion

        private string account;
        public string Account
        {
            get { return account; }
            set
            {
                if(value == null || value =="")
                {
                    AccountNotNullVisibility = "Visible";
                }
                else
                {
                    AccountNotNullVisibility = "Collapsed";
                }
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

        private string enterEmailVisibility;
        public string EnterEmailVisibility
        {
            get { return enterEmailVisibility; }
            set { enterEmailVisibility = value; OnPropertyChanged("EnterEmailVisibility"); }
        }

        private bool rememberAccount;
        public bool RememberAccount
        {
            get { return rememberAccount; }
            set
            {
                rememberAccount = value;
                OnPropertyChanged("RememberAccount");
            }
        }

        private string accountNotNullVisibility;
        public string AccountNotNullVisibility
        {
            get { return accountNotNullVisibility; }
            set { accountNotNullVisibility = value; OnPropertyChanged("AccountNotNullVisibility"); }
        }

        public string KiemTraInput { get; private set; }
    }
}


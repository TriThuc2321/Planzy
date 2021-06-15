using Planzy.Commands;
using Planzy.Models.Users;
using Planzy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Planzy.LoginRegister
{
    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        private static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        #region onpropertychange
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        Window parentView;

        public ICommand LoadWindowCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand Resendommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand PasswordChangCommand { get; set; }
        public ICommand ConfirmPasswordChangCommand { get; set; }
        public ICommand LoginXamlCommand { get; set; }

        string randomCode;
        Window parentWindow;
        RegisterViewModel parentRegister;

        public ForgotPasswordViewModel(string email, string verify, Window parentW, RegisterViewModel parentR)
        {
            Email = email;
            randomCode = verify;
            this.parentWindow = parentW;
            this.parentRegister = parentR;

            EnterEmailVisibility = "Collapsed";
            PasswordNotNullVisibility = "Collapsed";
            ConfirmPasswordIncorrectVisibility = "Collapsed";
            IncorrectVerifyCodeVisibility = "Collapsed";

            LoadWindowCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { this.parentView = p; });
            ExitCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { p.Close(); });
            Resendommand = new RelayCommand2<Object>((p) => { return true; }, (p) => { ResendClick(); });
            ResetCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => {
                if(parentWindow == null && parentRegister == null)
                {
                    ResetClick(p);
                }
                else
                {
                    ConfirmClick(p);
                }
            });
            PasswordChangCommand = new RelayCommand2<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            ConfirmPasswordChangCommand = new RelayCommand2<PasswordBox>((p) => { return true; }, (p) => { ConfirmPassword = p.Password; });
            LoginXamlCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { OpenLoginWindow(p); });

            checkParent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!IsConnectedToInternet())
            {
                timer.Stop();

                InternetCheckingView internetCheckingView = new InternetCheckingView(parentView, null);
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

        void ConfirmClick(Window p)
        {
            if (!checkEmail(Email))
            {
                EnterEmailVisibility = "Visible";
            }
            else
            {
                EnterEmailVisibility = "Collapsed";
            }

            if (randomCode != VerifyCode)
            {
                IncorrectVerifyCodeVisibility = "Visible";
            }
            else
            {
                IncorrectVerifyCodeVisibility = "Collapsed";
            }
           

            if (checkEmail(Email) && randomCode == VerifyCode )
            {
                this.parentRegister.isConfirmEmail = true;
                this.parentRegister.emailIsConfirm = Email;


                this.parentRegister.EmailConfirmedVisibility = "Visible";
                this.parentRegister.UnconfirmedEmailVisibility = "Collapsed";


                parentWindow.Show();
                if(parentRegister!= null) parentRegister.timerRegister.Start();
                timer.Stop();
                p.Close();
            }
        }
        void checkParent()
        {
            if(parentWindow == null && parentRegister == null)
            {
                ButtonTxt = "RESET";
                PasswordBoxVisibility = "Visible";
                ConfirmPasswordBoxVisibility = "Visible";
            }
            else
            {
                ButtonTxt = "CONFIRM";
                PasswordBoxVisibility = "Collapsed";
                ConfirmPasswordBoxVisibility = "Collapsed";
            }
        }
        void OpenLoginWindow(Window p)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            timer.Stop();
            p.Close();
        }
        void ResendClick()
        {
            if (checkEmail(Email))
            {
                sendEmail(Email);
                EnterEmailVisibility = "Collapsed";
                PasswordNotNullVisibility = "Collapsed";
                ConfirmPasswordIncorrectVisibility = "Collapsed";
                IncorrectVerifyCodeVisibility = "Collapsed";
            }
            else
            {
                EnterEmailVisibility = "Visible";
                PasswordNotNullVisibility = "Collapsed";
                ConfirmPasswordIncorrectVisibility = "Collapsed";
                IncorrectVerifyCodeVisibility = "Collapsed";
            }
            VerifyCode = "";
            Password = "";
            ConfirmPassword = "";
        }

        void ResetClick(Window p)
        {
            if (!checkEmail(Email))
            {
                EnterEmailVisibility = "Visible";
            }
            else
            {
                EnterEmailVisibility = "Collapsed";
            }

            if (randomCode != VerifyCode)
            {
                IncorrectVerifyCodeVisibility = "Visible";
            }
            else
            {
                IncorrectVerifyCodeVisibility = "Collapsed";
            }

            if (Password == null)
            {
                PasswordNotNullVisibility = "Visible";
            }
            else
            {
                PasswordNotNullVisibility = "Collapsed";
            }

            if(ConfirmPassword != Password)
            {
                ConfirmPasswordIncorrectVisibility = "Visible";
            }
            else
            {
                ConfirmPasswordIncorrectVisibility = "Collapsed";
            }

            if(checkEmail(Email) && randomCode == VerifyCode && Password != null && ConfirmPassword == Password)
            {
                ResetPassword(Email);
                MainWindow main = new MainWindow(Email);
                main.Show();
                timer.Stop();
                p.Close();
            }
        }

        void ResetPassword(string email)
        {
            string query = "UPDATE HANH_KHACH SET MAT_KHAU = @matkhau WHERE GMAIL = @gmail";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@matkhau", Encode(Password));
            command.Parameters.AddWithValue("@gmail", Email);
            
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {

            }
            finally
            {
                connection.Close();
            }
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

        void sendEmail(string email)
        {
            string from, pass, messageBody;
            Random rand = new Random();
            string randomCode = (rand.Next(999999)).ToString();
            this.randomCode = randomCode;
            MailMessage message = new MailMessage();
            string to = email;
            from = "planzyapplycation@gmail.com";
            pass = "ThucThienThang123";
            messageBody = "Thank for your using Planzy, this is your password reseting code: " + randomCode;
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Body = messageBody;
            message.Subject = "Password reset code Planzy";
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
        public string Encode(string stringValue)
        {
            MD5 mh = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(stringValue);
            byte[] hash = mh.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }


        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
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
        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
            }
        }
        private string verifyCode;
        public string VerifyCode
        {
            get { return verifyCode; }
            set
            {
                verifyCode = value;
                OnPropertyChanged("VerifyCode");
            }
        }
        private string enterEmailVisibility;
        public string EnterEmailVisibility
        {
            get { return enterEmailVisibility; }
            set { enterEmailVisibility = value; OnPropertyChanged("EnterEmailVisibility"); }
        }
        private string incorrectVerifyCodeVisibility;
        public string IncorrectVerifyCodeVisibility
        {
            get { return incorrectVerifyCodeVisibility; }
            set { incorrectVerifyCodeVisibility = value; OnPropertyChanged("IncorrectVerifyCodeVisibility"); }
        }
        private string confirmPasswordIncorrectVisibility;
        public string ConfirmPasswordIncorrectVisibility
        {
            get { return confirmPasswordIncorrectVisibility; }
            set { confirmPasswordIncorrectVisibility = value; OnPropertyChanged("ConfirmPasswordIncorrectVisibility"); }
        }
        private string passwordNotNullVisibility;
        public string PasswordNotNullVisibility
        {
            get { return passwordNotNullVisibility; }
            set { passwordNotNullVisibility = value; OnPropertyChanged("PasswordNotNullVisibility"); }
        }

        private string buttonTxt;
        public string ButtonTxt
        {
            get { return buttonTxt; }
            set
            {
                buttonTxt = value;
                OnPropertyChanged("ButtonTxt");
            }
        }


        private string confirmPasswordBoxVisibility;
        public string ConfirmPasswordBoxVisibility
        {
            get { return confirmPasswordBoxVisibility; }
            set { confirmPasswordBoxVisibility = value; OnPropertyChanged("ConfirmPasswordBoxVisibility"); }
        }
        private string passwordBoxVisibility;
        private DispatcherTimer timer;

        public string PasswordBoxVisibility
        {
            get { return passwordBoxVisibility; }
            set { passwordBoxVisibility = value; OnPropertyChanged("PasswordBoxVisibility"); }
        }
    }
}

using Planzy.Commands;
using Planzy.Models.Users;
using Planzy.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Planzy.LoginRegister
{
    public class RegisterViewModel: INotifyPropertyChanged
    {
        #region onpropertychange
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        Window registerWindow;
        UserServices userServices;
        User user;
        List<User> listUsers;
        public bool isConfirmEmail = false;
        string randomCode;
        public string emailIsConfirm = null;

        public ICommand ConfirmEmailCommand { get; set; }
        public ICommand PasswordChangCommand { get; set; }
        public ICommand ConfirmPasswordChangCommand { get; set; }
        public ICommand LoginXamlCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public RegisterViewModel()
        {

            userServices = new UserServices();
            listUsers = new List<User>(userServices.GetAll());

            DefaultTxt();


            ConfirmEmailCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { ConfirmEmail(p); });
            PasswordChangCommand = new RelayCommand2<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            ConfirmPasswordChangCommand = new RelayCommand2<PasswordBox>((p) => { return true; }, (p) => { ConfirmPassword = p.Password; });
            LoginXamlCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { OpenLoginWindow(p); });
            ExitCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { p.Close(); });
            RegisterCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { Register(p); });
        }
        void DefaultTxt()
        {
            AccountExistVisibility = "Collapsed";
            AccountNotNullVisibility = "Collapsed";
            EnterEmailVisibility = "Collapsed";
            EmailExistVisibility = "Collapsed";
            IncorrectVerifyCodeVisibility = "Collapsed";
            ConfirmPasswordIncorrectVisibility = "Collapsed";
            PasswordNotNullVisibility = "Collapsed";
            UnconfirmedEmailVisibility = "Collapsed";
            EmailConfirmedVisibility = "Collapsed";
        }
        void ConfirmEmail(Window p)
        {
            if (checkEmail(Email))
            {
                sendEmail(Email);
                ForgotPassword forgotPassword = new ForgotPassword(Email, this.randomCode, p, this);
                forgotPassword.Show();
                p.Hide();
            }
            else
            {
                EnterEmailVisibility = "Visible";
            }
        }
        void Register(Window p)
        {

            if (userServices.ExistId(Account)) AccountExistVisibility = "Visible";
            else AccountExistVisibility = "Collapsed";

            if (Account == null) AccountNotNullVisibility = "Visible";
            else AccountNotNullVisibility = "Collapsed";

            if (userServices.ExistEmail(Email)) EmailExistVisibility = "Visible";
            else EmailExistVisibility = "Collapsed";

            if (!checkEmail(Email)) EnterEmailVisibility = "Visible";
            else EnterEmailVisibility = "Collapsed";

            if (Password == null) PasswordNotNullVisibility = "Visible";
            else PasswordNotNullVisibility = "Collapsed";

            if (Password != ConfirmPassword) ConfirmPasswordIncorrectVisibility = "Visible";
            else ConfirmPasswordIncorrectVisibility = "Collapsed";

            if (isConfirmEmail)
            {
                EmailConfirmedVisibility = "Visible";
                UnconfirmedEmailVisibility = "Collapsed";
            }
            else
            {
                EmailConfirmedVisibility = "Collapsed";
                UnconfirmedEmailVisibility = "Visible";
            }



            if(!userServices.ExistId(Account) && Account != null && !userServices.ExistEmail(Email) && checkEmail(Email) && Password != null && Password == ConfirmPassword && isConfirmEmail)
            {
                user = new User();
                user.Gmail = Email;
                user.Password = Password;
                user.ID = Account;
                userServices.pushUserToSql(user);

                MainWindow main = new MainWindow(Email);
                main.Show();
                p.Close();
            }
        }
        void OpenLoginWindow(Window p)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            p.Close();
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

        private string accountExistVisibility;
        public string AccountExistVisibility
        {
            get { return accountExistVisibility; }
            set { accountExistVisibility = value; OnPropertyChanged("AccountExistVisibility"); }
        }
        private string accountNotNullVisibility;
        public string AccountNotNullVisibility
        {
            get { return accountNotNullVisibility; }
            set { accountNotNullVisibility = value; OnPropertyChanged("AccountNotNullVisibility"); }
        }


        private string enterEmailVisibility;
        public string EnterEmailVisibility
        {
            get { return enterEmailVisibility; }
            set { enterEmailVisibility = value; OnPropertyChanged("EnterEmailVisibility"); }
        }
        private string emailExistVisibility;
        public string EmailExistVisibility
        {
            get { return emailExistVisibility; }
            set { emailExistVisibility = value; OnPropertyChanged("EmailExistVisibility"); }
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

        private string emailConfirmedVisibility;
        public string EmailConfirmedVisibility
        {
            get { return emailConfirmedVisibility; }
            set { emailConfirmedVisibility = value; OnPropertyChanged("EmailConfirmedVisibility"); }
        }
        private string unconfirmedEmailVisibility;
        public string UnconfirmedEmailVisibility
        {
            get { return unconfirmedEmailVisibility; }
            set { unconfirmedEmailVisibility = value; OnPropertyChanged("UnconfirmedEmailVisibility"); }
        }
    }
    
}

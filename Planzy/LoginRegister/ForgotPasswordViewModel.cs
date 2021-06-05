using Planzy.Commands;
using Planzy.Models.Users;
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

namespace Planzy.LoginRegister
{
    class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        private static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PlanzyConnection"].ConnectionString);
        #region onpropertychange
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public ICommand ExitCommand { get; set; }
        public ICommand Resendommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand PasswordChangCommand { get; set; }
        public ICommand ConfirmPasswordChangCommand { get; set; }

        string randomCode;

        public ForgotPasswordViewModel(string email, string verify)
        {
            Email = email;
            randomCode = verify;

            EnterEmailVisibility = "Hidden";
            PasswordNotNullVisibility = "Hidden";
            ConfirmPasswordIncorrectVisibility = "Hidden";
            IncorrectVerifyCodeVisibility = "Hidden";

            ExitCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { p.Close(); });
            Resendommand = new RelayCommand2<Object>((p) => { return true; }, (p) => { ResendClick(); });
            ResetCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { ResetClick(p); });
            PasswordChangCommand = new RelayCommand2<PasswordBox>((p) => { return true; }, (p) => { Password = Encode(p.Password); });
            ConfirmPasswordChangCommand = new RelayCommand2<PasswordBox>((p) => { return true; }, (p) => { ConfirmPassword = Encode(p.Password); });
        }
        
        void ResendClick()
        {
            if (checkEmail(Email))
            {
                sendEmail(Email);
                EnterEmailVisibility = "Hidden";
                PasswordNotNullVisibility = "Hidden";
                ConfirmPasswordIncorrectVisibility = "Hidden";
                IncorrectVerifyCodeVisibility = "Hidden";
            }
            else
            {
                EnterEmailVisibility = "Visible";
                PasswordNotNullVisibility = "Hidden";
                ConfirmPasswordIncorrectVisibility = "Hidden";
                IncorrectVerifyCodeVisibility = "Hidden";
            }
        }

        void ResetClick(Window p)
        {
            if (!checkEmail(Email))
            {
                EnterEmailVisibility = "Visible";
            }
            else
            {
                EnterEmailVisibility = "Hidden";
            }

            if (randomCode != VerifyCode)
            {
                IncorrectVerifyCodeVisibility = "Visible";
            }
            else
            {
                IncorrectVerifyCodeVisibility = "Hidden";
            }

            if (Password == null)
            {
                PasswordNotNullVisibility = "Visible";
            }
            else
            {
                PasswordNotNullVisibility = "Hidden";
            }

            if(ConfirmPassword != Password)
            {
                ConfirmPasswordIncorrectVisibility = "Visible";
            }
            else
            {
                ConfirmPasswordIncorrectVisibility = "Hidden";
            }

            if(checkEmail(Email) && randomCode == VerifyCode && Password != null && ConfirmPassword == Password)
            {
                ResetPassword(Email);
                MainWindow main = new MainWindow(Email);
                main.Show();
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
    }
}

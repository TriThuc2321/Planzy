using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Planzy.LoginRegister
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        ForgotPasswordViewModel forgotPasswordViewModel;
        public ForgotPassword()
        {
            InitializeComponent();
        }
        public ForgotPassword(string email, string verifyCode)
        {
            InitializeComponent();
            forgotPasswordViewModel = new ForgotPasswordViewModel(email, verifyCode, null, null);
            this.DataContext = forgotPasswordViewModel;
        }
        public ForgotPassword(string email, string verifyCode, Window p, RegisterViewModel r)
        {
            InitializeComponent();
            forgotPasswordViewModel = new ForgotPasswordViewModel(email, verifyCode, p, r);
            this.DataContext = forgotPasswordViewModel;
        }
    }
}

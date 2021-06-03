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

namespace Planzy.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        #region PropertyChange       
       
        #endregion
        LoginViewModel loginViewModel;
        public Login()
        {            
            InitializeComponent();
            loginViewModel = new LoginViewModel();
            this.DataContext = loginViewModel;
        }


      
       

        
    }
}

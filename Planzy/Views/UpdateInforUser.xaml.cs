using Planzy.Models.Users;
using Planzy.ViewModels;
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

namespace Planzy.Views
{
    /// <summary>
    /// Interaction logic for UpdateInforUser.xaml
    /// </summary>
    public partial class UpdateInforUser : Window
    {
        UpdateInfoUserViewModel update;
        public UpdateInforUser(User user, Window p)
        {
            InitializeComponent();
            update = new UpdateInfoUserViewModel(user,p);
            this.DataContext = update;
        }
    }
}

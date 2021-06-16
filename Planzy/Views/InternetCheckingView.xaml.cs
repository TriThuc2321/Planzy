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
    /// Interaction logic for InternetCheckingView.xaml
    /// </summary>
    public partial class InternetCheckingView : Window
    {
        InternetCheckingViewModel viewModel;
        public InternetCheckingView(Window p, Window p2)
        {
            InitializeComponent();
            viewModel = new InternetCheckingViewModel(p, p2);
            this.DataContext = viewModel;
        }
    }
}

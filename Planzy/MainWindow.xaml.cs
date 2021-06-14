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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Planzy.Models.Users;
using Planzy.ViewModels;


namespace Planzy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PlanzyViewModel planzyViewModel;
        public MainWindow()
        {
            InitializeComponent();           
        }
        public MainWindow(string gmail)
        {
            InitializeComponent();
            Window parentWindow = Window.GetWindow(this);
            planzyViewModel = new PlanzyViewModel(gmail, parentWindow);
            this.DataContext = planzyViewModel;
        }

    }
}

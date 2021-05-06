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
using MaterialDesignThemes.Wpf;
namespace Planzy.Views
{
    /// <summary>
    /// Interaction logic for PlanzyView.xaml
    /// </summary>
    public partial class PlanzyView : UserControl
    {
        Grid gr_temp;
        Label lb_temp;
        PackIcon ic_temp;
        Button btn_temp;
        public PlanzyView()
        {
            InitializeComponent();
            gr_temp = gr_DatVe;
            lb_temp = lb_DatVe;
            ic_temp = ic_DatVe;
            btn_temp = btn_DatVe;
        }
        #region Xu ly giao dien
        private void ChoseLayout(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            if (sender == btn_DatVe)
            {
                gr_temp.Visibility = Visibility.Hidden;
                lb_temp.Foreground = (Brush)bc.ConvertFrom("#FF5867AC");
                ic_temp.Foreground = (Brush)bc.ConvertFrom("#FF5867AC");
                btn_temp.Background = (Brush)bc.ConvertFrom("#ffffffff");
                //---
                gr_temp = gr_DatVe; gr_temp.Visibility = Visibility.Visible;
                lb_temp = lb_DatVe; lb_temp.Foreground = (Brush)bc.ConvertFrom("#ffffffff");
                ic_temp = ic_DatVe; ic_temp.Foreground = (Brush)bc.ConvertFrom("#ffffffff");
                btn_temp = btn_DatVe; btn_temp.Background = (Brush)bc.ConvertFrom("#FF5867AC");
            }   
            else if (sender == btn_BanVe)
            {
                gr_temp.Visibility = Visibility.Hidden;
                lb_temp.Foreground = (Brush)bc.ConvertFrom("#FF5867AC");
                ic_temp.Foreground = (Brush)bc.ConvertFrom("#FF5867AC");
                btn_temp.Background = (Brush)bc.ConvertFrom("#ffffffff");
                //---
                gr_temp = gr_BanVe; gr_temp.Visibility = Visibility.Visible;
                lb_temp = lb_BanVe; lb_temp.Foreground = (Brush)bc.ConvertFrom("#ffffffff");
                ic_temp = ic_BanVe; ic_temp.Foreground = (Brush)bc.ConvertFrom("#ffffffff");
                btn_temp = btn_BanVe; btn_temp.Background = (Brush)bc.ConvertFrom("#FF5867AC");
            }   
            else if (sender == btn_TraCuu)
            {
                gr_temp.Visibility = Visibility.Hidden;
                lb_temp.Foreground = (Brush)bc.ConvertFrom("#FF5867AC");
                ic_temp.Foreground = (Brush)bc.ConvertFrom("#FF5867AC");
                btn_temp.Background = (Brush)bc.ConvertFrom("#ffffffff");
                //---
                gr_temp = gr_Tracuu; gr_temp.Visibility = Visibility.Visible;
                lb_temp = lb_TraCuu; lb_temp.Foreground = (Brush)bc.ConvertFrom("#ffffffff");
                ic_temp = ic_TraCuu; ic_temp.Foreground = (Brush)bc.ConvertFrom("#ffffffff");
                btn_temp = btn_TraCuu; btn_temp.Background = (Brush)bc.ConvertFrom("#FF5867AC");
            }   
            else if (sender == btn_NhanLich)
            {
                gr_temp.Visibility = Visibility.Hidden;
                lb_temp.Foreground = (Brush)bc.ConvertFrom("#FF5867AC");
                ic_temp.Foreground = (Brush)bc.ConvertFrom("#FF5867AC");
                btn_temp.Background = (Brush)bc.ConvertFrom("#ffffffff");
                //---
                gr_temp = gr_NhanLich; gr_temp.Visibility = Visibility.Visible;
                lb_temp = lb_NhanLich; lb_temp.Foreground = (Brush)bc.ConvertFrom("#ffffffff");
                ic_temp = ic_NhanLich; ic_temp.Foreground = (Brush)bc.ConvertFrom("#ffffffff");
                btn_temp = btn_NhanLich; btn_temp.Background = (Brush)bc.ConvertFrom("#FF5867AC");
            }   
            else
            {
                gr_temp.Visibility = Visibility.Hidden;
                lb_temp.Foreground = (Brush)bc.ConvertFrom("#FF5867AC");
                ic_temp.Foreground = (Brush)bc.ConvertFrom("#FF5867AC");
                btn_temp.Background = (Brush)bc.ConvertFrom("#ffffffff");
                //---
                gr_temp = gr_DoanhThu; gr_temp.Visibility = Visibility.Visible;
                lb_temp = lb_DoanhThu; lb_temp.Foreground = (Brush)bc.ConvertFrom("#ffffffff");
                ic_temp = ic_DoanhThu; ic_temp.Foreground = (Brush)bc.ConvertFrom("#ffffffff");
                btn_temp = btn_DoanhThu; btn_temp.Background = (Brush)bc.ConvertFrom("#FF5867AC");
            }    
        }
        #endregion
    }
}

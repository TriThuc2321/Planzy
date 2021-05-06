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

namespace Planzy.Views
{
    /// <summary>
    /// Interaction logic for Button_Function_View.xaml
    /// </summary>
    public partial class Button_Function_View : UserControl
    {
        public Button_Function_View()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string newContent { get; set; }
        public string newForeground { get; set; }
        public bool IsChoosed { get; set; }
        public string newBackground { get; set; }

    }
}

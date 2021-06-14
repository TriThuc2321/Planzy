using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for MyChartToolTipForDTNam.xaml
    /// </summary>
    public partial class MyChartToolTipForDTNam : IChartTooltip
    {
        public MyChartToolTipForDTNam()
        {
            InitializeComponent();
            DataContext = this;
        }

        private TooltipData data;

        public TooltipData Data
        {
            get { return data; }
            set { data = value; OnPropertyChanged("Data"); }
        }
        public TooltipSelectionMode? SelectionMode { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

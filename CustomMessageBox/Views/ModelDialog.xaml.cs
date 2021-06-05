using System.Windows;
using System.Windows.Input;


namespace CustomMessageBox
{
    /// <summary>
    /// Interaction logic for VirtuosoMainMessageBox.xaml
    /// </summary>
    public partial class ModelDialog : Window
    {
        #region Initialization of components
        public ModelDialog()
        {
            ModelDialogViewModel viewModel = new ModelDialogViewModel();
            InitializeComponent();
            DataContext = viewModel;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        #endregion
    }
}

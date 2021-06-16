using Planzy.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Planzy.ViewModels
{
    class InternetCheckingViewModel : INotifyPropertyChanged
    {
        #region onpropertychange
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ICommand ExitCommand { get; set; }
        public ICommand TryAgainCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
 
        Window parentView;

        DispatcherTimer timer;
        DispatcherTimer timerAuto;
        public InternetCheckingViewModel(Window p1, Window p2)
        {
            TextVisibility = "Visible";

            ExitCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { p1.Close(); if(p2!=null) p2.Close(); p.Close(); });
            TryAgainCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { tryAgain(p); });
            LoadWindowCommand = new RelayCommand2<Window>((p) => { return true; }, (p) => { this.parentView = p; });

            timerAuto = new DispatcherTimer();
            timerAuto.Interval = TimeSpan.FromSeconds(1);
            timerAuto.Tick += timerAuto_Tick;
            timerAuto.Start();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
        }

        private void timerAuto_Tick(object sender, EventArgs e)
        {
            if (IsConnectedToInternet())
            {
                this.parentView.Close();
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (IsConnectedToInternet())
            {
                this.parentView.Close();
            }
            else
            {
                TextVisibility = "Visible";
            }
            timer.Stop();
        }

        private void tryAgain(Window p)
        {
            TextVisibility = "Collapsed";
            timer.Start();
        }

        public bool IsConnectedToInternet()
        {
            try
            {
                System.Net.IPHostEntry i = System.Net.Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch
            {
                return false;
            }
        }


        private string textVisibility;
        public string TextVisibility
        {
            get { return textVisibility; }
            set { 
                textVisibility = value;
                OnPropertyChanged("TextVisibility");
            }
        }

    }
}

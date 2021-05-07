using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planzy.Models.SanBayModel;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Planzy.ViewModels
{
    class PlanzyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        SanBayServices sanBayServices;
        
        public PlanzyViewModel()
        {
            sanBayServices = new SanBayServices();
            LoadData();
        }
        private ObservableCollection<SanBay> sanbaysList;

        public ObservableCollection<SanBay> SanbaysList
        {
            get { return sanbaysList; }
            set { sanbaysList = value; OnPropertyChanged("SanbaysList"); }
        }

        private void LoadData()
        {
            SanbaysList = new ObservableCollection<SanBay>(sanBayServices.GetAll());
        }

        private SanBay sanBayDiDaChon = new SanBay();
        public SanBay SanBayDiDaChon
        {
            get { return sanBayDiDaChon; }
            set 
            {
                sanBayDiDaChon = value;
                OnPropertyChanged("SanBayDiDaChon");
                try
                {
                    if (sanBayDiDaChon.Id.Equals(sanBayDenDaChon.Id))
                    {
                        MyProperty = 22;
                    }
                }
                catch
                {

                }
            }
        }
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; OnPropertyChanged("MyProperty"); }
        }


        private SanBay sanBayDenDaChon = new SanBay();
        public SanBay SanBayDenDaChon
        {
            get { return sanBayDenDaChon; }
            set
            {
                sanBayDenDaChon = value;
                OnPropertyChanged("SanBayDenDaChon");
                try
                {
                    if (sanBayDiDaChon.Id.Equals(sanBayDenDaChon.Id))
                    {
                        MyProperty = 11;
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

    }
}

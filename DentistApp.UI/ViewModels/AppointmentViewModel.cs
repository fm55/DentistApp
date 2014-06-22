using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DentistApp.DAL;
using DentistApp.BL;
using System.Windows;
using System.Windows.Input;
using DentistApp.UI.Commands;
using DentistApp.Model;
using System.ComponentModel;
using System.Windows.Data;
using DentistApp.UI.UserControls;


namespace DentistApp.UI.ViewModels
{
    public class AppointmentViewModel : BaseViewModel
    {
        #region Properties
        /////properties////
        //list of appointments
        public ObservableCollection<Appointment> Appointments { get; set; }
        public ObservableCollection<Note> PatientNotes { get; set; }
        public AppointmentController AppointmentsController {get;set;}



        public ICommand DeleteAppointment
        {
            get
            {
                return new DelegateCommand((object o) =>
                {
                    if (!ShouldDelete()) return;

                    AppointmentsController.Delete(AppointmentsController.List((int)o).First());
                    var apps = AppointmentsController.List();
                    Appointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d => d.StartTime));
                    RaisePropertyChanged("Appointments");
                },
                (object o) =>
                {
                    return true;
                });
            }
        }


        public ICommand SaveAppointment
        {
            get
            {
                return new DelegateCommand((object o) =>
                {

                    var apps = AppointmentsController.List();
                    Appointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d => d.StartTime));
                    RaisePropertyChanged("Appointments");
                },
                (object o) =>
                {
                    return true;
                });
            }
        }

        private ICollectionView _items;
        public ICollectionView Items
        {
            get
            {
                if (_items == null)
                {
                    AppointmentsController = new AppointmentController();
                    var apps = AppointmentsController.List();
                    Appointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d=>d.StartTime));
                    RaisePropertyChanged("Appointments");
                    _items = System.Windows.Data.CollectionViewSource.GetDefaultView(apps.ToList<Appointment>());
                    _items.GroupDescriptions.Add(new PropertyGroupDescription("StartTime"));
                }

                return _items;
            }
        }

        public ICommand CreateAppointment
        {
            get
            {
                return new DelegateCommand(createAppointment, (object o) =>
                {
                    return true;
                });
            }

        }

        public void createAppointment(object o)
        {

            Window window = new Window
            {
                Title = "Create Appointment",
                Content = new CreateAppointmentUserControl(),
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            window.ShowDialog();
            Appointments = new ObservableCollection<Appointment>(AppointmentsController.List().OrderByDescending(d => d.StartTime));
            RaisePropertyChanged("Appointments");
        }
        #endregion

        #region Commands
        /////commands/////
        

        #endregion






        public AppointmentViewModel()
        {
            var items = Items;
            RaisePropertyChanged("Appointments");
            RaisePropertyChanged("Items");
        }
       



        

    }
}

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
using System.Windows.Controls;
using DentistApp.UI.Frames;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using DentistApp.UI.Events;


namespace DentistApp.UI.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Properties
        /////properties////
        //list of appointments
        public ObservableCollection<Appointment> Appointments { get; set; }
        public AppointmentController AppointmentsController {get;set;}
        NoteController NoteController = new NoteController();
        private void SetNotes()
        {
            foreach (var appointment in Appointments)
            {
                appointment.Notes = NoteController.GetNotesForAppointment(appointment.AppointmentId);
            }
        }
        public ICommand DeleteAppointment
        {
            get
            {
                return new DentistApp.UI.Commands.DelegateCommand((object o) =>
                {
                    if (!ShouldDelete()) return;
                    AppointmentsController.Delete(AppointmentsController.List((int)o).First());
                    _items = Items;
                },
                (object o) =>
                {
                    return true;
                });
            }
        }

        Window editWindow;
        public ICommand EditAppointment
        {
            get
            {
                return new DelegateCommand((object o) =>
                {

                    var vm = new CreateAppointmentUserControl(o as Appointment);
                    vm.RaisedClosed += new EventHandler(viewModel_RaisedClosed);
                    editWindow = new Window
                    {
                        Title = "Edit Appointment",
                        Content = vm,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        ResizeMode = ResizeMode.NoResize
                    };

                    editWindow.ShowDialog();
                    _items = Items;
                },
                (object o) =>
                {
                    return true;
                });
            }
        }
        void viewModel_RaisedClosed(object sender, EventArgs e)
        {
            if (editWindow != null)
                editWindow.Close();

            var apps = AppointmentsController.List(0, false, DateTime.Now.Date, DateTime.Now.AddDays(14));
            Appointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d => d.StartTime));
            RaisePropertyChanged("Appointments");
        }

        private ICollectionView _items;
        public ICollectionView Items
        {
            get
            {
                 AppointmentsController = new AppointmentController();
                    var apps = AppointmentsController.List(0, false, DateTime.Now.Date, DateTime.Now.AddDays(14));
                    Appointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d=>d.StartTime));
                    SetNotes();
                    RaisePropertyChanged("Appointments");
                    _items = System.Windows.Data.CollectionViewSource.GetDefaultView(apps.ToList<Appointment>());
                    _items.GroupDescriptions.Add(new PropertyGroupDescription("StartTime"));

                return _items;
            }
        }

        
        #endregion
        Window noteWindow;
        public ICommand CreateNote
        {
            get
            {
                return new DelegateCommand((object o) =>
                {
                    var note = new Note { AppointmentId = (int)o };
                    var vm = new CreateNoteUserControl(new NoteViewModel(note));
                    vm.RaiseClosed += new EventHandler(vm_RaisedClosed);
                    noteWindow = new Window
                    {
                        Title = "New Note",
                        Content = vm,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        ResizeMode = ResizeMode.NoResize
                    };

                    noteWindow.ShowDialog();
                    _items = Items;
                },
                (object o) =>
                {
                    return true;
                });
            }
        }

        void vm_RaisedClosed(object sender, EventArgs e)
        {
            if (noteWindow != null)
                noteWindow.Close();
        }

        IUnityContainer _container { get; set; }
        IEventAggregator _eventAggregator { get; set; }
        public HomeViewModel(IUnityContainer unityContainer, IEventAggregator eventAggregator)
        {
            _container = unityContainer;
            _eventAggregator = eventAggregator;
            var items = Items;
            RaisePropertyChanged("Appointments");
            RaisePropertyChanged("Items");
        }


        public Page SelectedPage { get; set; }
        public ICommand Navigate { get { return new DelegateCommand(NavigateCommand, canExecute); } }
        public void NavigateCommand(object o)
        {
            if (o.Equals("Appointments"))
                SelectedPage = new Appointments();

            else if (o.Equals("Operations"))
                SelectedPage = new Operations();

            else if (o.Equals("Home"))
                SelectedPage = new HomePage(_container, _eventAggregator);

            else if (o.Equals("Notes"))
                SelectedPage = new Notes(_container, _eventAggregator);

            else if (o.Equals("Exit"))
                Application.Current.Shutdown(110);

            else SelectedPage = new Patients(_container.Resolve<IPatientViewModel>()); ;
            RaisePropertyChanged("SelectedPage");
            _eventAggregator.GetEvent<SelectedMenuItemEvent>().Publish(SelectedPage);
        }

        
        public bool canExecute(object e)
        {
            return true;
        }

        

    }
}

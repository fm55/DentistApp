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
        public ObservableCollection<Note> AppointmenNotes { get; set; }
        public AppointmentController AppointmentsController {get;set;}
        public NoteController NoteController = new NoteController();
        public bool OnlyNotFullyPaid { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        private void SetNotes()
        {
            foreach (var a in Appointments)
            {
                a.Notes = NoteController.GetNotesForAppointment(a.AppointmentId);
            }
        }

        public ICommand SearchAppointments
        {
            get
            {
                return new DelegateCommand((object o) =>
                {
                    var apps = AppointmentsController.List(0, OnlyNotFullyPaid, Start, End);
                    Appointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d => d.StartTime));

                    SetNotes();

                    RaisePropertyChanged("Appointments");
                },
                (object o) =>
                {
                    return true;
                });
            }
        }

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
                    SetNotes();
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
                    SetNotes();
                    RaisePropertyChanged("Appointments");
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
                    vm.RaisedClosed+=new EventHandler(vm_RaisedClosed);
                    editWindow = new Window
                    {
                        Title = "Edit Appointment",
                        Content = vm,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        ResizeMode = ResizeMode.NoResize
                    };

                    editWindow.ShowDialog();
                    var apps = AppointmentsController.List();
                    Appointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d => d.StartTime));
                    SetNotes();
                    RaisePropertyChanged("Appointments");
                },
                (object o) =>
                {
                    return true;
                });
            }
        }

        Window noteWindow;
        public ICommand CreateNote
        {
            get
            {
                return new DelegateCommand((object o) =>
                {
                    var note = new Note{AppointmentId = (int)o};
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

        private ICollectionView _items;
        public ICollectionView Items
        {
            get
            {
               AppointmentsController = new AppointmentController();
                    var apps = AppointmentsController.List(0, OnlyNotFullyPaid, Start, End);
                    Appointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d=>d.StartTime));
                    SetNotes();
                    RaisePropertyChanged("Appointments");
                    _items = System.Windows.Data.CollectionViewSource.GetDefaultView(apps.ToList<Appointment>());
                    _items.GroupDescriptions.Add(new PropertyGroupDescription("StartTime"));

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

        Window window;
        public void createAppointment(object o)
        {
            var vm = new CreateAppointmentUserControl();
            vm.RaisedClosed+=new EventHandler(vm_RaisedClosed);
            window = new Window
            {
                Title = "Create Appointment",
                Content = vm,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            window.ShowDialog();
            Appointments = new ObservableCollection<Appointment>(AppointmentsController.List(0, OnlyNotFullyPaid, Start, End).OrderByDescending(d => d.StartTime));
            SetNotes();
            RaisePropertyChanged("Appointments");
        }

        void vm_RaisedClosed(object sender, EventArgs e)
        {
            if (editWindow!=null)
                editWindow.Close();
            if (window != null)
                window.Close();

            if (noteWindow != null)
                noteWindow.Close();
        }
        #endregion

        #region Commands
        /////commands/////
        

        #endregion






        public AppointmentViewModel()
        {
            var items = Items;
            Start = DateTime.Now.AddMonths(-1);
            End = DateTime.Now.AddMonths(1);
            OnlyNotFullyPaid = false;
            RaisePropertyChanged("Appointments");
            RaisePropertyChanged("Items");
            RaisePropertyChanged("OnlyNotFullyPaid");
            RaisePropertyChanged("Start");
            RaisePropertyChanged("End");
        }

        public event EventHandler RaiseClosed;


        public ICommand Save
        {
            get
            {
                return new DelegateCommand((object o) =>
                {
                    var note = o as Note;

                    NoteController.SaveNote(note);
                    if (RaiseClosed != null)
                        RaiseClosed(null, null);
                }
                );
            }
        }

        private Note SelectedNote { get; set; }
        public ICommand Delete
        {
            get
            {
                return new DelegateCommand((object o) =>
                {
                    if (!ShouldDelete()) return;
                    var note = o as Note;
                    NoteController.Delete(note);
                    _items = Items;
                });
            }
        }


        

    }
}

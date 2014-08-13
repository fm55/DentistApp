using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.BL;
using DentistApp.DAL;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Windows;
using DentistApp.UI.UserControls;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using DentistApp.UI.Modules;
using DentistApp.UI.Events;

namespace DentistApp.UI.ViewModels
{
    public class PatientTeethViewModel : BaseViewModel, IPatientTeethViewModel
    {
        IEventAggregator _eventAggregator;
        public PatientTeethViewModel()
        {
            _eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEventAggregator>();
            _eventAggregator.GetEvent<SelectedPatientItemEvent>().Subscribe((patient) =>
            {
                SelectedPatient = patient;
                RaisePropertyChanged("IsExistingPatient");
            });
        }
        public ICommand Delete
        {
            get
            {
                return new DentistApp.UI.Commands.DelegateCommand((object o) =>
                {
                    if (!ShouldDelete()) return;
                    var nvm = (NoteViewModel)o;
                    Note note = new Note { NoteId = nvm.NoteId };
                    NoteController.Delete(note);
                    var thisNotes = new ObservableCollection<Note>(NoteController.GetNotesForPatientAndTooth(SelectedPatient.PatientId, SelectedTooth.ToothId));
                    Notes = new ObservableCollection<NoteViewModel>(thisNotes.Select(i => new NoteViewModel(i)));
                    Update();
                });
            }
        }
        public ICommand Save
        {
            get
            {
                return new DentistApp.UI.Commands.DelegateCommand((object o) =>
                {
                    var nvm = (NoteViewModel)o;
                    Note note = new Note { NoteId = nvm.NoteId, Description = nvm.Description, PatientId= SelectedPatient.PatientId, ToothId = SelectedTooth.ToothId };
                    if (!Validate(note)) return;
                    NoteController.SaveNote(note);
                    var thisNotes = new ObservableCollection<Note>(NoteController.GetNotesForPatientAndTooth(SelectedPatient.PatientId, SelectedTooth.ToothId));
                    Notes = new ObservableCollection<NoteViewModel>(thisNotes.Select(i => new NoteViewModel(i)));
                    Update();
                }
                );
            }
        }
        IUnityContainer _container { get; set; }
        public NoteController NoteController = new NoteController();
        public AppointmentController AppointmentsController = new AppointmentController();
        public ToothController ToothController = new ToothController();
        public Tooth SelectedTooth { get; set; }
        public ObservableCollection<NoteViewModel> Notes { get; set; }
        public ObservableCollection<Appointment> Appointments { get; set; }
        public Patient SelectedPatient { get; set; }

        public ObservableCollection<Model.Operation> Operations { get; set; }
        private bool Validate(Note note)
        {
            if (string.IsNullOrWhiteSpace(note.Description))
            {
                MessageBox.Show("Please enter a description for the note");
                return false;
            }
            return true;
        }

        public ICommand AddNote
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SelectedTooth.EntityState = EntityState.Unchanged;
                    SelectedPatient.EntityState = EntityState.Unchanged;

                    NoteController.SaveNote(new Note { Description = "Click to edit", ToothId = SelectedTooth.ToothId, PatientId = SelectedPatient.PatientId });
                    var thisNotes = new ObservableCollection<Note>(NoteController.GetNotesForPatientAndTooth(SelectedPatient.PatientId, SelectedTooth.ToothId));
                    Notes = new ObservableCollection<NoteViewModel>(thisNotes.Select(i => new NoteViewModel(i)));
                    Update();
                });
            }
        }

        public ICommand CreateAppointment
        {
            get
            {
                return new DelegateCommand(createAppointment);
            }

        }
        Window window;
        public void createAppointment()
        {
            var vm = new CreateAppointmentUserControl(SelectedPatient, SelectedTooth);
            vm.RaisedClosed += new EventHandler(vm_RaisedClosed);
            window = new Window
            {
                Title = "Create Appointment",
                Content = vm,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            window.ShowDialog();

        }

        void vm_RaisedClosed(object sender, EventArgs e)
        {
            if (window!=null)
                window.Close();

            if (editWindow != null)
                editWindow.Close();

            if (noteWindow != null)
                noteWindow.Close();

            var apps = AppointmentsController.GetAppointmentsOfPatientAndTooth(SelectedPatient.PatientId, SelectedTooth.ToothId);
            Appointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d => d.StartTime));
            SetNotes();
            RaisePropertyChanged("Appointments");
        }

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
                    var apps = AppointmentsController.GetAppointmentsOfPatientAndTooth(SelectedPatient.PatientId, SelectedTooth.ToothId);
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
                return new DentistApp.UI.Commands.DelegateCommand((object o) =>
                {

                    var vm = new CreateAppointmentUserControl(SelectedPatient, SelectedTooth, o as Appointment);
                    vm.RaisedClosed += new EventHandler(vm_RaisedClosed);
                    editWindow = new Window
                    {
                        Title = "Edit Appointment",
                        Content = vm,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        ResizeMode = ResizeMode.NoResize
                    };

                    editWindow.ShowDialog();
                    var apps = AppointmentsController.GetAppointmentsOfPatientAndTooth(SelectedPatient.PatientId, SelectedTooth.ToothId);
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


        public void Update()
        {
            RaisePropertyChanged("Notes");
            RaisePropertyChanged("Appointments");
            RaisePropertyChanged("Operations");
        }

        Window noteWindow;
        public ICommand CreateNoteForTooth
        {
            get
            {
                return new DentistApp.UI.Commands.DelegateCommand((object o) =>
                {
                    var note = new Note { PatientId = SelectedPatient.PatientId, ToothId = SelectedTooth.ToothId };
                    
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
                    Notes = new ObservableCollection<NoteViewModel>(ToothController.GetNotesOfToothAndPatient(SelectedTooth.ToothId, SelectedPatient.PatientId).Select(i => new NoteViewModel(i)));
                    RaisePropertyChanged("PatientNotes");
                    Update();

                },
                (object o) =>
                {
                    return true;
                });
            }
        }


        public ICommand CreateNote
        {
            get
            {
                return new DentistApp.UI.Commands.DelegateCommand((object o) =>
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
                    var apps = AppointmentsController.GetAppointmentsOfPatientAndTooth(SelectedPatient.PatientId, SelectedTooth.ToothId);
                    Appointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d => d.StartTime));
                    SetNotes();
                    Update();

                },
                (object o) =>
                {
                    return true;
                });
            }
        }
    }
}

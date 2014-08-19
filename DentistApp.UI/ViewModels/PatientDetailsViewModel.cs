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
using System.Threading;
using System.Windows.Threading;
using DentistApp.UI.UserControls;
using Microsoft.Practices.Prism.Events;
using DentistApp.UI.Events;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using DentistApp.UI.Requests;
using Microsoft.Practices.ServiceLocation;

namespace DentistApp.UI.ViewModels
{
    public class PatientDetailsViewModel : BaseViewModel, IPatientDetailsViewModel
    {
        public PatientDetailsViewModel(IEventAggregator eventAggregator)
        {
            PatientController = new PatientController();
            SelectedPatient = new Patient();
            ProgressVisibility = "Collapsed";
            RaisePropertyChanged("ProgressVisibility");
            _eventAggregator = eventAggregator;
            SubscribeToEvents();
            IsExistingPatient = "Hidden";
            RaisePropertyChanged("IsExistingPatient");
            _eventAggregator.GetEvent<ReloadPatientsEvent>().Publish(true);
            this.NotificationToPatientDelete = new GenericInteractionRequest<Patient>();
            NoteController = new NoteController();
            AppointmentController = new AppointmentController();
            Start = DateTime.Now.AddMonths(-1);
            End = DateTime.Now.AddMonths(1);
            OnlyNotFullyPaid = false;
            RaisePropertyChanged("Appointments");
            RaisePropertyChanged("Items");
            RaisePropertyChanged("OnlyNotFullyPaid");
            RaisePropertyChanged("Start");
            RaisePropertyChanged("End");
            Reset();
            eventAggregator.GetEvent<RefreshAppointmentsEvent>().Subscribe(UpdateAppointments);
        }

        public void UpdateAppointments(bool value)
        {
            var apps = AppointmentController.GetAppointmentsOfPatient(SelectedPatient.PatientId);
            PatientAppointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d => d.StartTime));
            SetNotes();
            RaisePropertyChanged("PatientAppointments");
        }

        #region Properties
        public GenericInteractionRequest<Patient> NotificationToPatientDelete { get; private set; }
        public string IsExistingPatient { get; set; }
        private IEventAggregator _eventAggregator;
        public ObservableCollection<Appointment> PatientAppointments { get; set; }
        public ObservableCollection<Note> PatientNotes { get; set; }
        public ObservableCollection<Operation> PatientOperations { get; set; }
        public ObservableCollection<Tooth> PatientTeeth { get; set; }
        public double AmountToPay { get; set; }
        public double AmountPaid { get; set; }
        PatientController PatientController { get; set; }
        AppointmentController AppointmentController { get; set; }
        NoteController NoteController { get; set; }
        public string SearchText { get; set; }
        private Patient _selectedPatient;
        public bool OnlyNotFullyPaid { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public Patient SelectedPatient
        {

            get
            {
                return _selectedPatient;
            }
            set
            {
                if (value == _selectedPatient)
                    return;

                _selectedPatient = value;
                PopulatePatientDetails(value);

                RaisePropertyChanged("SelectedPatient");
            }
        }

        private void GetAmountDetails(Patient patient)
        {
            if (patient == null || patient.PatientId == 0) return;
            AmountToPay = PatientController.GetTotalAmountToPay(patient.PatientId);
            AmountPaid = PatientController.GetTotalAmountPaid(patient.PatientId);
            RaisePropertyChanged("AmountToPay");
            RaisePropertyChanged("AmountPaid");
        }

        private Patient _selectedPatientFullObject;
        public Patient SelectedPatientFullObject
        {

            get { return _selectedPatientFullObject; }
            set
            {
                if (value == _selectedPatientFullObject)
                    return;

                _selectedPatientFullObject = value;
                RaisePropertyChanged("SelectedPatientFullObject");
            }
        }

        private int progressValue;
        public int ProgressValue { get { return progressValue; } set { progressValue = value; RaisePropertyChanged("ProgressValue"); } }

        private string progressVisibility;
        public string ProgressVisibility { get { return progressVisibility; } set { progressVisibility = value; RaisePropertyChanged("ProgressVisibility"); } }

        

        #endregion

        #region Commands
        /////commands/////
        
        public ICommand AddPatient
        {
            get
            {
                return new DelegateCommand(addPatient, canCallSearchPatients);
            }
        }

        public ICommand AddNoteForPatient
        {
            get { return new DelegateCommand(addNoteForPatient, canCallSearchPatients); }
        }

        public ICommand DeletePatient
        {
            get
            {
                return new Microsoft.Practices.Prism.Commands.DelegateCommand(deletePatient, () => { return true; });
            }
        }

        public ICommand SavePatient
        {
            get
            {
                return new DelegateCommand(savePatient, canCallSearchPatients);
            }
        }

        private void addPatient(object context)
        {
            SelectedPatient = new Patient();
            IsExistingPatient = "Hidden";
            RaisePropertyChanged("IsExistingPatient");
            RaisePropertyChanged("SelectedPatient");
            Reset();
        }

        Window noteWindow;
        public void addNoteForPatient(object context)
        {
            SelectedPatient.EntityState = EntityState.Modified;
            var vm = new CreateNoteUserControl(new NoteViewModel(new Note { PatientId = SelectedPatient.PatientId }));
            vm.RaiseClosed += new EventHandler(viewModel_RaisedClosed);
            noteWindow = new Window
            {
                Title = "New Note",
                Content = vm,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            noteWindow.ShowDialog();

            SelectedPatient = PatientController.Get(SelectedPatient.PatientId);
            var thisNotes = new ObservableCollection<Note>(NoteController.GetNotesForPatient(SelectedPatient.PatientId));


            PatientNotes = thisNotes;
            RaisePropertyChanged("PatientNotes");
        }

        void nuc_RaiseClosed(object sender, EventArgs e)
        {
            noteWindow.Close();
        }
        
        private void deletePatient()
        {
            if (!ShouldDelete()) return;
            Patient p = SelectedPatient;
            //this.NotificationToPatientDelete.Raised += new EventHandler<GenericInteractionRequestEventArgs<Patient>>(NotificationToPatient_Raised);
            //this.NotificationToPatientDelete.Raise(p, this.DeletePatientCallback, () => { });
            //this is called when the button is clicked
            PatientController.Delete(SelectedPatient);
            var patients = PatientController.List(SearchText, SearchText);
            //Patients = new ObservableCollection<Patient>(patients);
            _eventAggregator.GetEvent<ReloadPatientsEvent>().Publish(true);
            RaisePropertyChanged("Patients");
            Reset();
        }

        void NotificationToPatient_Raised(object sender, GenericInteractionRequestEventArgs<Patient> e)
        {
            
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
            var viewModel = new CreateAppointmentUserControl(SelectedPatient);
            viewModel.RaisedClosed += new EventHandler(viewModel_RaisedClosed);
            window = new Window
            {
                Title = "Create Appointment",
                Content = viewModel,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };
            
            window.ShowDialog();
        }

        Window editWindow;
        public ICommand EditAppointment
        {
            get
            {
                return new DelegateCommand((object o) =>
                {

                    var vm = new CreateAppointmentUserControl(SelectedPatient, o as Appointment);
                    vm.RaisedClosed += new EventHandler(viewModel_RaisedClosed);
                    editWindow = new Window
                    {
                        Title = "Edit Appointment",
                        Content = vm,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        ResizeMode = ResizeMode.NoResize
                    };

                    editWindow.ShowDialog();
                    var apps = AppointmentController.GetAppointmentsOfPatient(SelectedPatient.PatientId);
                    PatientAppointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d => d.StartTime));
                    SetNotes();
                    RaisePropertyChanged("PatientAppointments");
                },
                (object o) =>
                {
                    return true;
                });
            }
        }

        private void SetNotes()
        {
            foreach (var appointment in PatientAppointments)
            {
                appointment.Notes = NoteController.GetNotesForAppointment(appointment.AppointmentId);
            }
        }

        void viewModel_RaisedClosed(object sender, EventArgs e)
        {
            if (window!=null)
                window.Close();
            if (editWindow != null)
                editWindow.Close();
             
            var apps = AppointmentController.GetAppointmentsOfPatient(SelectedPatient.PatientId);
            PatientAppointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d => d.StartTime));
            SetNotes();
            RaisePropertyChanged("PatientAppointments");
        }

        private void DeletePatientCallback(Patient p)
        {

        }
        private void savePatient(object context)
        {
            if (!Validate()) return;
            PatientController.Save(SelectedPatient);
            //this is called when the button is clicked
            var patients = PatientController.List(SearchText);
            //Patients = new ObservableCollection<Patient>(patients);
            _eventAggregator.GetEvent<ReloadPatientsEvent>().Publish(true);
            RaisePropertyChanged("Patients");
            Reset();
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(SelectedPatient.FirstName) || string.IsNullOrWhiteSpace(SelectedPatient.LastName))
            {
                MessageBox.Show("First and last name cannot be empty");
                return false;
            }

            return true;
        }

        private bool canCallSearchPatients(object context)
        {
            return true;
        }

        #endregion

        public ICommand DeleteAppointment
        {
            get
            {
                return new DentistApp.UI.Commands.DelegateCommand((object o) =>
                {
                    if (!ShouldDelete()) return;
                    AppointmentController.Delete(AppointmentController.List((int)o).First());
                    PopulatePatientDetails(SelectedPatient);
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
                    AppointmentController.SaveAppointment(o as Appointment);
                    PopulatePatientDetails(SelectedPatient);
                },
                (object o) =>
                {
                    return true;
                });
            }
        }

        private void PopulatePatientDetails(Patient patient)
        {
            if (patient==null || patient.PatientId == 0) return;
            var patientId = patient.PatientId;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync(patientId);
            
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            AppointmentController ac = new BL.AppointmentController();
            NoteController nc = new BL.NoteController();
            var patientId = (int)e.Argument;
            ProgressVisibility = "Show";
            ProgressValue = 60;
            RaisePropertyChanged("ProgressVisibility");
            RaisePropertyChanged("ProgressValue");

            SelectedPatientFullObject = PatientController.Get(patientId);

            PatientAppointments = new ObservableCollection<Appointment>(ac.GetAppointmentsOfPatientDuringTime(patientId, OnlyNotFullyPaid, Start, End).OrderByDescending(d => d.StartTime));
            SetNotes();
            var thisNotes = new ObservableCollection<Note>(nc.GetNotesForPatient(patientId));
            var notes = thisNotes.Select(note => new NoteViewModel(note)).ToList();
            PatientNotes = new ObservableCollection<Note>(thisNotes);

            var ops = new List<Operation>();

            PatientOperations = new ObservableCollection<Operation>(PatientController.GetOperationsOfPatient(patientId).OrderByDescending(o => o.DateCreated));


            ProgressValue = 100;
            RaisePropertyChanged("ProgressValue");
            ProgressVisibility = "Hidden";
            RaisePropertyChanged("ProgressVisibility");
            


            RaisePropertyChanged("PatientAppointments");
            RaisePropertyChanged("PatientNotes");
            RaisePropertyChanged("PatientOperations");


            GetAmountDetails(SelectedPatientFullObject);
        }

        

        public ICommand SearchAppointments
        {
            get
            {
                return new DelegateCommand((object o) =>
                {
                    RefreshAppointments();
                },
                (object o) =>
                {
                    return true;
                });
            }
        }

        private void RefreshAppointments()
        {
            AppointmentController ac = new BL.AppointmentController();
            PatientAppointments = new ObservableCollection<Appointment>(ac.GetAppointmentsOfPatientDuringTime(SelectedPatient.PatientId, OnlyNotFullyPaid, Start, End).OrderByDescending(d => d.StartTime));
            PatientAppointments = new ObservableCollection<Appointment>(PatientAppointments.OrderByDescending(d => d.StartTime).OrderByDescending(d => d.StartTime));
            SetNotes();
            RaisePropertyChanged("PatientAppointments");
        }
        public int SelectedTabNumber { get; set; }
        private void SubscribeToEvents()
        {
            _eventAggregator.GetEvent<SelectedPatientItemEvent>().Subscribe((patient) =>
            {
                SelectedPatient = patient;
                IsExistingPatient = "Visible";
                RaisePropertyChanged("IsExistingPatient");
                SelectedTabNumber = 0;
                RaisePropertyChanged("SelectedTabNumber");
            });
        }


        private void Reset()
        {
            SelectedPatient = new Patient();
        }



        public event EventHandler RaiseClosed;

        private bool Validate(Note note)
        {
            if (string.IsNullOrWhiteSpace(note.Description))
            {
                MessageBox.Show("Please enter a description for the note");
                return false;
            }
            return true;
        }
        public ICommand Save
        {
            get
            {
                return new DelegateCommand((object o) =>
                {
                    var note = o as Note;
                    if (!Validate(note)) return;
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
                    var thisNotes = new ObservableCollection<Note>(NoteController.GetNotesForPatient(SelectedPatient.PatientId));
                    PatientNotes = thisNotes;
                    RaisePropertyChanged("PatientNotes");
                });
            }
        }

         public ICommand CreateNoteForPatient
        {
            get
            {
                return new DelegateCommand((object o) =>
                {
                    var note = new Note { PatientId = SelectedPatient.PatientId };
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
                    PatientNotes = new ObservableCollection<Note>(NoteController.GetNotesForPatient(SelectedPatient.PatientId));
                    RaisePropertyChanged("PatientNotes");

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
                     RefreshAppointments();

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
    }
}

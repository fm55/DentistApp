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
            IsExistingPatient = false;
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
        }

        #region Properties
        public GenericInteractionRequest<Patient> NotificationToPatientDelete { get; private set; }
        public Boolean IsExistingPatient { get; set; }
        private IEventAggregator _eventAggregator;
        public ObservableCollection<Appointment> PatientAppointments { get; set; }
        public ObservableCollection<NoteViewModel> PatientNotes { get; set; }
        public ObservableCollection<Operation> PatientOperations { get; set; }
        public ObservableCollection<Tooth> PatientTeeth { get; set; }
        public double AmountToPay { get; set; }
        public double AmountPaid { get; set; }
        PatientController PatientController { get; set; }
        AppointmentController AppointmentController { get; set; }
        NoteController NoteController { get; set; }
        public string SearchText { get; set; }
        ObservableCollection<Appointment> Appointments { get; set; }
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
            AmountToPay = PatientController.TotalAmountToPay(patient.PatientId);
            AmountPaid = PatientController.TotalAmountPaid(patient.PatientId);
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
            IsExistingPatient = false;
            RaisePropertyChanged("IsExistingPatient");
            RaisePropertyChanged("SelectedPatient");
            Reset();
        }

        public void addNoteForPatient(object context)
        {
            SelectedPatient.EntityState = EntityState.Modified;
            Note n = new Note()
            {
                Description = "Double click to edit",
                EntityState = EntityState.Added,
                PatientId = SelectedPatient.PatientId
            };
            NoteController.SaveNote(n);
            SelectedPatient = PatientController.Get(SelectedPatient.PatientId);
            var thisNotes = new ObservableCollection<Note>(PatientController.GetPatientNotes(SelectedPatient.PatientId));


            PatientNotes = new ObservableCollection<NoteViewModel>(thisNotes.Select(note => new NoteViewModel(note)).ToList());
            RaisePropertyChanged("PatientNotes");
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

        public void createAppointment(object o)
        {

            Window window = new Window
            {
                Title = "Create Appointment",
                Content = new CreateAppointmentUserControl(SelectedPatient),
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            window.ShowDialog();
        }

        private void DeletePatientCallback(Patient p)
        {

        }
        private void savePatient(object context)
        {
            PatientController.Save(SelectedPatient);
            //this is called when the button is clicked
            var patients = PatientController.List(SearchText);
            //Patients = new ObservableCollection<Patient>(patients);
            _eventAggregator.GetEvent<ReloadPatientsEvent>().Publish(true);
            RaisePropertyChanged("Patients");
            Reset();
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
            var patientId = (int)e.Argument;
            ProgressVisibility = "Show";
            ProgressValue = 60;
            RaisePropertyChanged("ProgressVisibility");
            RaisePropertyChanged("ProgressValue");

            SelectedPatientFullObject = PatientController.Get(patientId);
            PatientAppointments = new ObservableCollection<Appointment>(PatientController.GetPatientAppointments(patientId, OnlyNotFullyPaid, Start, End));

            var thisNotes = new ObservableCollection<Note>(PatientController.GetPatientNotes(patientId));
            var notes = thisNotes.Select(note => new NoteViewModel(note)).ToList();
            PatientNotes = new ObservableCollection<NoteViewModel>(notes);

            var ops = new List<Operation>();

            List<List<OperationAppointment>> operations = SelectedPatientFullObject.Appointments.Select(d => d.Operation).ToList();
            foreach (List<OperationAppointment> o in operations)
            {
                ops.AddRange(o.Select(d => d.Operation));
            }
            PatientOperations = new ObservableCollection<Operation>(PatientController.GetPatientOperations(patientId));


            ProgressValue = 100;
            RaisePropertyChanged("ProgressValue");
            ProgressVisibility = "Hidden";
            RaisePropertyChanged("ProgressVisibility");
            


            //PatientTeeth = new ObservableCollection<Tooth>(fullPatient.Teeth);
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
                    PatientAppointments = new ObservableCollection<Appointment>(PatientController.GetPatientAppointments(SelectedPatient.PatientId, OnlyNotFullyPaid, Start, End));
                    Appointments = new ObservableCollection<Appointment>(PatientAppointments.OrderByDescending(d => d.StartTime));
                    RaisePropertyChanged("PatientAppointments");
                },
                (object o) =>
                {
                    return true;
                });
            }
        }

        private void SubscribeToEvents()
        {
            _eventAggregator.GetEvent<SelectedPatientItemEvent>().Subscribe((patient) =>
            {
                SelectedPatient = patient;
                IsExistingPatient = true;
                RaisePropertyChanged("IsExistingPatient");
            });
        }


        private void Reset()
        {
            SelectedPatient = new Patient();
        }
    }
}

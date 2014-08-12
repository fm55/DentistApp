using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DentistApp.DAL;
using DentistApp.BL;
using System.Collections.ObjectModel;
using DentistApp.UI.ViewModels;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using DentistApp.Model;
using DentistApp.UI.Modules;
using Microsoft.Practices.Prism.Events;
using DentistApp.UI.Events;

namespace DentistApp.UI.UserControls
{
    
    /// <summary>
    /// Interaction logic for PatientTeethUserControl.xaml
    /// </summary>
    public partial class PatientTeethUserControl : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<NoteViewModel> Notes { get; set; }
        NoteController NoteController = new NoteController();
        ObservableCollection<Appointment> _Appointments;
        public ObservableCollection<Appointment> Appointments
        {
            get { return _Appointments; }
            set
            {
                _Appointments = value;
                foreach (var a in Appointments)
                {
                    a.Notes = NoteController.GetNotesForAppointment(a.AppointmentId);
                }
            }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        IEventAggregator _eventAggregator { get; set; }
        
        public PatientTeethUserControl()
        {
            InitializeComponent();
            ToothDetailsTab.Visibility = System.Windows.Visibility.Collapsed;
            RaisePropertyChanged("ShowAppointmentsAndNotes");
            Context = new PatientTeethViewModel();
            _eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEventAggregator>();
            _eventAggregator.GetEvent<SelectedPatientItemEvent>().Subscribe((patient) =>
            {
                
                SelectedPatient = patient;
                RaisePropertyChanged("SelectedPatient");
                Notes = new ObservableCollection<NoteViewModel>(new List<NoteViewModel>());
                Appointments = new ObservableCollection<Appointment>(new List<Appointment>());
                Context.SelectedPatient = patient;
                Context.Notes = Notes;
                Context.Appointments = Appointments;
                Context.Operations = new ObservableCollection<Operation>(new List<Operation>());
                Context.SelectedTooth = null;
                RevertAllTeethColour();
                ColourPatientTeeth(patient);


                RaisePropertyChanged("Notes");
                RaisePropertyChanged("Appointments");
                RaisePropertyChanged("Operations");

                this.DataContext = Context;
                Context.Update();
            });
            

        }

        private void RevertAllTeethColour()
        {
            Border b = new Border();
            b.Background = Brushes.Transparent;
            var contentControls = TeethScreen.Children;
            foreach (var contentControl in contentControls)
            {
                if (contentControl is ContentControl)
                    ((ContentControl)contentControl).Content = b;
            }
        }

        private void RevertAllTeethBorder()
        {
            var contentControls = TeethScreen.Children;
            foreach (var contentControl in contentControls)
            {
                if (contentControl is ContentControl)
                {
                    var content = ((ContentControl)contentControl).Content;
                    if (content is Border)
                        ((Border)content).BorderBrush = Brushes.Transparent;
                }
            }
        }

        private void ColourPatientTeeth(Patient patient)
        {
            PatientController PatientController = new PatientController();
            AppointmentController appointmentController = new AppointmentController();
            var patientAppointments = appointmentController.GetAppointmentsOfPatient(patient.PatientId);
            var getAllTeethFromAppointments = patientAppointments.Select(a => appointmentController.GetTeethOfAppointment(a.AppointmentId)).SelectMany(i=>i).Distinct();
            
            //colour code the teeth
            foreach (var tooth in getAllTeethFromAppointments)
            {
                AddBackgroundOfTooth(tooth);
                
            }
        }

        private void AddBackgroundOfTooth(Tooth tooth)
        {
            Border b = new Border();
            b.Background = Brushes.Red;
            b.Opacity = 0.1;

            //get content control
            var contentControl = (ContentControl)FindName("Tooth" + tooth.ToothId.ToString());
            if (contentControl!=null)
                contentControl.Content = b;
        }

        public PatientTeethViewModel Context { get; set; }
        
        public static DependencyProperty SelectedPatientProperty = DependencyProperty.Register(
            "SelectedPatient", typeof(Patient), typeof(PatientTeethUserControl));

        public Tooth SelectedTooth { get; set; }

        public Patient SelectedPatient
        {
            get
            {
                return (Patient)GetValue(SelectedPatientProperty);
            }
            set
            {
                SetValue(SelectedPatientProperty, value);
                RaisePropertyChanged("SelectedPatient");
            }

        }

        
        private void toothGotFocus(object sender, MouseButtonEventArgs e)
        {
            PatientController PatientController = new PatientController();
            AppointmentController appointmentController = new AppointmentController();
            ToothController toothController = new ToothController();
            var element = sender as ContentControl;
            



            var toothId = Convert.ToInt32(element.ToolTip.ToString());
            if (SelectedPatient == null) { MessageBox.Show("Select a patient please." + Context.SelectedPatient.PatientId); return; }
            var fullPatient = PatientController.Get(SelectedPatient.PatientId);
            ToothDetailsTab.Visibility = System.Windows.Visibility.Visible;
            ColourPatientTeeth(fullPatient);
            RevertAllTeethBorder();
            CreateBorderOnSelectedTooth(element);
            if (fullPatient != null)
            {
                Context.SelectedPatient = fullPatient;
            }
            fullPatient =  Context.SelectedPatient; 


            //get appoitnments for patient on the tooth
            //get operations for patient on the tooth
            //get notes for patient on the tooth
            var SelectedToothAppointments = appointmentController.GetAppointmentsOfPatientAndTooth(SelectedPatient.PatientId, toothId).OrderByDescending(i=>i.StartTime);
            var Operations = PatientController.GetOperationsOfPatientAndTooth(SelectedPatient.PatientId, toothId).OrderByDescending(o => o.DateCreated); 
            Notes = new ObservableCollection<NoteViewModel>(toothController.GetNotesOfToothAndPatient(toothId, SelectedPatient.PatientId).ToList().Select(t=>new NoteViewModel(t)));
            Appointments = new ObservableCollection<Appointment>(SelectedToothAppointments.ToList());
            RaisePropertyChanged("Notes");
            RaisePropertyChanged("Appointments");
            RaisePropertyChanged("Operations");
            Context.Notes = Notes;
            Context.Appointments = new ObservableCollection<Appointment>(Appointments);
            Context.Operations = new ObservableCollection<Operation>(Operations.ToList());
            Context.SelectedTooth = toothController.List(toothId).FirstOrDefault();


            this.DataContext = Context;
            Context.Update();
        }

        private static void CreateBorderOnSelectedTooth(ContentControl element)
        {
            var thickNess = new Thickness(1.0);
            element.Content = null;
            var border = new Border();
            border.BorderBrush = Brushes.Blue;
            border.BorderThickness = thickNess;
            element.Content = border;
        }
    }
}

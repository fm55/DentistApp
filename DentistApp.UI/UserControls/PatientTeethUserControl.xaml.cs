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

namespace DentistApp.UI.UserControls
{
    
    /// <summary>
    /// Interaction logic for PatientTeethUserControl.xaml
    /// </summary>
    public partial class PatientTeethUserControl : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<NoteViewModel> Notes { get; set; }
        public ObservableCollection<Appointment> Appointments { get; set; }
        public void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public PatientTeethUserControl()
        {
            InitializeComponent();

            Context = new PatientTeethViewModel();
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
            var fullPatient = PatientController.Get(SelectedPatient.PatientId);

            
            if (fullPatient != null)
            {
                Context.SelectedPatient = fullPatient;
            }
            fullPatient =  Context.SelectedPatient; 


            //get appoitnments for patient on the tooth
            //get operations for patient on the tooth
            //get notes for patient on the tooth
            var SelectedToothAppointments = PatientController.GetPatientAppointments(SelectedPatient.PatientId, toothId);
            var Operations = PatientController.GetPatientOperations(SelectedPatient.PatientId, toothId); 
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
    }
}

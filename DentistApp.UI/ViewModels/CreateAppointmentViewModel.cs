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


namespace DentistApp.UI.ViewModels
{
    public class SelectableTooth : BaseViewModel
    {
        public Tooth Tooth { get; set; }
        private bool? isSelected;
        public bool? IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
    }

    public class SelectableOperation : BaseViewModel
    {
        public Operation Operation { get; set; }
        private bool? isSelected;
        public bool? IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
    }


    public class CreateAppointmentViewModel : BaseViewModel
    {
        private Patient SelectedPatient_2;
        private Tooth SelectedTooth;

        #region Properties
        /////properties////
        //list of appointments
        public ObservableCollection<SelectableOperation> Operations { get; set; }
        public ObservableCollection<Operation> SelectedOperation { get; set; }
        public ObservableCollection<SelectableTooth> Teeth { get; set; }
        public ObservableCollection<Tooth> SelectetedTeeth { get; set; }
        public Appointment NewAppointment { get; set; }
        public ObservableCollection<Note> Notes { get; set; }
        public AppointmentController AppointmentController { get; set; }
        public ToothController ToothController { get; set; }
        public OperationController OperationController { get; set; }
        public Patient SelectedPatient { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
        public PatientController PatientController { get; set; }
        #endregion

        #region Commands
        /////commands/////
        private ICommand createAppointment {get;set;}
        public DelegateCommand CreateAppointment
        {
            get
            {
                return new DelegateCommand((object o) => {
                    AppointmentController.SaveAppointment(NewAppointment, SelectedPatient, Operations.Where(p => p.IsSelected == true).Select(t => t.Operation).ToList(), Teeth.Where(t => t.IsSelected == true).Select(to => to.Tooth).ToList(), Notes.ToList()); 
                }, (object b) => { return true; });
            }
        }
        #endregion




        public CreateAppointmentViewModel()
        {
            ToothController = new BL.ToothController();
            OperationController = new BL.OperationController();
            AppointmentController = new BL.AppointmentController();
            PatientController = new BL.PatientController();
            NewAppointment = new Appointment();
            var teeth = ToothController.List();
            var selectableTeeth = teeth.Select(t => new SelectableTooth() { Tooth = t, IsSelected = false });
            var operations = OperationController.List();
            var selectableoperations = operations.Select(t => new SelectableOperation() { Operation = t, IsSelected = false });
            Teeth = new ObservableCollection<SelectableTooth>(selectableTeeth);
            Operations = new ObservableCollection<SelectableOperation>(selectableoperations);
           
            Patients = new ObservableCollection<Patient>(PatientController.List());
            Notes = new ObservableCollection<Note>();
            RaisePropertyChanged("NewAppointment");
            RaisePropertyChanged("Teeth");
            RaisePropertyChanged("Operations");
            RaisePropertyChanged("Patients");
        }

        public CreateAppointmentViewModel(Patient SelectedPatient)
        {
            HidePatient = "Collapsed";
            ToothController = new BL.ToothController();
            OperationController = new BL.OperationController();
            AppointmentController = new BL.AppointmentController();
            PatientController = new BL.PatientController();
            this.SelectedPatient = SelectedPatient;
            NewAppointment = new Appointment();
            var teeth = ToothController.List();
            var selectableTeeth = teeth.Select(t => new SelectableTooth() { Tooth = t, IsSelected = false });
            var operations = OperationController.List();
            var selectableoperations = operations.Select(t => new SelectableOperation() { Operation = t, IsSelected = false });
            Teeth = new ObservableCollection<SelectableTooth>(selectableTeeth);
            Operations = new ObservableCollection<SelectableOperation>(selectableoperations);

            Patients = new ObservableCollection<Patient>(PatientController.List());
            Notes = new ObservableCollection<Note>();
            RaisePropertyChanged("NewAppointment");
            RaisePropertyChanged("Teeth");
            RaisePropertyChanged("Operations");
            RaisePropertyChanged("Patients");
        }

        public CreateAppointmentViewModel(Patient SelectedPatient, Tooth SelectedTooth)
        {
            // TODO: Complete member initialization
            HideTeeth = "Hidden";
            HidePatient = "Collapsed";
            this.SelectedPatient = SelectedPatient;
            this.SelectedTooth = SelectedTooth;
            ToothController = new BL.ToothController();
            OperationController = new BL.OperationController();
            AppointmentController = new BL.AppointmentController();
            PatientController = new BL.PatientController();
            NewAppointment = new Appointment();

            
            var teeth = ToothController.List();
            var selectableTeeth = teeth.Where(t=>t.ToothId!=SelectedTooth.ToothId).Select(t => new SelectableTooth() { Tooth = t, IsSelected = false });
            var allTeeth= selectableTeeth.ToList();
            allTeeth.Add(new SelectableTooth() { Tooth = SelectedTooth, IsSelected = true });
            var operations = OperationController.List();
            var selectableoperations = operations.Select(t => new SelectableOperation() { Operation = t, IsSelected = false });
            Teeth = new ObservableCollection<SelectableTooth>(allTeeth);
            Operations = new ObservableCollection<SelectableOperation>(selectableoperations);

            Patients = new ObservableCollection<Patient>(PatientController.List());
            Notes = new ObservableCollection<Note>();
            RaisePropertyChanged("NewAppointment");
            RaisePropertyChanged("Teeth");
            RaisePropertyChanged("Operations");
            RaisePropertyChanged("Patients");

        }






        public string HidePatient { get; set; }
        public string HideTeeth { get; set; }
    }
}

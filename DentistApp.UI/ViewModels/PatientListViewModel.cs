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

namespace DentistApp.UI.ViewModels
{
    public class PatientListViewModel : BaseViewModel, IPatientListViewModel
    {
        #region Properties
        private IEventAggregator _eventAggregator;
        public ObservableCollection<Patient> Patients { get; set; }
        PatientController PatientController { get; set; }
        public string SearchText { get; set; }
        private Patient _selectedPatient;
       
        public Patient SelectedPatient
        {

            get
            {
                return _selectedPatient;
            }
            set
            {
                if (value == _selectedPatient || value == null)
                    return;

                _selectedPatient = value;
                RaisePropertyChanged("SelectedPatient");
                if (_eventAggregator!=null)
                    _eventAggregator.GetEvent<SelectedPatientItemEvent>().Publish(value);
            }
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
        //select patient

        //delete patient

        //save patient

        //search patients
        public ICommand SearchPatients
        {
            get
            {
                return new DelegateCommand(searchPatients, canCallSearchPatients);
            }
        }

        

        private void searchPatients(object context)
        {
            BackgroundWorker w = new BackgroundWorker();
            w.WorkerReportsProgress = true;
            ProgressVisibility = "Show";
            RaisePropertyChanged("ProgressVisibility");
            ProgressValue = 10;
            RaisePropertyChanged("ProgressValue");
            w.ReportProgress(50);
            w.DoWork += new DoWorkEventHandler(w_DoWork);
            w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(w_RunWorkerCompleted);
            w.RunWorkerAsync();
            
        }

        void w_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RaisePropertyChanged("Patients");
            ProgressVisibility = "Hidden";
            RaisePropertyChanged("ProgressVisibility");
           
        }

        void w_DoWork(object sender, DoWorkEventArgs e)
        {
            //this is called when the button is clicked
            var patients = PatientController.List(SearchText, SearchText);
            ProgressValue = 40;
            RaisePropertyChanged("ProgressValue");
            Patients = new ObservableCollection<Patient>(patients);
            var w = (sender as BackgroundWorker);
            ProgressValue = 80;
            RaisePropertyChanged("ProgressValue");
            w.ReportProgress(100);
        }

        

        private bool canCallSearchPatients(object context)
        {
            return true;
        }

        #endregion



        public PatientListViewModel(IEventAggregator eventAggregator)
        {
            PatientController = new PatientController();
            SelectedPatient = new Patient();
            ProgressVisibility = "Collapsed";
            RaisePropertyChanged("ProgressVisibility");
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SearchedPatientsEvent>().Subscribe((patients) =>
            {
                Patients = new ObservableCollection<Patient>(patients);
                RaisePropertyChanged("Patients");
            });

            _eventAggregator.GetEvent<ReloadPatientsEvent>().Subscribe((reload) =>
            {
                if (reload)
                    searchPatients(null);
            });

        }
       



        

    }
}

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
using Microsoft.Practices.Unity;

namespace DentistApp.UI.ViewModels
{
    public class PatientViewModel : BaseViewModel, IPatientViewModel
    {
        #region Properties
        private IUnityContainer _container;
        private IEventAggregator _eventAggregator;
        PatientController PatientController { get; set; }
        public string SearchText { get; set; }
        private int progressValue;
        public int ProgressValue { get { return progressValue; } set { progressValue = value; RaisePropertyChanged("ProgressValue"); } }
        private string progressVisibility;
        public string ProgressVisibility { get { return progressVisibility; } set { progressVisibility = value; RaisePropertyChanged("ProgressVisibility"); } }     
        #endregion

        #region Commands
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
            var patients = e.Result as List<Patient>;
            _eventAggregator.GetEvent<SearchedPatientsEvent>().Publish(patients);
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
            var w = (sender as BackgroundWorker);
            ProgressValue = 80;
            RaisePropertyChanged("ProgressValue");
            w.ReportProgress(100);
            e.Result = patients;
        }

        

        private bool canCallSearchPatients(object context)
        {
            return true;
        }

        #endregion






        public PatientViewModel(IUnityContainer container, IEventAggregator eventAggregator)
        {
            PatientController = new PatientController();
            ProgressVisibility = "Collapsed";
            RaisePropertyChanged("ProgressVisibility");
            _eventAggregator = eventAggregator;
            _container = container;
        }
       



        

    }
}

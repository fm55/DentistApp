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
using DentistApp.UI.ViewModels;
using DentistApp.DAL;
using DentistApp.BL;

namespace DentistApp.UI.UserControls
{
    /// <summary>
    /// Interaction logic for AppointmentUserControl.xaml
    /// </summary>
    public partial class CreateAppointmentUserControl : UserControl
    {
        private DAL.Patient SelectedPatient;
        private Tooth SelectedTooth;

        public event EventHandler RaisedClosed;
        public Patient Patient { get; set; }

        public CreateAppointmentUserControl(Patient patient)
        {
            InitializeComponent();
            Patient = patient;
            var vm = new CreateAppointmentViewModel(Patient);
            vm.RaiseClosed += new EventHandler(vm_RaiseClosed);
            DataContext = vm;
        }

        public CreateAppointmentUserControl(Patient patient, Appointment Appointment)
        {
            InitializeComponent();
            Patient = patient;
            var vm = new CreateAppointmentViewModel(Patient, Appointment);
            vm.RaiseClosed+=new EventHandler(vm_RaiseClosed);
            DataContext = vm;
        }

        void vm_RaiseClosed(object sender, EventArgs e)
        {
            if (RaisedClosed != null)
                RaisedClosed(this, EventArgs.Empty);
        }

        public CreateAppointmentUserControl()
        {
            InitializeComponent();
            var vm = new CreateAppointmentViewModel();
            vm.RaiseClosed += new EventHandler(vm_RaiseClosed);
            DataContext = vm;
        }

        public CreateAppointmentUserControl(DAL.Patient SelectedPatient, Tooth SelectedTooth)
        {
            // TODO: Complete member initialization
            this.SelectedPatient = SelectedPatient;
            this.SelectedTooth = SelectedTooth;
            InitializeComponent();
            var vm = new CreateAppointmentViewModel(SelectedPatient, SelectedTooth);
            vm.RaiseClosed += new EventHandler(vm_RaiseClosed);
            DataContext = vm;
        }

        public CreateAppointmentUserControl(DAL.Patient SelectedPatient, Tooth SelectedTooth, Appointment Appointment)
        {
            // TODO: Complete member initialization
            this.SelectedPatient = SelectedPatient;
            this.SelectedTooth = SelectedTooth;
            InitializeComponent();
            var vm = new CreateAppointmentViewModel(SelectedPatient, SelectedTooth, Appointment);
            vm.RaiseClosed += new EventHandler(vm_RaiseClosed);
            DataContext = vm;
        }

        public CreateAppointmentUserControl(Appointment appointment)
        {
            InitializeComponent();
            var vm = new CreateAppointmentViewModel(appointment);
            vm.RaiseClosed += new EventHandler(vm_RaiseClosed);
            DataContext = vm;
        }

    }
}

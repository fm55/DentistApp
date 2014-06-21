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

namespace DentistApp.UI.UserControls
{
    /// <summary>
    /// Interaction logic for AppointmentUserControl.xaml
    /// </summary>
    public partial class CreateAppointmentUserControl : UserControl
    {
        private DAL.Patient SelectedPatient;
        private Tooth SelectedTooth;

        public Patient Patient { get; set; }
        public CreateAppointmentUserControl(Patient patient)
        {
            InitializeComponent();
            Patient = patient;
            DataContext = new CreateAppointmentViewModel(Patient);
        }

        public CreateAppointmentUserControl()
        {
            InitializeComponent();
            DataContext = new CreateAppointmentViewModel();
        }

        public CreateAppointmentUserControl(DAL.Patient SelectedPatient, Tooth SelectedTooth)
        {
            // TODO: Complete member initialization
            this.SelectedPatient = SelectedPatient;
            this.SelectedTooth = SelectedTooth;
            InitializeComponent();
            DataContext = new CreateAppointmentViewModel(SelectedPatient, SelectedTooth);
        }

    }
}

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

namespace DentistApp.UI.ViewModels
{
    public class PatientTeethViewModel : BaseViewModel
    {
        public NoteController NoteController = new NoteController();
        public AppointmentController AppointmentsController = new AppointmentController();
        public ToothController ToothController = new ToothController();
        public Tooth SelectedTooth { get; set; }
        public ObservableCollection<NoteViewModel> Notes { get; set; }
        public ObservableCollection<Appointment> Appointments { get; set; }
        public Patient SelectedPatient { get; set; }

        public ObservableCollection<Model.Operation> Operations { get; set; }
        public ICommand AddNote
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SelectedTooth.EntityState = EntityState.Unchanged;
                    SelectedPatient.EntityState = EntityState.Unchanged;
                    NoteController.SaveNote(new Note { Description = "Click to edit", ToothId = SelectedTooth.ToothId, PatientId = SelectedPatient.PatientId });
                    Notes = new ObservableCollection<NoteViewModel>(ToothController.GetNotesOfToothAndPatient(SelectedTooth.ToothId, SelectedPatient.PatientId).Select(i => new NoteViewModel(i)));
                    Update();
                });
            }
        }


        internal void Update()
        {
            RaisePropertyChanged("Notes");
            RaisePropertyChanged("Appointments");
            RaisePropertyChanged("Operations");
        }

        public ICommand CreateAppointment
        {
            get
            {
                return new DelegateCommand(createAppointment);
            }

        }

        public void createAppointment()
        {

            Window window = new Window
            {
                Title = "Create Appointment",
                Content = new CreateAppointmentUserControl(SelectedPatient, SelectedTooth),
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };

            window.ShowDialog();

        }
        public ICommand DeleteAppointment
        {
            get
            {
                return new DentistApp.UI.Commands.DelegateCommand((object o) =>
                {
                    if (ShouldDelete()) return;
                    AppointmentsController.Delete(AppointmentsController.List((int)o).First());
                    var apps = AppointmentsController.List();
                    Appointments = new ObservableCollection<Appointment>(apps.OrderByDescending(d => d.StartTime));
                    RaisePropertyChanged("Appointments");
                },
                (object o) =>
                {
                    return true;
                });
            }
        }
    }
}

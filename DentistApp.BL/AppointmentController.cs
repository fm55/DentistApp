using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.Model;
using DentistApp.DAL.DAL;
using DentistApp.DAL;

namespace DentistApp.BL
{
    public class AppointmentController
    {
        public IGenericDataRepository<Appointment> AppointmentRepository { get; set; }
        public IGenericDataRepository<OperationAppointment> OperationAppointmentRepository { get; set; }
        public IGenericDataRepository<TeethAppointment> TeethAppointmentRepository { get; set; }
        public OperationController OperationController { get; set; }
        public ToothController ToothController { get; set; }

        public AppointmentController()
        {
            AppointmentRepository = new GenericDataRepository<Appointment>();
            OperationAppointmentRepository = new GenericDataRepository<OperationAppointment>();
            TeethAppointmentRepository = new GenericDataRepository<TeethAppointment>();
            OperationController = new OperationController();
            ToothController = new ToothController();
        }

        public AppointmentController(IGenericDataRepository<Appointment> appointmentRepository)
        {
            AppointmentRepository = appointmentRepository;
        }

        public void SaveAppointment(Appointment Appointment)
        {
            if (Appointment.AppointmentId == 0)
            {
                Appointment.EntityState = EntityState.Added;
                AppointmentRepository.Add(Appointment);
            }
            else
            {
                //set state of all entities to unchanged
                Appointment.EntityState = EntityState.Modified;
                AppointmentRepository.Update(Appointment);
            }

        }

        public void SaveAppointment(Appointment Appointment, Patient Patient, List<Operation> Operations, List<Tooth> Teeth, List<Note> Notes)
        {
            Patient.EntityState = EntityState.Unchanged;
            if (Appointment.AppointmentId == 0)
            {
                Appointment.EntityState = EntityState.Added;
                Appointment.PatientId = Patient.PatientId;
                SaveAppointment(Appointment);
                Appointment = List(Appointment.AppointmentId).FirstOrDefault();//set to original
            }
            Appointment.EntityState = EntityState.Modified;
            //delete existing operations and teeth
            if (Appointment.Operation != null)
            {
                OperationAppointmentRepository.Remove(Appointment.Operation.ToArray());
            }

            if (Appointment.Teeth != null)
            {
                TeethAppointmentRepository.Remove(Appointment.Teeth.ToArray());
            }


            foreach (var note in Notes)
            {
                //add via note controller
                note.EntityState = EntityState.Added;
            }

            var operationApps = new List<OperationAppointment>();

            foreach (var o in Operations)
            {
                var opA = new OperationAppointment { OperationId = o.OperationId, Operation = o, AppointmentId = Appointment.AppointmentId };
                OperationAppointmentRepository.Add(opA);
            }

            var teethApps = new List<TeethAppointment>();
            foreach (var o in Teeth)
            {
                var teA = new TeethAppointment { TeethId = o.ToothId, Teeth = o, Appointment = Appointment, AppointmentId = Appointment.AppointmentId };
                TeethAppointmentRepository.Add(teA);
            }
            SaveAppointment(Appointment);

        }

        private EntityState GetStateOfTooth(Tooth o, Appointment Appointment, List<Tooth> Teeth)
        {
            if (Appointment.Teeth == null) return EntityState.Added;
            return EntityState.Deleted;
        }

        private EntityState GetStateOfOperation(Operation o, Appointment Appointment, List<Operation> Operations)
        {
            //if operation is in original and new appointment - then unchanged
            //if operation is in original and not in new appointment - then deleted
            //if operation is not in original and is in new - then added
            if (Appointment.Operation == null) return EntityState.Added;
            if (Appointment.Operation.FirstOrDefault(a => a.OperationId == o.OperationId) == null)//deleted
            {
                return EntityState.Unchanged;
            }

            else if (Appointment.Operation.FirstOrDefault(a => a.OperationId == o.OperationId) != null)//modified
            {
                return EntityState.Unchanged;
            }
            return EntityState.Unchanged;
        }

        public IEnumerable<Appointment> List(int AppointmentId = 0, bool onlyNotFullyPaid = false, DateTime? start = null, DateTime? end = null)
        {
            if (AppointmentId == 0)
            {
                var apps = AppointmentRepository.GetAll();
                if (onlyNotFullyPaid)
                    apps = apps.Where(a => a.AmountPaid < a.AmountToPay).ToList();

                apps = apps.Where((a => ((a.StartTime >= start || start == null) && (a.EndTime <= end || end == null)))).ToList();
                foreach (var a in apps)
                {
                    a.Teeth = TeethAppointmentRepository.GetList(t => t.AppointmentId == a.AppointmentId).ToList();
                    a.Operation = OperationAppointmentRepository.GetList(t => t.AppointmentId == a.AppointmentId).ToList();
                }
                return apps;
            }
            var thisApp = AppointmentRepository.GetList(d => d.AppointmentId == AppointmentId);
            foreach (var a in thisApp)
            {
                a.Teeth = TeethAppointmentRepository.GetList(t=>t.AppointmentId==a.AppointmentId).ToList();
                a.Operation = OperationAppointmentRepository.GetList(t => t.AppointmentId == a.AppointmentId).ToList();
            }
            return thisApp;

        }


        public IEnumerable<Appointment> GetAppointmentsOfPatientDuringTime(int PatientId, bool onlyNotFullyPaid = false, DateTime? start = null, DateTime? end = null)
        {
            var apps = List(0, onlyNotFullyPaid, start, end).Where(a=>a.PatientId ==PatientId);
            return apps;
        }

        public void Delete(Appointment Appointment)
        {

            AppointmentRepository.Remove(Appointment);

        }


        public IEnumerable<Appointment> GetAppointmentsOfPatient(int patientId)
        {
            return List(0, false, DateTime.MinValue, DateTime.MaxValue).Where(a => a.PatientId == patientId);
        }


        public IEnumerable<Appointment> GetAppointmentsOfPatientAndTooth(int patientId, int toothId)
        {
             var apps = GetAppointmentsOfPatient(patientId);//all apps for the patient

            var SelectedToothAppointments = apps.Select(d => d.Teeth.Where(t => t.TeethId == toothId)).SelectMany(i => i).ToList();//teeth apps for the patient
            var appIds = SelectedToothAppointments.Select(i=>i.AppointmentId); //app Ids from the teeth apps above

            var appToReturn = new List<Appointment>();
            foreach (var i in appIds)
            {
                if (appIds.Contains(i))
                {
                    appToReturn.Add(apps.Where(a => a.AppointmentId == i).FirstOrDefault());
                }
            }
            return appToReturn;//filtered apps for the patient and tooth
            
        }

        public IEnumerable<Operation> GetOperationsOfAppointment(int appointmentId)
        {
            var app = AppointmentRepository.GetList(a=>a.AppointmentId == appointmentId).FirstOrDefault();//all apps
            app.Operation = OperationAppointmentRepository.GetList(t => t.AppointmentId == app.AppointmentId).ToList();
            
            var opreationsToReturn = new List<Operation>();
            foreach (var i in app.Operation)
            {
                var operation = OperationController.List(i.OperationId).FirstOrDefault();
                if (operation != null)
                {
                    operation.DateCreated = app.StartTime;//TODO this isn't correct but as the UI needs the date, I am just setting it here
                    opreationsToReturn.Add(operation);
                }
            }
            return opreationsToReturn;//filtered apps for the patient and tooth
        }

       
        public IEnumerable<Appointment> ListTodayAppointments()
        {
            return List(0, false, DateTime.Today.Date, DateTime.Today.AddDays(1).Date);

        }


        public IEnumerable<Tooth> GetTeethOfAppointment(int appointmentId)
        {
            var app = AppointmentRepository.GetList(a => a.AppointmentId == appointmentId).FirstOrDefault();//all apps

            var TeethToReturn = new List<Tooth>();
            app.Teeth = TeethAppointmentRepository.GetList(t => t.AppointmentId == appointmentId).ToList();
            foreach (var i in app.Teeth)
            {
                TeethToReturn.Add(ToothController.List(i.TeethId).FirstOrDefault());
            }
            return TeethToReturn;//filtered apps for the patient and tooth
        }
    }
}

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
        IGenericDataRepository<Appointment> AppointmentRepository { get; set; }
        public PatientController PatientController { get; set; }

        public AppointmentController()
        {
            AppointmentRepository = new GenericDataRepository<Appointment>();
            PatientController = new PatientController();
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
                AppointmentRepository.Update(Appointment);
            }

        }

        public void SaveAppointment(Appointment Appointment, Patient Patient, List<Operation> Operations, List<Tooth> Teeth, List<Note> Notes)
        {
            Patient.EntityState = EntityState.Unchanged;

            foreach (var note in Notes)
            {
                note.EntityState = EntityState.Added;
            }
            
            var operationApps = new List<OperationAppointment>();

            foreach (var o in Operations)
            {
                o.EntityState = EntityState.Unchanged;
                operationApps.Add(new OperationAppointment { Operation = o, Appointment = Appointment, EntityState = EntityState.Added });
            }

            var teethApps = new List<TeethAppointment>();
            foreach (var o in Teeth)
            {
                o.EntityState = EntityState.Unchanged;
                teethApps.Add(new TeethAppointment { Teeth = o, Appointment = Appointment, EntityState = EntityState.Added });
            }

            Appointment.Patient = Patient;
            Appointment.Operation = operationApps;
            Appointment.Teeth = teethApps;
            Appointment.Notes = Notes;
            Appointment.EntityState = EntityState.Added;
            SaveAppointment(Appointment);
        }

        public IEnumerable<Appointment> List(int AppointmentId = 0, bool onlyNotFullyPaid = false, DateTime? start = null, DateTime? end = null)
        {
            if (AppointmentId == 0)
            {
                var apps = AppointmentRepository.GetAll(d => d.Operation, d => d.Teeth, d => d.Patient, d=>d.Operation.Select(e=>e.Operation));
                apps = apps.Where(a => a.AmountPaid < a.AmountToPay || (!onlyNotFullyPaid)).ToList();

                apps = apps.Where((a => ((a.StartTime >= start || start == null) && (a.EndTime <= end || end == null)))).ToList();
                return apps;
            }
            return AppointmentRepository.GetList(d => d.AppointmentId == AppointmentId, d => d.Operation, d => d.Teeth, d => d.Patient, d=>d.Operation.Select(e=>e.Operation));

        }


        public void Delete(Appointment Appointment)
        {

            AppointmentRepository.Remove(Appointment);

        }


        public IEnumerable<Appointment> GetAppointmentsOfPatient(int patientId)
        {
            return PatientController.Get(patientId).Appointments;
        }


        public IEnumerable<Appointment> GetAppointmentsOfPatientAndTooth(int patientId, int toothId)
        {
            var patient = PatientController.Get(patientId);
            var SelectedToothAppointments = patient.Appointments.Select(d => d.Teeth.Where(t => t.Teeth.ToothId == toothId)).SelectMany(i => i).ToList();
            return SelectedToothAppointments.Select(i => i.Appointment).Where(a=>a.IsDeleted==false).ToList();
            
        }

        public IEnumerable<Operation> GetOperationsOfAppointment(int appointmentId)
        {
            var app = List(appointmentId);
            var SelectedOperations = app.Select(a => a.Operation.Select(o => o.Operation)).SelectMany(o => o);
            return SelectedOperations;

        }

        public IEnumerable<Appointment> ListTodayAppointments()
        {
            return AppointmentRepository.GetList(d => d.StartTime.Date==DateTime.Today.Date, d => d.Operation, d => d.Teeth, d => d.Patient, d=>d.Operation.Select(e=>e.Operation));

        }


        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.DAL.DAL;
using DentistApp.Model;
using DentistApp.DAL;

namespace DentistApp.BL
{
    public class PatientController
    {
        IGenericDataRepository<Patient> PatientDAL { get; set; }
        public PatientController()
        {
            PatientDAL = new GenericDataRepository<Patient>();
        }

        public PatientController(IGenericDataRepository<Patient> patientDAL)
        {
            PatientDAL = patientDAL;
        }

        public void Save(Patient patient)
        {
           patient.EntityState = EntityState.Modified;

           if (patient.PatientId == 0)
           {
               patient.EntityState = EntityState.Added;
               PatientDAL.Add(patient);
           }
           else
           {
               PatientDAL.Update(patient);
           }
        }

        public void Delete(Patient patient)
        {
            patient.EntityState = EntityState.Modified;
            PatientDAL.Remove(patient);
        }
        

        public List<Patient> List(string firstName="", string lastName="")
        {
            if (firstName == null)
                firstName = "";

            return PatientDAL.GetList(b=>((b.FirstName.Contains(firstName) || b.LastName.Contains(lastName)))).ToList();

        }

        public Patient Get(int patientId)
        {

            var patient = PatientDAL.GetSingle(d => d.PatientId == patientId, b => b.Appointments, b => b.Notes, b => b.Appointments.Select(c => c.Operation), b => b.Appointments.Select(c => c.Notes), b=>b.Appointments.Select(t=>t.Teeth.Select(e=>e.Teeth)), b => b.Appointments.Select(c => c.Operation.Select(o => o.Operation)));
            patient.Appointments = patient.Appointments.GroupBy(p => p.AppointmentId).Select(g => g.First()).ToList().Where(p => p.IsDeleted == false).ToList();
            
            return patient;
        }

        public List<Note> GetPatientNotes(int patientId)
        {
            var patient = Get(patientId);
            var allNotes = new List<Note>();
            allNotes.AddRange(patient.Notes);
            //allNotes.AddRange(patient.Appointments.Select(a => a.Notes).SelectMany(i => i));
            //allNotes.AddRange(patient.Appointments.Select(a => a.Operation.Select(o => o.Notes).SelectMany(i => i)).SelectMany(i => i));
            //allNotes.AddRange(patient.Appointments.Select(a => a.Teeth.Select(o => o.Notes).SelectMany(i => i)).SelectMany(i => i));
            return allNotes.Where(n=>n.IsDeleted==false).ToList<Note>();

        }

        public List<Appointment> GetPatientAppointments(int patientId)
        {
            var list= Get(patientId).Appointments.Select(d => d.Teeth.Where(t => t.IsDeleted == false)).SelectMany(i => i).Where(a => a.Appointment.IsDeleted == false).ToList().Select(i => i.Appointment).ToList();
            return list.GroupBy(p => p.AppointmentId).Select(g => g.First()).ToList().Where(p => p.IsDeleted == false).ToList(); ;
        }

        public List<Appointment> GetPatientAppointments(int patientId, int toothId)
        {
            var list = Get(patientId).Appointments.Select(d => d.Teeth.Where(t => t.Teeth.ToothId == toothId && t.IsDeleted == false)).SelectMany(i => i).Where(a => a.Appointment.IsDeleted == false).ToList().Select(i => i.Appointment).ToList();
            return list.GroupBy(p => p.AppointmentId).Select(g => g.First()).ToList().Where(p => p.IsDeleted == false).ToList();
        }


        public List<Operation> GetPatientOperations(int patientId, int toothId)
        {
            return GetPatientAppointments(patientId, toothId).Select(o=>o.Operation.Select(i => i.Operation)).SelectMany(i => i).OrderBy(o => o.Description).ToList();
        }

        public List<Operation> GetPatientOperations(int patientId)
        {
            return GetPatientAppointments(patientId).Select(o => o.Operation.Select(i => i.Operation)).SelectMany(i => i).OrderBy(o => o.Description).ToList();
        }

        public double TotalAmountToPay(int patiendId)
        {
            return GetPatientAppointments(patiendId).Sum(p => p.AmountToPay);
        }

        public double TotalAmountPaid(int patiendId)
        {
            return GetPatientAppointments(patiendId).Sum(p => p.AmountPaid);
        }
    }
}

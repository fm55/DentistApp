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
        AppointmentController AppointmentController { get; set; }
        OperationController OperationController { get; set; }
        public PatientController()
        {
            PatientDAL = new GenericDataRepository<Patient>();
            AppointmentController = new AppointmentController();
            OperationController = new OperationController();
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
            return PatientDAL.GetList(b => (((string.IsNullOrEmpty(firstName) || b.FirstName.Contains(firstName)) && (string.IsNullOrEmpty(lastName) || b.LastName.Contains(lastName))))).ToList();

        }
        
        public List<Operation> GetOperationsOfPatient(int patientId)
        {
            var patientApps = AppointmentController.GetAppointmentsOfPatient(patientId);
            var operations = patientApps.Select(a => AppointmentController.GetOperationsOfAppointment(a.AppointmentId)).SelectMany(f => f);
          
            return operations.ToList();
        }

        public List<Operation> GetOperationsOfPatientAndTooth(int patientId, int toothid)
        {
            var patientApps = AppointmentController.GetAppointmentsOfPatientAndTooth(patientId, toothid);
            var operations = patientApps.Select(a => AppointmentController.GetOperationsOfAppointment(a.AppointmentId)).SelectMany(f => f);

            return operations.ToList();
        }
        
        public double GetTotalAmountPaid(int patiendId)
        {
            var patientApps = AppointmentController.GetAppointmentsOfPatient(patiendId);
            return patientApps.Sum(p => p.AmountToPay);
        }

        public double GetTotalAmountToPay(int patiendId)
        {
            var patientApps = AppointmentController.GetAppointmentsOfPatient(patiendId);
            return patientApps.Sum(p => p.AmountPaid);
        }


        public Patient Get(int patientId)
        {
            return PatientDAL.GetSingle(p=>p.PatientId==patientId);
        }
    }
}

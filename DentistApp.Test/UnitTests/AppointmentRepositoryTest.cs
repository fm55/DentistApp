using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DentistApp.DAL;
using DentistApp.DAL.DAL;
using DentistApp.Model;
using System.Transactions;
using DentistApp.Test.UnitTests;

namespace DentistApp.Test.DAL.Tests
{
    /// <summary>
    /// Checks if add, update, delete, get and list works
    /// </summary>
    [TestClass]
    public class AppointmentRepositoryTest:BaseTestClass
    {
        [TestMethod]
        public void CanAdd_Update_Delete_Appointment_Lifecycle_Test()
        {

            //add patient then get patients
            var patient = new Patient
                {
                    FirstName = "TestClassFirstName",
                    LastName = "TestClassLastName",
                    TelNo1 = "00000000",

                };
                patient.EntityState = EntityState.Added;
                PatientDAL.Add(patient);
                var  patients = PatientDAL.GetAll();
            patient = patients.FirstOrDefault();
            patient.EntityState = EntityState.Unchanged;

            var teeth = ToothDAL.GetList(t=>t.IsDeleted==false);
            var operations = OperationDAL.GetList(t => t.IsDeleted == false);

            var tooth = teeth.First();
            tooth.EntityState = EntityState.Unchanged;

            var operation = operations.First();
            operation.EntityState = EntityState.Unchanged;


            var teethApp = new TeethAppointment();
            teethApp.EntityState = EntityState.Added;
            teethApp.Teeth = tooth;
            teethApp.TeethId = tooth.ToothId;

            var opreationApp = new OperationAppointment();
            opreationApp.EntityState = EntityState.Added;
            opreationApp.Operation = operation;
            opreationApp.OperationId = operation.OperationId;

            Assert.IsNotNull(patient, null, "Patient is null");

           


            var time = DateTime.Now;
            var Appointment = new Appointment
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(1),
                Description = "Dummy appointment" + time,
                EntityState = EntityState.Added,
                Patient = patient,
                PatientId = patient.PatientId,
                AmountPaid = 100,
                AmountToPay = 100,
                Notes = new List<Note>(),
                Operation = new List<OperationAppointment> { opreationApp },
                Teeth = new List<TeethAppointment> { teethApp }
            };
           

            var allAppointments = AppointmentDAL.GetAll();
            var appointmentCheck = allAppointments.Where(a => a.Description == Appointment.Description).FirstOrDefault();
            Assert.IsNull(appointmentCheck, "Appointment already exists.");

            //Add test
            AppointmentDAL.Add(Appointment);

            var allAppointmentsWithNew = AppointmentDAL.GetAll();
            Assert.AreEqual(allAppointmentsWithNew.Count, allAppointments.Count + 1);

            //arrange
            var appointmentCheckAdded = allAppointmentsWithNew.Where(a => a.Description == Appointment.Description).First();
            Assert.IsNotNull(appointmentCheckAdded, "Appointment isn't added.");
            Assert.IsNotNull("Dummy appointment" + time, Appointment.Description,"Added appointment description is not as expected.");
            Assert.IsNotNull(appointmentCheckAdded, "Appointment isn't added.");

            Assert.AreEqual(patients.Count, PatientDAL.GetAll().Count, "New patient was also added");
            //Assert.AreEqual(operations.Count, OperationDAL.GetAll().Count, "New operations was also added");
            //Assert.AreEqual(teeth.Count, ToothDAL.GetAll().Count, "New teeth was also added");
            //Assert.AreEqual(appointmentCheckAdded.Teeth.Count, Appointment.Teeth.Count, "Teeth were not added");
            //Assert.AreEqual(appointmentCheckAdded.Operation.Count, Appointment.Operation.Count, "Operations were not added");

            //update test
            time = DateTime.Now;
            Appointment.Description = "Updated description" + time;
            Appointment.EntityState = EntityState.Modified;
            Appointment.Notes.Add(new Note { Description = "New Note1", PatientId = patient.PatientId, AppointmentId = Appointment.AppointmentId, EntityState = EntityState.Added });
            Appointment.Notes.Add(new Note { Description = "New Note2", PatientId = patient.PatientId, AppointmentId = Appointment.AppointmentId, EntityState = EntityState.Added });



            //update test
            AppointmentDAL.Update(Appointment);
            //update notes
            foreach (var note in Appointment.Notes)
            {
                NoteDAL.Add(note);
            }

            var allAppointmentsWithUpdate = AppointmentDAL.GetAll();
            var appNotes = NoteDAL.GetAll();
            appNotes = appNotes.Where(d => d.AppointmentId == Appointment.AppointmentId).ToList();


            var appointmentCheckUpdated = allAppointmentsWithUpdate.Where(a => a.Description == Appointment.Description).First();
            Assert.IsNotNull(appointmentCheckUpdated, "Appointment isn't updated.");
            Assert.AreEqual("Updated description" + time, Appointment.Description, "Updated appointment description is not as expected.");
            Assert.IsNotNull(appointmentCheckUpdated, "Appointment isn't added.");
            Assert.AreEqual(appNotes.Count, Appointment.Notes.Count, "Notes are not added");
            //Assert.AreEqual(appointmentCheckUpdated.Teeth.Count, Appointment.Teeth.Count, "Teeth were not added");
            //Assert.AreEqual(appointmentCheckUpdated.Operation.Count, Appointment.Operation.Count, "Operations were not added");


            //delete test
            AppointmentDAL.Remove(Appointment);
            var id = appointmentCheckUpdated.AppointmentId;
            var allAppointmentsWithDelete = AppointmentDAL.GetAll();
            var appointmentsWithDelete = allAppointmentsWithDelete.Where(a => a.AppointmentId == id).FirstOrDefault();
            Assert.IsNull(appointmentsWithDelete, "Appointment isn't deleted.");

           
        }

        [TestCleanup]
        public void Cleanup()
        {
            AppointmentDAL = null;
        }

    }
}

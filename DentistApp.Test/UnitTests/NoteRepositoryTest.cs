using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DentistApp.DAL;
using DentistApp.DAL.DAL;
using DentistApp.Test.UnitTests;

namespace DentistApp.Test.DAL.Tests
{
    /// <summary>
    /// Checks if add, update, delete, get and list works
    /// </summary>
    [TestClass]
    public class NoteRepositoryTest:BaseTestClass
    {
        [TestMethod]
        public void CanAdd_Update_DeleteNoteForPatientAndToothTest()
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
            var patients = PatientDAL.GetAll();
            patient = patients.FirstOrDefault();
            patient.EntityState = EntityState.Unchanged;

            //get tooth
            var tooth = ToothDAL.GetAll().FirstOrDefault();
            var Note = new Note
            {
                Description = "Test Note",
                PatientId = patient.PatientId,
                ToothId = tooth.ToothId
            };

            NoteDAL.Add(Note);
            //arrange
            var expected = NoteDAL.GetSingle(p => p.Description == "Test Note");
            NoteDAL.Remove(expected);
            //act
            var actual = NoteDAL.GetSingle(p => p.Description == "Test Note");
            //assert
            Assert.AreEqual(null, actual, "Record still exists");


        }

        [TestMethod]
        public void CanAdd_Update_DeleteNoteForPatientAndToothAndAppointmentTest()
        {
            string time  = DateTime.Now.ToLongTimeString();

            //add patient then get patients
            var patient = new Patient
            {
                FirstName = "TestClassFirstName",
                LastName = "TestClassLastName",
                TelNo1 = "00000000",

            };
            patient.EntityState = EntityState.Added;
            PatientDAL.Add(patient);
            var patients = PatientDAL.GetAll();
            patient = patients.FirstOrDefault();
            patient.EntityState = EntityState.Unchanged;

            //get tooth
            var tooth = ToothDAL.GetAll().FirstOrDefault();

            //add appointment
            var Appointment = new Appointment
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(1),
                Description = "Dummy appointment" + time,
                EntityState = EntityState.Added,
                Patient = patient,
                PatientId = patient.PatientId,
                AmountPaid = 100,
                AmountToPay = 100
            };


                   //Add test
            AppointmentDAL.Add(Appointment);
            var allAppointments = AppointmentDAL.GetAll();
            var app = allAppointments.Where(a => a.Description == Appointment.Description).FirstOrDefault();
            
            var Note = new Note
            {
                Description = "Test Note",
                PatientId = patient.PatientId,
                ToothId = tooth.ToothId,
                AppointmentId = app.AppointmentId
            };

            NoteDAL.Add(Note);
            //arrange
            var expected = NoteDAL.GetSingle(p => p.Description == "Test Note");

            var expectedNotExists = NoteDAL.GetSingle(p => p.Description == "TestNote123");
            Assert.IsNull(expectedNotExists, "Record exists");

            Note.Description = "TestNote123";

            NoteDAL.Update(Note);

            var expectedExists = NoteDAL.GetSingle(p => p.Description == "TestNote123");
            Assert.IsNotNull(expectedExists, "Record does not exist");

            NoteDAL.Remove(expectedExists);
            //act
            var actual = NoteDAL.GetSingle(p => p.Description == "TestNote123");
            //assert
            Assert.AreEqual(null, actual, "Record still exists");


        }


        [TestCleanup]
        public void Cleanup()
        {
            NoteDAL = null;
        }

    }
}

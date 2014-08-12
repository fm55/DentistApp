using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DentistApp.BL;
using DentistApp.Test.UnitTests;
using DentistApp.DAL;
using DentistApp.DAL.DAL;
using DentistApp.Model;

namespace DentistApp.Test.IntegrationTests
{
    [TestClass]
    public class PatientControllerTests:BaseTestClass
    {
        PatientController PatientController = new PatientController();
        AppointmentController AppointmentController = new AppointmentController();
        
       
        private Patient CreatePatient()
        {
            return new Patient
            {
                FirstName = "FirstName",
                LastName = "LastName"
            };

        }


        private Appointment CreateDummyAppointment(Patient patient, Tooth tooth = null, Operation operation = null)
        {
            patient.EntityState = EntityState.Unchanged;
            var teeth = ToothDAL.GetAll().ToList();
            var operations = OperationDAL.GetList(t => t.IsDeleted == false);

            if (tooth == null)
            {
                tooth = teeth.First();
            }
            tooth.EntityState = EntityState.Unchanged;

            if (operation == null)
                operation = operations.First();

            operation.EntityState = EntityState.Unchanged;


            var teethApp = new TeethAppointment();
            teethApp.EntityState = EntityState.Added;
            teethApp.Teeth = tooth;
            teethApp.TeethId = tooth.ToothId;

            var opreationApp = new OperationAppointment();
            opreationApp.EntityState = EntityState.Added;
            opreationApp.Operation = operation;
            opreationApp.OperationId = operation.OperationId;
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
            return Appointment;
        }

        [TestMethod]
        public void DeleteTest()
        {
            var patient = CreatePatient();
            PatientDAL.Add(patient);

            var patientsFromDB = PatientDAL.GetAll();
            Assert.AreEqual(1, patientsFromDB.Count());

            PatientController.Delete(patient);
            patientsFromDB = PatientDAL.GetAll();
            Assert.AreEqual(0, patientsFromDB.Count());
        }

        [TestMethod]
        public void GetTest()
        {
            var patient = CreatePatient();
            PatientDAL.Add(patient);

            var patientsFromDB = PatientDAL.GetAll();

            
            Assert.AreEqual(1, patientsFromDB.Count());
            var id = patientsFromDB.FirstOrDefault().PatientId;


            var patientFromController = PatientController.Get(id);
            Assert.IsNotNull(patientFromController);
            Assert.AreEqual(patientFromController.PatientId, patientsFromDB.FirstOrDefault().PatientId);
        }

        [TestMethod]
        public void GetOperationsOfPatientTest()
        {
            var patient = CreatePatient();
            PatientDAL.Add(patient);

            var app = CreateDummyAppointment(patient);

            AppointmentController.SaveAppointment(app);

            var ops = PatientController.GetOperationsOfPatient(patient.PatientId);

            Assert.AreEqual(ops.Count, 1);

        }

        [TestMethod]
        public void GetOperationsOfPatientAndToothTest()
        {
            var patient = CreatePatient();
            PatientController.Save(patient);
            var tooth = ToothDAL.GetAll().FirstOrDefault();
            var app = CreateDummyAppointment(patient, tooth);
            AppointmentController.SaveAppointment(app);//one tooth for appointment
            var ops = PatientController.GetOperationsOfPatientAndTooth(patient.PatientId, tooth.ToothId);

            Assert.AreEqual(ops.Count, 1);
        }

        [TestMethod]
        public void ListTest()
        {
            var patientA = CreatePatient();
            patientA.FirstName = "FirstPatient";

            var patientB = CreatePatient();
            patientB.FirstName = "LastPatient";

            PatientController.Save(patientA);
            PatientController.Save(patientB);

            var patients = PatientController.List();
            Assert.AreEqual(2, patients.Count);

            var patientsSubList = PatientController.List("Patient");
            Assert.AreEqual(2, patientsSubList.Count);

            var patientsSubListWithFirst = PatientController.List("First");
            
            Assert.AreEqual(1, patientsSubListWithFirst.Count);
        }

        [TestMethod]
        public void SaveTest()
        {
            var patientA = CreatePatient();
            patientA.FirstName = "FirstPatient";

            PatientController.Save(patientA);

            var patients = PatientController.List();
            Assert.AreEqual(1, patients.Count);

            Assert.AreEqual("FirstPatient", patients.FirstOrDefault().FirstName);

            patientA.FirstName = "LastPatient";
            PatientController.Save(patientA);
            patients = PatientController.List();
            Assert.AreEqual(1, patients.Count);
            Assert.AreEqual("LastPatient", patients.FirstOrDefault().FirstName);
        }

        [TestMethod]
        public void GetTotalAmountPaidTest()
        {
            var patient = CreatePatient();
            PatientController.Save(patient);

            patient = PatientController.List().First();
            var tooth = ToothDAL.GetAll().FirstOrDefault();
            var app = CreateDummyAppointment(patient, tooth);
            app.AmountPaid = 100;

            AppointmentController.SaveAppointment(app);//one tooth for appointment

            Assert.AreEqual(100, PatientController.GetTotalAmountPaid(patient.PatientId));
        }

        [TestMethod]
        public void GetTotalAmountToPayTest()
        {
            var patient = CreatePatient();
            PatientController.Save(patient);

            patient = PatientController.List().First();
            var tooth = ToothDAL.GetAll().FirstOrDefault();
            var app = CreateDummyAppointment(patient, tooth);
            app.AmountToPay = 100;

            AppointmentController.SaveAppointment(app);//one tooth for appointment

            Assert.AreEqual(100, PatientController.GetTotalAmountToPay(patient.PatientId));
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DentistApp.BL;
using DentistApp.Test.UnitTests;
using DentistApp.DAL;
using DentistApp.Model;

namespace DentistApp.Test.IntegrationTests
{
    [TestClass]
    public class AppointmentControllerTests : BaseTestClass
    {
        private AppointmentController AppointmentController = new AppointmentController();


        public DentistApp.DAL.DAL.IGenericDataRepository<DentistApp.DAL.Appointment> AppointmentRepository
        {
            get
            {
                return AppointmentDAL;
            }
        }

        [TestMethod]
        public void DeleteAppointmentTest()
        {
            var patient = CreateDummyPatient(1);
            
            
            var app = CreateDummyAppointment(patient);
            AppointmentDAL.Add(app);

            var appFromDb = AppointmentDAL.GetList(d => d.Description == app.Description).FirstOrDefault();

            AppointmentController.Delete(appFromDb);
            appFromDb = AppointmentDAL.GetList(d => d.Description == app.Description).FirstOrDefault();
            Assert.IsNull(appFromDb, "Appointment is not deleted");

        }

        [TestMethod]
        public void GetAppointmentsOfPatientTest()
        {
            var patient = CreateDummyPatient(1);
           
            
            var appA = CreateDummyAppointment(patient);
            AppointmentDAL.Add(appA);
            var appB = CreateDummyAppointment(patient);
            AppointmentDAL.Add(appB);
            IEnumerable<DentistApp.DAL.Appointment> apps = AppointmentController.GetAppointmentsOfPatient(patient.PatientId);
            Assert.AreEqual(2, apps.Count(), "Not as expected appointments for the patient");

        }

        [TestMethod]
        public void GetAppointmentsOfPatientAndToothTest()
        {
            var patient = CreateDummyPatient(1);

            var tooth = ToothDAL.GetAll().FirstOrDefault();
            var appA = CreateDummyAppointment(patient, tooth);
            AppointmentDAL.Add(appA);

            var appB = CreateDummyAppointment(patient, tooth);
            AppointmentDAL.Add(appB);

            var apps = AppointmentController.GetAppointmentsOfPatientAndTooth(patient.PatientId, tooth.ToothId);

            Assert.IsNotNull(apps, "Appointment is not created");
            Assert.AreEqual(2, apps.Count(), "Not as expected appointments for the patient and tooth");
        }

        [TestMethod]
        public void GetOperationsOfAppointmentTest()
        {
            var patient = CreateDummyPatient(1);

            var operation = OperationDAL.GetAll().Last();
            //an appointment for last operation
            var app = CreateDummyAppointment(patient, null, operation);
            app.Description="GetOperationsOfAppointmentTest";
            AppointmentDAL.Add(app);

            //get appointment back
            var appFromDb = AppointmentDAL.GetList(d => d.Description == "GetOperationsOfAppointmentTest").FirstOrDefault();
            //get operations of appointment
            var ops = AppointmentController.GetOperationsOfAppointment(appFromDb.AppointmentId);

            Assert.IsNotNull(ops, "Operation is not in db");
            Assert.AreEqual(1, ops.Count(), "Not as expected appointments for the patient and tooth");
            //operation must be same
            Assert.AreEqual(ops.FirstOrDefault().OperationId, operation.OperationId);


        }

        [TestMethod]
        public void ListAppointmentsWithinATimeFrameFullyNotPaidTest()
        {
            var patient = CreateDummyPatient(1);

            var appA = CreateDummyAppointment(patient);
            //add it for today
            appA.StartTime = DateTime.Now.AddHours(-1);
            appA.EndTime = DateTime.Now.AddHours(1);
            appA.AmountToPay = 100;
            appA.AmountPaid = 10;
            AppointmentDAL.Add(appA);

            var appB = CreateDummyAppointment(patient);
            appB.AmountToPay = 100;
            appB.AmountPaid = 10;
            //add it after one year
            appB.StartTime = DateTime.Now.AddYears(-1);
            appB.EndTime = DateTime.Now.AddYears(1);
            AppointmentDAL.Add(appB);

            //should return just one
            var apps = AppointmentController.List(0, false, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
            Assert.AreEqual(1, apps.Count());

        }

        [TestMethod]
        public void ListAppointmentsWithinATimeFrameFullyPaidTest()
        {
            var patient = CreateDummyPatient(1);

            var appA = CreateDummyAppointment(patient);
            //add it for today
            appA.StartTime = DateTime.Now.AddHours(-1);
            appA.EndTime = DateTime.Now.AddHours(1);
            appA.AmountToPay = 100;
            appA.AmountPaid = 100;
            AppointmentDAL.Add(appA);

            var appB = CreateDummyAppointment(patient);
            appB.AmountToPay = 100;
            appB.AmountPaid = 10;
            //add it after one year
            appB.StartTime = DateTime.Now.AddHours(-1);
            appB.EndTime = DateTime.Now.AddHours(1);
            AppointmentDAL.Add(appB);

            //should return just one as it is fully paid
            var apps = AppointmentController.List(0, true, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
            Assert.AreEqual(1, apps.Count());

        }


        [TestMethod]
        public void ListTodayAppointmentsTest()
        {
            var patient = CreateDummyPatient(1);

            var appA = CreateDummyAppointment(patient);
            //add it for today
            appA.StartTime = DateTime.Now;
            appA.EndTime = DateTime.Now.AddHours(1);
            AppointmentDAL.Add(appA);

            var appB = CreateDummyAppointment(patient);
            //add it after one year
            appB.StartTime = DateTime.Now.AddYears(1);
            appB.EndTime = DateTime.Now.AddYears(2);
            AppointmentDAL.Add(appB);

            //should return just one as it is fully paid
            var apps = AppointmentController.ListTodayAppointments();
            Assert.AreEqual(1, apps.Count());
        }

        [TestMethod]
        public void SaveExistingAppointmentTest()
        {
            var patient = CreateDummyPatient(1);

            var appA = CreateDummyAppointment(patient);
            appA.Description="SaveExistingAppointmentTest";
            AppointmentDAL.Add(appA);
            //get appointment back
            appA = AppointmentDAL.GetList(d => d.Description == "SaveExistingAppointmentTest").FirstOrDefault();
            Assert.IsNotNull(appA);
            appA.Description = "SaveExistingAppointmentTestTwo";
            AppointmentController.SaveAppointment(appA);
            //get appointment back
            appA = AppointmentController.List(appA.AppointmentId).FirstOrDefault();
            Assert.IsNotNull(appA);
            Assert.AreEqual("SaveExistingAppointmentTestTwo", appA.Description);
        }

        [TestMethod]
        public void SaveNewAppointmentTest()
        {
            var patient = CreateDummyPatient(1);

            var appA = CreateDummyAppointment(patient);
            appA.Description = "SaveExistingAppointmentTest";
            AppointmentController.SaveAppointment(appA);
            //get appointment back
            appA = AppointmentController.List(appA.AppointmentId).FirstOrDefault();
            Assert.IsNotNull(appA);
            Assert.AreEqual("SaveExistingAppointmentTest", appA.Description);
        }

        
        [TestMethod]
        public void GetTeethOfAppointmentTest()
        {
            var patient = CreateDummyPatient(1);

            var tooth = ToothDAL.GetAll().FirstOrDefault();
            var appA = CreateDummyAppointment(patient, tooth);
            AppointmentController.SaveAppointment(appA);//one tooth for appointment
            //get it back
            var appFromDb = AppointmentController.List(appA.AppointmentId).First();
            var teeth = AppointmentController.GetTeethOfAppointment(appFromDb.AppointmentId);

            Assert.IsNotNull(appFromDb, "Appointment is not created");
            Assert.AreEqual(1, teeth.Count(), "Not as expected tooth for the appointment");
            Assert.AreEqual(teeth.First().ToothId, tooth.ToothId, "Not the same tooth as expected");//Is it the same tooth
        }

        private Patient CreateDummyPatient(int randomInteger)
        {
            //add patient then get patients
            var patient = new Patient
            {
                FirstName = "TestClassFirstName" + randomInteger,
                LastName = "TestClassLastName",
                TelNo1 = "00000000",

            };
            PatientDAL.Add(patient);
            var patients = PatientDAL.GetList(p=>p.FirstName=="TestClassFirstName" + randomInteger);
            patient = patients.FirstOrDefault();
            patient.EntityState = EntityState.Added;
            return patient;
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

            if (operation==null)
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

    }
}

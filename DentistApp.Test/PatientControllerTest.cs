using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DentistApp.BL;
using DentistApp.Model;
using Moq;
using System.Linq.Expressions;
using DentistApp.DAL.DAL;
using DentistApp.DAL;

namespace DentistApp.Test
{
    [TestClass]
    public class PatientControllerTest
    {
        private IGenericDataRepository<Patient> PatientDAL;
        PatientController Controller;

        [TestInitialize]
        public void Initialize()
        {
            //http://www.codeproject.com/Articles/47603/Mock-a-database-repository-using-Moq
            //https://github.com/Moq/moq4/wiki/Quickstart

            //mock repository
            var mockPatientDAL = new Mock<IGenericDataRepository<Patient>>();

            //initialize patients and add to the repository
            var patients = new List<Patient>{
                new Patient {PatientId = 1, FirstName = "F1", LastName = "L1", IsDeleted = false},
                new Patient {PatientId = 2, FirstName = "F2", LastName = "L2", IsDeleted = false},
                new Patient {PatientId = 3, FirstName = "F3", LastName = "L3", IsDeleted = false},
                new Patient {PatientId = 4, FirstName = "F4", LastName = "L4", IsDeleted = false}
            };

           
            mockPatientDAL.Setup(m => m.GetAll()).Returns(patients);
            
            mockPatientDAL.Setup(m => m.GetList(It.IsAny<Func<Patient, bool>>())
                ).Returns((Func<Patient, bool> s) =>
                {
                    var patientsThatMeetCondition =  patients.Where(s).ToList<Patient>();
                    return patientsThatMeetCondition.Where(p => p.IsDeleted == false).ToList<Patient>();
                    
                }); 

            //behaviour for save

            mockPatientDAL.Setup(m => m.Add(It.IsAny<Patient>())).Returns(
                (Patient input) =>
                {
                    input.PatientId = patients.Count + 1;
                    input.IsDeleted = false;
                    patients.Add(input);
                    return 1;
                }
                );
            
            //behaviour for remove
            mockPatientDAL.Setup(m => m.Remove(It.IsAny<Patient[]>())).Returns(
                (Patient input) => 
                {
                    var patient = patients.Where(p => p.PatientId == input.PatientId).FirstOrDefault();
                    patient.IsDeleted = true;
                    return 1;
                }
            );
            

            //behaviour for get single

            mockPatientDAL.Setup(m => m.GetSingle(It.IsAny<Func<Patient, bool>>())).Returns(
                (Func<Patient, bool> input) =>
                {
                    var patientsThatMeetCondition =  patients.Where(input).ToList<Patient>();
                    return patientsThatMeetCondition.Where(p => p.IsDeleted == false).FirstOrDefault<Patient>();
                });

            PatientDAL = mockPatientDAL.Object;
            
            Controller = new PatientController(PatientDAL);

        }

        /// <summary>
        //Create a patient
        ///</summary>
        [TestMethod()]
        public void UpdatePatientTest()
        {
            var patientAlreadyExisted = new Patient { PatientId = 4, FirstName = "F4Updated", LastName = "L4Updated", IsDeleted = false };
            Controller.Save(patientAlreadyExisted);
            var patient = Controller.Get(4);
            Assert.IsNotNull(patient);
            Assert.IsInstanceOfType(patient, typeof(Patient));
            Assert.AreEqual(patient.FirstName, patientAlreadyExisted.FirstName);
            
        }

        /// <summary>
        //Create a patient
        ///</summary>
        [TestMethod()]
        public void CreatePatientTest()
        {
            var patientNew = new Patient { FirstName = "F5", LastName = "L5", IsDeleted = false };
            Controller.Save(patientNew);
            var patient = Controller.Get(5);
            Assert.IsNotNull(patient);
            Assert.IsInstanceOfType(patient, typeof(Patient));
            Assert.AreEqual(patient.PatientId, Controller.List().Count);
        }

        /// <summary>
        //Create a patient
        ///</summary>
        [TestMethod()]
        public void DeletePatientTest()
        {
            var patientAlreadyExisted = new Patient { PatientId = 4, FirstName = "F4Updated", LastName = "L4Updated", IsDeleted = false };
            Controller.Delete(patientAlreadyExisted);
            var patient = Controller.Get(4);            
            Assert.IsNull(patient);
        }

        /// <summary>
        //Create a patient
        ///</summary>
        [TestMethod()]
        public void GetPatientTest()
        {
            var patientToExpect = new Patient { PatientId=1, FirstName = "F1", LastName = "L1" };
            var patient = Controller.Get(1);
            Assert.AreEqual(patientToExpect.PatientId, patient.FirstName);
        }

        /// <summary>
        //Create a patient
        ///</summary>
        [TestMethod()]
        public void ListAllPatientsTest()
        {
            var patients = Controller.List();
            var patientNew = new Patient { FirstName = "F5", LastName = "L5", IsDeleted = false };
            Controller.Save(patientNew);
            var patientsNew = Controller.List();
            Assert.AreEqual(patientsNew.Count, patients.Count+1);
        }

        /// <summary>
        //Create a patient
        ///</summary>
        [TestMethod()]
        public void ListPatientsWithCriteriaTest()
        {
            var patient = Controller.List("F1", "L1").FirstOrDefault();
            Assert.IsNotNull(patient);
            Assert.IsInstanceOfType(patient, typeof(Patient));
            Assert.AreEqual(patient.PatientId, 1);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            PatientDAL = null;
            Controller = null;
        }

    }
}

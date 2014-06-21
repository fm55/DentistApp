using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DentistApp.DAL;
using DentistApp.DAL.DAL;

namespace DentistApp.Test.DAL.Tests
{
    [TestClass]
    public class PatientRepositoryTest
    {
        private IGenericDataRepository<Patient> PatientDAL { get; set; }
        
        public PatientRepositoryTest()
        {
            
        }

        [TestInitialize]
        public void Initialize()
        {
            PatientDAL = new GenericDataRepository<Patient>();

        }

        [TestMethod]
        public void CanAddTest()
        {
            var Patient = new Patient
            {
                FirstName = "TestClassFirstName",
                LastName = "TestClassLastName",
                TelNo1 = "00000000",

            };


            //arrange
            var expected = 1;
            //act
            var actual = PatientDAL.Add(Patient);
            //assert
            Assert.AreEqual(expected, actual, "Number of record saved is not 1");
        }

        [TestMethod]
        public void CanUpdateTest()
        {
            var Patient = new Patient
            {
                FirstName = "TestClassFirstName1",
                LastName = "TestClassLastName2",
                TelNo1 = "00000000",
                PatientId = 1
            };


            //arrange
            var expected = 1;
            //act
            var actual = PatientDAL.Update(Patient);
            //assert
            Assert.AreEqual(expected, actual, "Number of record saved is not 1");


        }


        [TestMethod]
        public void CanDeleteTest()
        {
            var Patient = new Patient
            {
                PatientId = 1
            };


            //arrange
            var expected = 1;
            //act
            var actual = PatientDAL.Remove(Patient);
            //assert
            Assert.AreEqual(expected, actual, "Number of record deleted is not 1");


        }

        [TestMethod]
        public void CanGetTest()
        {
            string time  = DateTime.Now.ToLongTimeString();
            var patient = new Patient
            {
                FirstName = "NewRecord" + time

            };

            var expected = 1;
            var actual = PatientDAL.Add(patient);
            Assert.AreEqual(expected, actual, "Number of record deleted is not 1");

            var patientFromDB = PatientDAL.GetSingle(d => d.FirstName == "NewRecord" + time);
            Assert.IsNotNull(patientFromDB, "Patient record is null");
            Assert.AreEqual(patientFromDB.FirstName, patient.FirstName, "Created and saved patient records are not same");

        }

        [TestMethod]
        public void CanListTest()
        {

            string time = DateTime.Now.ToLongTimeString();
            var patient = new Patient
            {
                FirstName = "NewRecord" + time

            };

            var expected = new List<Patient>(){patient};
            var actual = PatientDAL.Add(patient);
            Assert.AreEqual(1, actual, "Number of record deleted is not 1");

            var patientFromDB = PatientDAL.GetList(d => d.FirstName == "NewRecord" + time);
            Assert.IsNotNull(patientFromDB, "Patient record is null");
            Assert.AreEqual(1, patientFromDB.Count);
            CollectionAssert.AllItemsAreInstancesOfType(patientFromDB.ToList<Patient>(),typeof(Patient), "All items must be of type patient.");
            
        }


        [TestCleanup]
        public void Cleanup()
        {
            PatientDAL = null;
        }

    }
}

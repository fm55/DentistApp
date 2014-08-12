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
    public class PatientRepositoryTest:BaseTestClass

    {

        [TestMethod]
        public void CanAddTest()
        {
            var time = DateTime.Now;
            var Patient = new Patient
            {
                FirstName = "TestClassFirstName1" + time,
                LastName = "TestClassLastName2",
                TelNo1 = "00000000"
            };
            PatientDAL.Add(Patient);
            var expected = PatientDAL.GetSingle(p => p.FirstName == "TestClassFirstName1" + time);

            //act
            Assert.IsNotNull(expected);

        }

        [TestMethod]
        public void CanUpdateTest()
        {
            var time = DateTime.Now;
            var Patient = new Patient
            {
                FirstName = "TestClassFirstName1" + time,
                LastName = "TestClassLastName2",
                TelNo1 = "00000000"
            };
            PatientDAL.Add(Patient);
            Patient.FirstName = "TestClassFirstName2" + time;

            PatientDAL.Update(Patient);
            //arrange
            var expected = PatientDAL.GetSingle(p => p.FirstName == "TestClassFirstName2" + time);
            
            //act
            Assert.IsNotNull(expected);
        }


        [TestMethod]
        public void CanDeleteTest()
        {
            var time = DateTime.Now;
            var Patient = new Patient
            {
                FirstName = "TestClassFirstName1" + time,
                LastName = "TestClassLastName2",
                TelNo1 = "00000000"
            };
            PatientDAL.Add(Patient);
            var expected = PatientDAL.GetSingle(p => p.FirstName == "TestClassFirstName1" + time);
            PatientDAL.Remove(expected);
            //act
            var actual = PatientDAL.GetSingle(p => p.FirstName == "TestClassFirstName1" + time);
            //assert
            Assert.IsNull(actual, "Record still exists");


        }


        [TestCleanup]
        public void Cleanup()
        {
            PatientDAL = null;
        }

    }
}

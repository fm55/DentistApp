using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DentistApp.DAL;
using DentistApp.DAL.DAL;
using DentistApp.Model;
using DentistApp.Test.UnitTests;

namespace DentistApp.Test.DAL.Tests
{
    /// <summary>
    /// Checks if add, update, delete, get and list works
    /// </summary>
    [TestClass]
    
    public class OperationRepositoryTest:BaseTestClass
    {
        private Operation CreateOperation(DateTime time)
        {
            
            var Operation = new Operation
            {
                Description = "Dummy operation" + time

            };
            return Operation;
        }
        [TestMethod]
        public void CanAddTest()
        {

            var time = DateTime.Now;
           OperationDAL.Add(CreateOperation(time));
           var op = OperationDAL.GetList(o => o.Description == "Dummy operation" + time).FirstOrDefault();//does it come from database?

            //assert
           Assert.IsNotNull(op, "Operation not saved");
        }

        [TestMethod]
        public void CanUpdateTest()
        {
            var time = DateTime.Now;
            OperationDAL.Add(CreateOperation(time));
            var op = OperationDAL.GetList(o => o.Description == "Dummy operation" + time).FirstOrDefault();//does it come from database?
            op.Description = "Changed description of the operation";
            OperationDAL.Update(op);
            //act
            var actual = OperationDAL.GetSingle(p => p.OperationId == op.OperationId);
            Assert.AreEqual(actual.Description, "Changed description of the operation");
        }


        [TestMethod]
        public void CanDeleteTest()
        {
            var time = DateTime.Now;
            OperationDAL.Add(CreateOperation(time));
            var op = OperationDAL.GetList(o => o.Description == "Dummy operation" + time).FirstOrDefault();//does it come from database?
            OperationDAL.Remove(op);
            op = OperationDAL.GetList(o => o.Description == "Dummy operation" + time).FirstOrDefault();//does it come from database?
            //assert
            Assert.AreEqual(null, op, "Record still exists");


        }

    }
}

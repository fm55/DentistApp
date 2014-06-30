using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DentistApp.DAL;
using DentistApp.DAL.DAL;
using DentistApp.Model;

namespace DentistApp.Test.DAL.Tests
{
    /// <summary>
    /// Checks if add, update, delete, get and list works
    /// </summary>
    [TestClass]
    public class OperationRepositoryTest
    {
        private IGenericDataRepository<Operation> OperationDAL { get; set; }
        
        public OperationRepositoryTest()
        {
            
        }

        [TestInitialize]
        public void Initialize()
        {
            OperationDAL = new GenericDataRepository<Operation>();

        }

        [TestMethod]
        public void CanAddTest()
        {
            var Operation = new Operation
            {
               

            };


            //arrange
            var expected = 1;
            //act
            var actual = OperationDAL.Add(Operation);
            //assert
            Assert.AreEqual(expected, actual, "Number of record saved is not 1");
        }

        [TestMethod]
        public void CanUpdateTest()
        {
            var Operation = new Operation
            {
               
                OperationId = 1
            };


            //arrange
            var expected = Operation;
            OperationDAL.Update(Operation);
            //act
            var actual = OperationDAL.GetSingle(p => p.OperationId == expected.OperationId); ;




            //assert
           
        }


        [TestMethod]
        public void CanDeleteTest()
        {
            var Operation = new Operation
            {
                OperationId = 1
            };


            //arrange
            var expected = OperationDAL.GetSingle(p => p.OperationId == Operation.OperationId);
            OperationDAL.Remove(expected);
            //act
            var actual = OperationDAL.GetSingle(p => p.OperationId == Operation.OperationId);
            //assert
            Assert.AreEqual(null, actual, "Record still exists");


        }

        [TestMethod]
        public void CanGetTest()
        {
            string time  = DateTime.Now.ToLongTimeString();
            var Operation = new Operation
            {
               
            };

            var expected = 1;
            var actual = OperationDAL.Add(Operation);

           

        }

        [TestMethod]
        public void CanListTest()
        {

            string time = DateTime.Now.ToLongTimeString();
            var Operation = new Operation
            {
               
            };

            var expected = new List<Operation>(){Operation};
            OperationDAL.Add(Operation);

            var OperationFromDB = OperationDAL.GetList(o=>o.OperationId==1);
            Assert.IsNotNull(OperationFromDB, "Operation record is null");
            Assert.AreEqual(1, OperationFromDB.Count);
            CollectionAssert.AllItemsAreInstancesOfType(OperationFromDB.ToList<Operation>(),typeof(Operation), "All items must be of type Operation.");
            
        }


        [TestCleanup]
        public void Cleanup()
        {
            OperationDAL = null;
        }

    }
}

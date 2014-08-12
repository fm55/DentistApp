using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using DentistApp.DAL.DAL;
using DentistApp.DAL;
using DentistApp.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DentistApp.Test.UnitTests
{
    public class BaseTestClass
    {
        public IGenericDataRepository<Appointment> AppointmentDAL { get; set; }

        //adding patient dal as appointments are dependent on patients
        public IGenericDataRepository<Patient> PatientDAL { get; set; }
        public IGenericDataRepository<Appointment> AppointmenttDAL { get; set; }
        public IGenericDataRepository<Tooth> ToothDAL { get; set; }
        public IGenericDataRepository<Operation> OperationDAL { get; set; }
        public IGenericDataRepository<Note> NoteDAL { get; set; }
        DentistDbContext context;
        TransactionScope scope;

        public BaseTestClass()
        {
            context = new DentistDbContext();
        }

        [TestInitialize]
        public void Initialize()
        {
            AppointmentDAL = new GenericDataRepository<Appointment>();
            PatientDAL = new GenericDataRepository<Patient>();
            ToothDAL = new GenericDataRepository<Tooth>();
            OperationDAL = new GenericDataRepository<Operation>();
            NoteDAL = new GenericDataRepository<Note>();
            BeginTransaction();
        }
        
        
        public void BeginTransaction()
        {
            // define our transaction scope
            scope = new TransactionScope(
                // a new transaction will always be created
                TransactionScopeOption.RequiresNew,
                // we will allow volatile data to be read during transaction
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }
            );
        }

        [TestCleanup]
        public void TestCleanup()
        {
            scope.Dispose();
        }
    }
}

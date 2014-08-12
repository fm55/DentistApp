using DentistApp.BL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DentistApp.DAL;
using DentistApp.DAL.DAL;
using System.Collections.Generic;
using DentistApp.Test.UnitTests;

namespace DentistApp.Test
{
    
    
    /// <summary>
    ///This is a test class for NoteControllerTest and is intended
    ///to contain all NoteControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NoteControllerTest:BaseTestClass
    {
        private Note CreateNote()
        {
            return new Note()
            {
                Description = "Test Note",
                DateCreated = DateTime.Now
            };
        }
        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            NoteController target = new NoteController(); // TODO: Initialize to an appropriate value
            Note Note = CreateNote();
            NoteDAL.Add(Note);

            var notes = NoteDAL.GetAll();

            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(Note.Description, notes[0].Description);

            target.Delete(Note);

            notes = NoteDAL.GetAll();

            Assert.AreEqual(0, notes.Count);
        }

        /// <summary>
        ///A test for List
        ///</summary>
        [TestMethod()]
        public void ListTest()
        {
            NoteController target = new NoteController(); // TODO: Initialize to an appropriate value
            Note Note = CreateNote();
            NoteDAL.Add(Note);

            var notes = NoteDAL.GetList(n => n.Description == "Test Note");

            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(Note.Description, "Test Note");
        }

        /// <summary>
        ///A test for SaveNote
        ///</summary>
        [TestMethod()]
        public void SaveNoteTest()
        {
            NoteController target = new NoteController(); // TODO: Initialize to an appropriate value
            Note Note = CreateNote();
            target.SaveNote(Note);

            var notes = NoteDAL.GetList(n => n.Description == "Test Note");

            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(Note.Description, "Test Note");
        }

        /// <summary>
        ///A test for SaveNote
        ///</summary>
        [TestMethod()]
        public void SaveNoteTest1()
        {
            NoteController target = new NoteController(); // TODO: Initialize to an appropriate value
            Note Note = CreateNote();
            Nullable<int> PatientId = 1;
            Nullable<int> OperationId = 2; // TODO: Initialize to an appropriate value
            Nullable<int> ToothId = 3; // TODO: Initialize to an appropriate value
            Nullable<int> AppointmentId = 4; // TODO: Initialize to an appropriate value
            target.SaveNote(Note, PatientId, OperationId, ToothId, AppointmentId);
            var notes = NoteDAL.GetList(n => n.Description == "Test Note");

            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(Note.Description, "Test Note");
            Assert.AreEqual(Note.PatientId, 1);
            Assert.AreEqual(Note.OperationId, 2);
            Assert.AreEqual(Note.ToothId, 3);
            Assert.AreEqual(Note.AppointmentId, 4);
        }

        [TestMethod()]
        public void GetNotesForPatientTest()
        {
            NoteController target = new NoteController();
            Note Note = CreateNote();
            int PatientId = 1;
            target.SaveNote(Note, PatientId);

            var notes = target.GetNotesForPatient(PatientId);
            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(1, notes[0].PatientId);
        }

        [TestMethod()]
        public void GetNotesForPatientAndToothTest()
        {
            NoteController target = new NoteController();
            Note Note = CreateNote();
            int PatientId = 1;
            target.SaveNote(Note, PatientId,0, 2);

            var notes = target.GetNotesForPatientAndTooth(PatientId, 2);
            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(1, notes[0].PatientId);
            Assert.AreEqual(2, notes[0].ToothId);
        }

        [TestMethod()]
        public void GetNotesForAppointmentTest()
        {
            NoteController target = new NoteController();
            Note Note = CreateNote();
            int PatientId = 1;
            target.SaveNote(Note, PatientId,0,0, 3);

            var notes = target.GetNotesForAppointment(3);
            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(3, notes[0].AppointmentId);
        }
        [TestMethod()]
        public void GetNotesForAppointmentAndOperationTest()
        {
            NoteController target = new NoteController();
            Note Note = CreateNote();
            int PatientId = 1;
            target.SaveNote(Note, PatientId, 2, 0, 3);

            var notes = target.GetNotesForPatient(PatientId);
            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(3, notes[0].AppointmentId);
            Assert.AreEqual(2, notes[0].OperationId);
        }


        [TestMethod()]
        public void GetNotesForOperationTest()
        {
            NoteController target = new NoteController();
            Note Note = CreateNote();
            target.SaveNote(Note, 0, 1);

            var notes = target.GetNotesForOperation(1);
            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(1, notes[0].OperationId);
        }

        [TestMethod()]
        public void GetNotesForToothTest()
        {
            NoteController target = new NoteController();
            Note Note = CreateNote();
            target.SaveNote(Note, 0,0,1,0);

            var notes = target.GetNotesForTooth(1);
            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(1, notes[0].ToothId);
        }
        [TestMethod()]
        public void GetNotesForSystemTest()
        {
            NoteController target = new NoteController();
            Note Note = CreateNote();
            target.SaveNote(Note);

            var notes = target.GetNotesForSystem();
            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(null, notes[0].PatientId);
        }


       
    }
}

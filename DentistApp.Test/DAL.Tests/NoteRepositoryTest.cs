using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DentistApp.DAL;
using DentistApp.DAL.DAL;

namespace DentistApp.Test.DAL.Tests
{
    /// <summary>
    /// Checks if add, update, delete, get and list works
    /// </summary>
    [TestClass]
    public class NoteRepositoryTest
    {
        private IGenericDataRepository<Note> NoteDAL { get; set; }
        
        public NoteRepositoryTest()
        {
            
        }

        [TestInitialize]
        public void Initialize()
        {
            NoteDAL = new GenericDataRepository<Note>();

        }

        [TestMethod]
        public void CanAdd_Update_DeleteNoteForApplicationTest()
        {
            var Note = new Note
            {
               

            };


            //arrange
            var expected = 1;
            //act
            var actual = NoteDAL.Add(Note);
            //assert
            Assert.AreEqual(expected, actual, "Number of record saved is not 1");
        }

        [TestMethod]
        public void CanAdd_Update_DeleteNoteForPatientTest()
        {
            var Note = new Note
            {
                
                NoteId = 1
            };


            //arrange
            var expected = Note;
            NoteDAL.Update(Note);
            //act
            var actual = NoteDAL.GetSingle(p => p.NoteId == expected.NoteId); ;




            //assert
          
        }


        [TestMethod]
        public void CanAdd_Update_DeleteNoteForPatientAndToothTest()
        {
            var Note = new Note
            {
                NoteId = 1
            };


            //arrange
            var expected = NoteDAL.GetSingle(p => p.NoteId == Note.NoteId);
            NoteDAL.Remove(expected);
            //act
            var actual = NoteDAL.GetSingle(p => p.NoteId == Note.NoteId);
            //assert
            Assert.AreEqual(null, actual, "Record still exists");


        }

        [TestMethod]
        public void CanAdd_Update_DeleteNoteForPatientAndAppointmentTest()
        {
            string time  = DateTime.Now.ToLongTimeString();
            var Note = new Note
            {
             

            };

            var expected = 1;
            var actual = NoteDAL.Add(Note);


        }


        [TestCleanup]
        public void Cleanup()
        {
            NoteDAL = null;
        }

    }
}

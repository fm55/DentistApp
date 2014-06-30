using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.Model;
using DentistApp.DAL.DAL;
using DentistApp.DAL;

namespace DentistApp.BL
{
    public class NoteController
    {
        IGenericDataRepository<Note> NoteRepository { get; set; }
        public PatientController PatientController { get; set; }

        public NoteController()
        {
            NoteRepository = new GenericDataRepository<Note>();
            PatientController = new PatientController();
        }

        public NoteController(IGenericDataRepository<Note> noteRepository)
        {
            NoteRepository = noteRepository;
        }

        public void SaveNote(Note Note)
        {
            if (Note.NoteId == 0)
            {
                Note.EntityState = EntityState.Added;
                NoteRepository.Add(Note);
            }
            else
            {
                Note.EntityState = EntityState.Modified;
                NoteRepository.Update(Note);
            }

        }

        public void SaveNote(Note Note, int? PatientId=0, int? OperationId=0, int? ToothId=0, int? AppointmentId=0)
        {
            SetPatientId(Note, PatientId);

            SetAppointmentId(Note, AppointmentId);

            SetToothId(Note, ToothId);

            SetOperationId(Note, OperationId);
        }

        private void SetOperationId(Note Note, int? OperationId)
        {
            if (OperationId == 0)
            {
                SaveNote(Note);
                return;
            }
            Note.OperationId = OperationId;
        }

        private void SetToothId(Note Note, int? ToothId)
        {
            if (ToothId == 0)
            {
                SaveNote(Note);
                return;
            }
            Note.AppointmentId = ToothId;
        }

        private void SetAppointmentId(Note Note, int? AppointmentId)
        {
            if (AppointmentId == 0)
            {
                SaveNote(Note);
                return;
            }
            Note.AppointmentId = AppointmentId;
        }

        private void SetPatientId(Note Note, int? PatientId)
        {
            if (PatientId == 0)
            {
                SaveNote(Note);
                return;
            }
            Note.PatientId = PatientId;
            SaveNote(Note);
        }

        public IEnumerable<Note> List(int? NoteId = 0)
        {
            if (NoteId == 0)
            {
                return NoteRepository.GetAll();
            }
            return NoteRepository.GetList(d => d.NoteId == NoteId);

        }


        public void Delete(Note Note)
        {

            NoteRepository.Remove(Note);

        }
    }
}

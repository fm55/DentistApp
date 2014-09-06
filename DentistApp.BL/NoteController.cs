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

        public NoteController()
        {
            NoteRepository = new GenericDataRepository<Note>();
        }

        public NoteController(IGenericDataRepository<Note> noteRepository)
        {
            NoteRepository = noteRepository;
        }

        public void SaveNote(Note Note)
        {
            if (Note.NoteId == 0)
            {
                NoteRepository.Add(Note);
            }
            else
            {
                NoteRepository.Update(Note);
            }

        }

        public void SaveNote(Note Note, int? PatientId=0, int? OperationId=0, int? ToothId=0, int? AppointmentId=0)
        {
            SetPatientId(Note, PatientId);

            SetAppointmentId(Note, AppointmentId);

            SetToothId(Note, ToothId);

            SetOperationId(Note, OperationId);


            SaveNote(Note);
        }

        public IEnumerable<Note> List(int? NoteId = 0)
        {
            if (NoteId == 0)
            {
                return NoteRepository.GetAll().OrderByDescending(d => d.DateCreated);
            }
            return NoteRepository.GetList(d => d.NoteId == NoteId).OrderByDescending(d => d.DateCreated);

        }


        public void Delete(Note Note)
        {

            NoteRepository.Remove(Note);

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
            Note.ToothId = ToothId;
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
        }


        public List<Note> GetNotesForPatient(int patientId)
        {
            return NoteRepository.GetList(d => d.PatientId == patientId).OrderByDescending(d => d.DateCreated).ToList();
        }

        public List<Note> GetNotesForPatientAndTooth(int patientId, int toothId)
        {
            return NoteRepository.GetList(d => d.PatientId == patientId && d.ToothId == toothId).OrderByDescending(d => d.DateCreated).ToList();
        }


        public List<Note> GetNotesForAppointment(int appointmentId)
        {
            return NoteRepository.GetList(d => d.AppointmentId == appointmentId).OrderByDescending(d => d.DateCreated).ToList();
        }

        public List<Note> GetNotesForAppointmentAndOperation(int appointmentId, int operationId)
        {
            return NoteRepository.GetList(d => d.AppointmentId == appointmentId && d.OperationId == operationId).OrderByDescending(d => d.DateCreated).ToList();
        }



        public List<Note> GetNotesForOperation(int operationId)
        {
            return NoteRepository.GetList(d => d.OperationId == operationId).OrderByDescending(d => d.DateCreated).ToList();
        }


        public List<Note> GetNotesForTooth(int toothId)
        {
            return NoteRepository.GetList(d => d.ToothId == toothId).OrderByDescending(d => d.DateCreated).ToList(); ;
        }

        public List<Note> GetNotesForSystem()
        {
            return NoteRepository.GetList(d => d.AppointmentId == null && d.OperationId == null && d.PatientId == null && d.ToothId == null).OrderByDescending(d => d.DateCreated).ToList();
        }


        
    }
}

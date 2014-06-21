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

        public void SaveNote(Note Note, Patient Patient, List<Operation> Operations, List<Tooth> Teeth, List<Note> Notes)
        {
            SaveNote(Note);
        }

        public IEnumerable<Note> List(int NoteId = 0)
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

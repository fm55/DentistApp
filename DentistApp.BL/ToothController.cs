using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.Model;
using DentistApp.DAL.DAL;
using DentistApp.DAL;

namespace DentistApp.BL
{
    public class ToothController
    {
        IGenericDataRepository<Tooth> ToothRepository { get; set; }
        IGenericDataRepository<Note> NoteRepository { get; set; }
        public ToothController()
        {
            ToothRepository = new GenericDataRepository<Tooth>();
            NoteRepository  = new GenericDataRepository<Note>();
        }

        public void SaveTooth(Tooth Tooth)
        {
            if (Tooth.ToothId == 0)
            {
                ToothRepository.Add(Tooth);
            }
            else
            {
                ToothRepository.Update(Tooth);
            }

        }

        public IEnumerable<Tooth> List(int ToothId = 0)
        {
            if (ToothId == 0)
            {
                return ToothRepository.GetAll();
            }
            return ToothRepository.GetList(d => d.ToothId == ToothId);

        }


        public void Delete(Tooth Tooth)
        {

            ToothRepository.Remove(Tooth);

        }




        public List<Note> GetNotesOfToothAndPatient(int toothId, int patientId)
        {
            var notes = new List<Note>();
            var notesOfTeeth = NoteRepository.GetAll(n=>n.Tooth, n=>n.Patient, n=>n.Operation, n=>n.Appointment).Where((n=>n.Patient.PatientId==patientId)).ToList<Note>();

            foreach (Note n in notesOfTeeth)
            {
                if (n.Tooth != null)
                {
                    if (n.Tooth.ToothId == toothId)
                    {
                        notes.Add(n);
                    }
                }
            }

            return notes;
        }
    }
}

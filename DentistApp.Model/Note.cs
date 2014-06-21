using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.Model;

namespace DentistApp.DAL
{
    public class Note : BaseEntity
    {
        public int NoteId { get; set; }
        public string Description { get; set; }
        public Patient Patient { get; set; }
        public Operation Operation { get; set; }
        public Tooth Tooth { get; set; }
        public Appointment Appointment { get; set; }
        public Note()
            : base()
        {
        }
    }
}

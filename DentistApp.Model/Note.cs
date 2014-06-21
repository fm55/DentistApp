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
        public int PatientId { get; set; }
        public int OperationId { get; set; }
        public int ToothId { get; set; }
        public int AppointmentId { get; set; }
        public Note()
            : base()
        {
        }
    }
}

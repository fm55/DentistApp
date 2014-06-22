using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentistApp.Model
{
    public class TeethAppointment:BaseEntity
    {
        [Key]
        public int TeethAppointmentId { get; set; }
        public virtual Tooth Teeth { get; set; }
        public virtual Appointment Appointment { get; set; }
    }
}

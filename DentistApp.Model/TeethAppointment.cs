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
        [NotMapped]
        public Tooth Teeth { get; set; }
        [NotMapped]
        public Appointment Appointment { get; set; }

        public int TeethId { get; set; }
        public int AppointmentId { get; set; }
    }
}

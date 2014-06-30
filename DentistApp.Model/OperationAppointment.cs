using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentistApp.Model
{
    public class OperationAppointment:BaseEntity
    {
        [Key]
        public int OperationAppointmentId { get; set; }
        
        public Operation Operation { get; set; }
        public Appointment Appointment { get; set; }
        public double Amount { get; set; }
    }
}

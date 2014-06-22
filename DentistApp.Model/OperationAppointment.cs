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
        public virtual int OperationAppointmentId { get; set; }
        
        public virtual Operation Operation { get; set; }
        public virtual Appointment Appointment { get; set; }
        public virtual double Amount { get; set; }
    }
}

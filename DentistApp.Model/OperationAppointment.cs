using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.DAL;

namespace DentistApp.Model
{
    public class OperationAppointment:BaseEntity
    {
        public virtual int OperationAppointmentId { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual Appointment Appointment { get; set; }
        public virtual double Amount { get; set; }
    }
}

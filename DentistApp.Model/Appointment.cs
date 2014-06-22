using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentistApp.DAL
{
    public class Appointment : BaseEntity
    {
        [Key]
        public int AppointmentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        
        public virtual Patient Patient { get; set; }
        public virtual List<OperationAppointment> Operation { get; set; }
        public decimal Amount { get; set; }
        public virtual List<TeethAppointment> Teeth { get; set; }
        public double AmountToPay { get; set; }
        public double AmountPaid { get; set; }

        public Appointment()
        {
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
        }
    }
}

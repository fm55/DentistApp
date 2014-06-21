using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.Model;

namespace DentistApp.DAL
{
    public class Patient : BaseEntity
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelNo1 { get; set; }
        public string TelNo2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Appointment> Appointments { get; set; }
        public Patient()
            : base()
        {
        }
    }
}

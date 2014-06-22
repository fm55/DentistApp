using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.DAL;
using System.ComponentModel.DataAnnotations;

namespace DentistApp.Model
{
    public class User:BaseEntity
    {

        public User() : base() { }
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelNo1 { get; set; }
        public string TelNo2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

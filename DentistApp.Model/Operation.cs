using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.DAL;
using System.ComponentModel.DataAnnotations;

namespace DentistApp.Model
{
    public class Operation:BaseEntity
    {
        [Key]
        public int OperationId{get;set;}
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}

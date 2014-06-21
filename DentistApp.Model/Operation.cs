using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.DAL;

namespace DentistApp.Model
{
    public class Operation:BaseEntity
    {
        public int OperationId{get;set;}
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}

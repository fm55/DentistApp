using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DentistApp.DAL
{
    public class Tooth : BaseEntity
    {
        [Key]
        public int ToothId { get; set; }
        public string Name{get;set;}
        public string Description { get; set; }
        public string PicUrl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentistApp.DAL
{
    public class Tooth : BaseEntity
    {
        public int ToothId { get; set; }
        public string Name{get;set;}
        public string Description { get; set; }
        public string PicUrl { get; set; }
    }
}

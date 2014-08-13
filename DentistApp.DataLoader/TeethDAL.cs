using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.DAL;
using System.IO;

namespace DentistApp.DataLoader
{
    public class TeethDAL
    {
        public static List<Tooth> GetTeeth()
        {
            var teeth = new List<Tooth>();
            string fileName = "..\\..\\teeth.csv";

            var lines = File.ReadAllLines(fileName);
            foreach (var line in lines)
            {
                var cols = line.Split(',');
                teeth.Add(new Tooth { Name = cols[1], Description = cols[1] });

            }
            return teeth;
        }


        


    }
}

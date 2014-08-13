using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.BL;
using DentistApp.DAL;

namespace DentistApp.DataLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please press a key");
            Console.WriteLine("1. To insert static list of teeth");
            var key = Console.ReadKey();
            if (key.KeyChar.ToString()=="1")
            {
                AddTeethToSystem();
            }
            else
            {
                Console.WriteLine("Didn't recognise key.  Please start data loader again.");
                Console.ReadKey();
            }

        }

        private static void AddTeethToSystem()
        {
            ToothController ToothController = new ToothController();
            var teeth = TeethDAL.GetTeeth();
            foreach (Tooth tooth in teeth)
            {
                ToothController.SaveTooth(tooth);
            }
            Console.WriteLine("\nAll Teeth added.");
            Console.ReadKey();
        }
    }
}

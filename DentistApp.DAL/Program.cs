using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using DentistApp.DAL.DAL;

namespace DentistApp.DAL
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = BaseDAL.GetContext())
            {
                // Create and save a new Blog 
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                var blog = new Patient { FirstName = name };
                db.Patients.Add(blog);
                db.SaveChanges();

                // Display all Blogs from the database 
                var query = from b in db.Patients
                            orderby b.FirstName
                            select b;

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.FirstName);
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            } 
        }
    }
}

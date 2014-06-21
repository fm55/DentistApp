using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.BL;
using System.Text.RegularExpressions;
using DentistApp.Model;
using DentistApp.DAL.DAL;


namespace DentistApp.DAL
{
    class Program
    {

        static void Main(string[] args)
        {
            int id = 1;
            if (id == 1)
            {
                Create32Teeth();
                CreateOperations();
                //CreatePatients();
            }
            //CreateAppointmentsForPatientsWithNotes();



            // Create and save a new Blog 
            PatientController p = new PatientController();
            var patients = p.List();

            foreach (var patient in patients)
                Console.WriteLine(patient.PatientId);



            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

        }

        private static void CreateOperations()
        {
            OperationController tD = new OperationController();
            for (int i = 1; i < 33; i++)
            {
                Operation t = new Operation();
                t.Description = "Operation " + i;
                t.EntityState = EntityState.Added;
                //t.OperationId = i;
                tD.SaveOperation(t);
            }
        }

        private static void CreateAppointmentsForPatientsWithNotes()
        {
            ToothController tD = new ToothController();
            OperationController oD = new OperationController();
            PatientController pD = new PatientController();
            AppointmentController aD = new AppointmentController();
            //two teeth
            var teethApps = tD.List().Take(2);
            //three operations
            var operations = oD.List().Take(3);
            //one patient
            var patient = pD.Get(1);
            //create appointment

            var operationApps = new List<OperationAppointment>();
           
            var teethAp = new List<TeethAppointment>();

            patient.EntityState = EntityState.Unchanged;
            var app = new Appointment()
            {
                Patient = patient,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                EntityState = EntityState.Added
            };

            app.Notes = new List<Note>();
            app.Notes.Add(new Note { Description = "Note", EntityState = EntityState.Added });
            
            //aD.SaveAppointment(app);
            
            
            
            var appFromDb = app;

            foreach (var o in operations)
            {
                o.EntityState = EntityState.Unchanged;
                operationApps.Add(new OperationAppointment { Operation = o, Appointment = app, EntityState = EntityState.Added });
            }

            foreach (var o in teethApps)
            {
                o.EntityState = EntityState.Unchanged;
                teethAp.Add(new TeethAppointment { Teeth = o, Appointment = app, EntityState = EntityState.Added });
            }

            appFromDb.Operation = operationApps;
            appFromDb.Teeth = teethAp;

            aD.SaveAppointment(appFromDb);
          
            IGenericDataRepository<OperationAppointment> AppointmentRepository  = new GenericDataRepository<OperationAppointment>();

            //AppointmentRepository.Add(operationApps.ToArray());

            

            var appFromDbNew = aD.List();

        }

        private static void CreatePatients()
        {
            PatientController target = new PatientController(); // TODO: Initialize to an appropriate value
            for (int i = 1; i < 33; i++)
            {
                Patient patient = new Patient { FirstName = "Test", LastName = "TestL", PatientId=i, EntityState=EntityState.Added }; // TODO: Initialize to an appropriate value
                target.Save(patient);
            }
        }

        private static void Create32Teeth()
        {
            ToothController tD = new ToothController();
            for (int i = 1; i < 33; i++)
            {
                Tooth t = new Tooth();
                t.Name = "Tooth " + i;
                t.EntityState = EntityState.Added;
                //t.ToothId = i;
                tD.SaveTooth(t);
            }
        }
    }
}

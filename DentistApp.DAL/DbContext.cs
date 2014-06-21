using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using DentistApp.Model;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentistApp.DAL
{
    public class NotAllowedDomainConfiguration : EntityTypeConfiguration<Tooth>
    {
        public NotAllowedDomainConfiguration()
        {
            //Table
            ToTable("Tooth");

            //Primary key
            HasKey(e => e.ToothId);

            //Properties
            Property(e => e.ToothId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

    public class DentistDbContext:DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tooth> Teeth { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<OperationAppointment> OperationAppointment { get; set; }
        public DbSet<TeethAppointment> TeethAppointment { get; set; }
        public DentistDbContext()
            : base("DenstistDatabase")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        /*
         * protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new NotAllowedDomainConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        */
    }
}

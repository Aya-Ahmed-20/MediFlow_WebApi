using MediFlowApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace MediFlowApi.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Doctor>Doctors { get; set; }
        public DbSet<Patient>Patient { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItem { get; set; }
        public DbSet<DispensingRecord> DispensingRecord { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 

            //  (Multiple Cascade Paths)
            builder.Entity<DispensingRecord>()
                .HasOne(d => d.Pharmacist)
                .WithMany()
                .HasForeignKey(d => d.PharmacistId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}

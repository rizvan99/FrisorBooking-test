using Hairdresser.Core.Entities;
using Hairdresser.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Infrastructure.Context
{
    public class HairdresserDbContext : DbContext
    {
        public HairdresserDbContext(DbContextOptions<HairdresserDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<Appointment> Appointment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => new { u.UserID });
            modelBuilder.Entity<Treatment>()
                .HasKey(s => new { s.TreatmentID });
            modelBuilder.Entity<Appointment>()
                .HasKey(a => new { a.AppointmentDateTime, a.EmployeeID });

        }
    }
}

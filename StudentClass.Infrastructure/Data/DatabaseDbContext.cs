using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentClass.Domain;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace StudentClass.Infrastructure.Data
{
    public class DatabaseDbContext: IdentityDbContext<AppUser>
    {
        public DatabaseDbContext(DbContextOptions<DatabaseDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentInClass>()
                .HasKey(x => new { x.StudentId, x.ClassId });
            modelBuilder.Entity<StudentInClass>()
                .HasOne(si => si.Student)
                .WithMany(si => si.StudentInClasses)
                .HasForeignKey(si => si.StudentId);
            modelBuilder.Entity<StudentInClass>()
                .HasOne(si => si.Class)
                .WithMany(si => si.StudentInClasses)
                .HasForeignKey(si => si.ClassId);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<StudentInClass> StudentInClass { get; set; }
    }
}
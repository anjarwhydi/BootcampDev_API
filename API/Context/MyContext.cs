using API.Model;
using Microsoft.EntityFrameworkCore;
using System;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options)
    {
    }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Profiling> Profilings { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<University> Universities { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasOne(a => a.Employee)
            .WithOne(e => e.Account)
            .HasForeignKey<Account>(a => a.NIK);

        modelBuilder.Entity<Profiling>()
            .HasOne(p => p.Account)
            .WithOne(a => a.Profiling)
            .HasForeignKey<Profiling>(p => p.NIK);

        modelBuilder.Entity<Profiling>()
            .HasOne(p => p.Education)
            .WithMany(e => e.Profilings)
            .HasForeignKey(p => p.Education_Id);

        modelBuilder.Entity<Education>()
            .HasOne(e => e.University)
            .WithMany(u => u.Educations)
            .HasForeignKey(e => e.University_Id);
    }
}

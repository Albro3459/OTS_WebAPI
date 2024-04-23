using API_Proj.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Infastructure
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Region> Region { get; set; }
        public DbSet<Office> Office { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Laptop> Laptop { get; set; }

        //DONT NEED CAUSE ITS IN PROGRAM.cs
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Development");
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Region>()
                .HasMany(r => r.Offices)
                .WithOne(o => o.Region)
                .HasForeignKey(o => o.RegionID);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.Employees)
                .WithMany(e => e.Offices)
                .UsingEntity(
                    "OfficeEmployee",
                    l => l.HasOne(typeof(Employee)).WithMany().HasForeignKey("EmployeesID").HasPrincipalKey("EmployeeID"),
                    r => r.HasOne(typeof(Office)).WithMany().HasForeignKey("OfficesID").HasPrincipalKey("OfficeID"),
                    j => j.HasKey("OfficesID", "EmployeesID"));

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Laptop)
                .WithOne(l => l.Employee)
                .HasForeignKey<Laptop>(l => l.EmployeeID);


            modelBuilder.Entity<Region>().HasData(new Region { RegionID = 1001, RegionName = "South West" },
                                                    new Region { RegionID = 1002, RegionName = "South" });

            modelBuilder.Entity<Office>().HasData(new Office { OfficeID = 1001, OfficeName = "Galvez Building", RegionID = 1001 },
                                                    new Office { OfficeID = 1002, OfficeName = "Deloitte Austin", RegionID = 1002 });

            modelBuilder.Entity<Employee>().HasData(new Employee { EmployeeID = 1001, EmployeeName = "Alex Brodsky", JobTitle = "Student Developer", YearsAtCompany = 0.5, CurrentProjects = new List<string>() { "Api Project" } });
            modelBuilder.Entity<Employee>().HasData(new Employee { EmployeeID = 1002, EmployeeName = "Hoa Nguyen", JobTitle = "Student Developer", YearsAtCompany = 0.5, CurrentProjects = new List<string>() { "Twidling Thumbs" } });
            modelBuilder.Entity<Employee>().HasData(new Employee { EmployeeID = 1003, EmployeeName = "Mr. Test", JobTitle = "Tester", YearsAtCompany = 0, CurrentProjects = new List<string>() { "Made to Test :(" } });
            modelBuilder.Entity<Employee>().HasData(new Employee { EmployeeID = 1004, EmployeeName = "Ahhh Test", JobTitle = "Tester", YearsAtCompany = 0, CurrentProjects = new List<string>() { "Made to Test :(" } });


            modelBuilder.Entity("OfficeEmployee").HasData(new { EmployeesID = 1001, OfficesID = 1001 },
                                                            new { EmployeesID = 1001, OfficesID = 1002 },
                                                            new { EmployeesID = 1002, OfficesID = 1001 });

            modelBuilder.Entity<Laptop>().HasData(new Laptop { LaptopID = 1001, LaptopName = "Brodsky's Laptop", EmployeeID = 1001 },
                                                    new Laptop { LaptopID = 1002, LaptopName = "Hoa's Laptop", EmployeeID = 1002 });

        }

    }

}

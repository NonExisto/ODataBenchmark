using Microsoft.EntityFrameworkCore;

namespace ODataBenchmark.DataModel
{
	public class BenchmarkContext : DbContext
	{
		public readonly int SeedSize = 500;
		public BenchmarkContext(DbContextOptions<BenchmarkContext> options)
			: base(options)
		{
		}
		public DbSet<Person> People { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<Scope> Scopes { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Manager> Managers { get; set; }
		public DbSet<JobTitle> JobTitles { get; set; }
		public DbSet<JobClassification> JobClassifications { get; set; }

		public DbSet<WorkItem> WorkItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			FakeData fakeData = new(SeedSize);

			modelBuilder.Entity<Scope>().HasData(fakeData.Scopes);
			modelBuilder.Entity<JobClassification>().HasData(fakeData.JobClassifications);
			modelBuilder.Entity<JobTitle>().HasMany(c => c.Scopes).WithMany("JobScopes").UsingEntity(e => e.HasData(fakeData.JobScopes));
			modelBuilder.Entity<JobTitle>().HasOne(c => c.JobClassification).WithMany().HasForeignKey(e => e.JobClassificationId);
			modelBuilder.Entity<JobTitle>().HasData(fakeData.JobTitles);
			
			modelBuilder.Entity<Person>();
			modelBuilder.Entity<Employee>().OwnsOne(c => c.HomeAddress).WithOwner().HasForeignKey(c => c.EmployeeId);
			modelBuilder.Entity<Employee>().OwnsOne(c => c.HomeAddress).HasData(fakeData.Addresses);
			modelBuilder.Entity<Employee>().HasMany(c => c.JobTitles).WithMany(c => c.Employees).UsingEntity(j => j.HasData(fakeData.EmployeeJobTitles));
			modelBuilder.Entity<Employee>().HasData(fakeData.Employees);

			modelBuilder.Entity<Manager>().HasMany(c => c.Subordinates).WithMany(c => c.Managers).UsingEntity(j => j.HasData(fakeData.EmployeeManagers));
			modelBuilder.Entity<Manager>().HasData(fakeData.Managers);
			modelBuilder.Entity<Customer>().HasData(fakeData.Customers);
			modelBuilder.Entity<Project>().HasMany(c => c.Members).WithMany(e => e.WorksOn).UsingEntity(j => j.HasData(fakeData.ProjectMembers));
			modelBuilder.Entity<Project>().HasOne(c => c.Superviser).WithMany(e => e.Supervise);
			modelBuilder.Entity<Project>().HasMany(c => c.Owners).WithMany(e => e.Owns).UsingEntity(j => j.HasData(fakeData.ProjectOwners));
			modelBuilder.Entity<Project>().HasMany(c => c.Scopes).WithMany("ProjectScopes").UsingEntity(j => j.HasData(fakeData.ProjectScopes));
			modelBuilder.Entity<Project>().HasMany(c => c.WorkItems).WithOne(c => c.Project).HasForeignKey(c => c.ProjectId);
			modelBuilder.Entity<Project>().HasData(fakeData.Projects);
			modelBuilder.Entity<WorkItem>().HasData(fakeData.WorkItems);
		}
	}
}

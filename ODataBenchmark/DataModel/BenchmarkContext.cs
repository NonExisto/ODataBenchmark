using Microsoft.EntityFrameworkCore;

namespace ODataBenchmark.DataModel
{
	public class BenchmarkContext : DbContext
	{
		public BenchmarkContext(DbContextOptions<BenchmarkContext> options)
			: base(options)
		{
		}
		public DbSet<Person> People { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<Scope> Scopes { get; set; }
		public DbSet<Emploee> Emploees { get; set; }
		public DbSet<Manager> Managers { get; set; }
		public DbSet<JobTitle> JobTitles { get; set; }
		public DbSet<JobClassification> JobClassifications { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			FakeData fakeData = new(500);

			modelBuilder.Entity<Scope>().HasData(fakeData.Scopes);
			modelBuilder.Entity<JobClassification>().HasData(fakeData.JobClassifications);
			modelBuilder.Entity<JobTitle>().HasMany(c => c.Scopes).WithMany("JobScopes");
			modelBuilder.Entity<JobTitle>().HasOne(c => c.JobClassification).WithMany().HasForeignKey(e => e.JobClassificationId);
			modelBuilder.Entity<JobTitle>().HasData(fakeData.JobTitles);

			modelBuilder.Entity<Emploee>().OwnsOne(c => c.HomeAddress).WithOwner();
			modelBuilder.Entity<Emploee>().HasMany(c => c.JobTitles).WithMany(c => c.Emploees);

			modelBuilder.Entity<Manager>().HasMany(c => c.Subordinates).WithOne(c => c.Manager);
			modelBuilder.Entity<Project>().HasMany(c => c.Members).WithMany(e => e.WorksOn);
			modelBuilder.Entity<Project>().HasOne(c => c.Superviser).WithMany(e => e.Supervise);
			modelBuilder.Entity<Project>().HasMany(c => c.Owners).WithMany(e => e.Owns);
			modelBuilder.Entity<Project>().HasMany(c => c.Scopes);
			modelBuilder.Entity<Customer>();
		}
	}
}

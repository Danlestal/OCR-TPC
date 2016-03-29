using System.Data.Entity;

namespace OCR_API.DataLayer
{
    public class OCR_TPC_Context : DbContext
    {
        public OCR_TPC_Context() : base("name=OCR_TPC_ConnectionString") 
        {
            Database.SetInitializer<OCR_TPC_Context>(new CreateDatabaseIfNotExists<OCR_TPC_Context>());
        }

        public DbSet<Contributor> Contributors { get; set; }

        public DbSet<ContributionPeriod> Periods { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContributionPeriod>()
                    .HasRequired<Contributor>(s => s.Contributor)
                    .WithMany(s => s.ContributionPeriods)
                    .HasForeignKey(s => s.ContributorRefId);
        }
    }
}
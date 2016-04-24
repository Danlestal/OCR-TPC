using System.Collections.Generic;
using System.Data.Entity;

namespace OCR_API.DataLayer
{
    public class OCR_TPC_Context : DbContext
    {
        public OCR_TPC_Context() : base("name=OCR_TPC_ConnectionString") 
        {
            Database.SetInitializer<OCR_TPC_Context>(new OCR_TPC_Initializer());
        }

        public DbSet<Contributor> Contributors { get; set; }

        public DbSet<ContributionPeriod> Periods { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContributionPeriod>()
                    .HasRequired<Contributor>(s => s.Contributor)
                    .WithMany(s => s.ContributionPeriods)
                    .HasForeignKey(s => s.ContributorRefId);
        }
    }


    public class OCR_TPC_Initializer : DropCreateDatabaseAlways<OCR_TPC_Context>
    {
        protected override void Seed(OCR_TPC_Context context)
        {
            context.Users.Add(new User() { Name = "admin", Password = "P3C4nS0ft" });
            base.Seed(context);
        }
    }
}
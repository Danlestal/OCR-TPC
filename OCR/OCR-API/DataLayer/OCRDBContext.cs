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

        public DbSet<Contribuidor> Contributors { get; set; }

        public DbSet<PeriodoContribucion> Periods { get; set; }

        public DbSet<Usuario> Users { get; set; }

        public DbSet<PdfToDelete> PdfToDelete { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeriodoContribucion>()
                    .HasRequired<Contribuidor>(s => s.Contributor)
                    .WithMany(s => s.PeriodosContribucion)
                    .HasForeignKey(s => s.ContribuidorRefId).WillCascadeOnDelete();
        }
    }

    public class OCR_TPC_Initializer : CreateDatabaseIfNotExists<OCR_TPC_Context>
    {
        protected override void Seed(OCR_TPC_Context context)
        {
            context.Users.Add(new Usuario() { Nombre = "admin", Password = "P3C4nS0ft" });
            base.Seed(context);
        }
    }
}
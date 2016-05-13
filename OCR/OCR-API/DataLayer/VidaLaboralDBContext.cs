using System.Collections.Generic;
using System.Data.Entity;

namespace OCR_API.DataLayer
{
    public class VidaLaboral_Context : DbContext
    {
        public VidaLaboral_Context() : base("name=VidaLaboral_ConnectionString")
        {
            Database.SetInitializer<VidaLaboral_Context>(new CreateDatabaseIfNotExists<VidaLaboral_Context>());
        }

        public DbSet<DatoPersonal> DatoPersonal { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeriodoVidaLaboral>()
                        .HasRequired<DatoPersonal>(s => s.Persona)
                        .WithMany(s => s.PeriodosVidaLaboral)
                        .HasForeignKey(s => s.DatoPersonalRefId);
        }
    }


}
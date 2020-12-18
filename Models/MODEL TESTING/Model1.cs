namespace allpax_service_record.Models.MODEL_TESTING
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<tbl_resourceTypes> tbl_resourceTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_resourceTypes>()
                .Property(e => e.resourceType)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_resourceTypes>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_resourceTypes>()
                .Property(e => e.rate)
                .HasPrecision(5, 2);
        }
    }
}

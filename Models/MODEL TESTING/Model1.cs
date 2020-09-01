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

        public virtual DbSet<tbl_dailyReportTimeEntry> tbl_dailyReportTimeEntry { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_dailyReportTimeEntry>()
                .Property(e => e.workDescription)
                .IsUnicode(false);
        }
    }
}

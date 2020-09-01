namespace allpax_service_record.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using allpax_service_record.Models.MODEL_TESTING;

    public partial class allpaxServiceRecordEntities : DbContext
    {
        public allpaxServiceRecordEntities()
            : base("name=allpaxServiceRecordEntities")
        {
        }

        public virtual DbSet<tbl_customers> tbl_customers { get; set; }
        public virtual DbSet<tbl_dailyReport> tbl_dailyReport { get; set; }
        public virtual DbSet<tbl_dailyReportUsers> tbl_dailyReportUsers { get; set; }
        public virtual DbSet<tbl_dailyReportTimeEntry> tbl_dailyReportTimeEntry { get; set; }
        public virtual DbSet<tbl_dailyReportTimeEntryUsers> tbl_dailyReportTimeEntryUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_customers>()
                .Property(e => e.customerCode)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customers>()
                .Property(e => e.customerName)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customers>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dailyReport>()
                .Property(e => e.jobID)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dailyReport>()
                .Property(e => e.equipment)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dailyReportUsers>()
                .Property(e => e.userName)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dailyReportTimeEntry>()
                .Property(e => e.workDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_dailyReportTimeEntryUsers>()
                .Property(e => e.userName)
                .IsUnicode(false);

        }

        public System.Data.Entity.DbSet<allpax_service_record.Models.View_Models.vm_workDesc> vm_workDesc { get; set; }
    }
}

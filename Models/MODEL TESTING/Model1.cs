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

        public virtual DbSet<tbl_jobCorrespondents> tbl_jobCorrespondents { get; set; }
        public virtual DbSet<tbl_jobResourceTypes> tbl_jobResourceTypes { get; set; }
        public virtual DbSet<tbl_Jobs> tbl_Jobs { get; set; }
        public virtual DbSet<tbl_jobSubJobs> tbl_jobSubJobs { get; set; }
        public virtual DbSet<tbl_resourceTypes> tbl_resourceTypes { get; set; }
        public virtual DbSet<tbl_subJobTypes> tbl_subJobTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_jobCorrespondents>()
                .Property(e => e.jobID)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_jobCorrespondents>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_jobCorrespondents>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_jobResourceTypes>()
                .Property(e => e.jobID)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_jobResourceTypes>()
                .Property(e => e.rate)
                .HasPrecision(6, 2);

            modelBuilder.Entity<tbl_Jobs>()
                .Property(e => e.jobID)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Jobs>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Jobs>()
                .Property(e => e.customerCode)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Jobs>()
                .Property(e => e.customerContact)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Jobs>()
                .Property(e => e.location)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Jobs>()
                .HasMany(e => e.tbl_jobCorrespondents)
                .WithRequired(e => e.tbl_Jobs)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Jobs>()
                .HasMany(e => e.tbl_jobResourceTypes)
                .WithRequired(e => e.tbl_Jobs)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_jobSubJobs>()
                .Property(e => e.jobID)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_resourceTypes>()
                .Property(e => e.resourceType)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_subJobTypes>()
                .Property(e => e.description)
                .IsUnicode(false);
        }
    }
}

namespace allpax_sale_miner.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class allpax_sale_minerEntities : DbContext
    {
        public allpax_sale_minerEntities()
            : base("name=allpax_sale_minerEntities")
        {
        }

        public virtual DbSet<tbl_customer> tbl_customer { get; set; }
        public virtual DbSet<tbl_customer_eqpmt> tbl_customer_eqpmt { get; set; }
        public virtual DbSet<tbl_customer_event> tbl_customer_event { get; set; }
        public virtual DbSet<tbl_eqpmt_kits_avlbl> tbl_eqpmt_kits_avlbl { get; set; }
        public virtual DbSet<tbl_eqpmt_kits_current> tbl_eqpmt_kits_current { get; set; }
        public virtual DbSet<tbl_eqpmt_type> tbl_eqpmt_type { get; set; }
        public virtual DbSet<tbl_eqpmt_type_mgmt> tbl_eqpmt_type_mgmt { get; set; }
        public virtual DbSet<tbl_event_type> tbl_event_type { get; set; }
        public virtual DbSet<tbl_kit> tbl_kit { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_customer>()
                .Property(e => e.customerCode)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer>()
                .Property(e => e.city)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer>()
                .Property(e => e.state)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer>()
                .Property(e => e.zipCode)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer>()
                .HasMany(e => e.tbl_customer_eqpmt)
                .WithRequired(e => e.tbl_customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_customer>()
                .HasMany(e => e.tbl_customer_event)
                .WithRequired(e => e.tbl_customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_customer_eqpmt>()
                .Property(e => e.customerCode)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer_eqpmt>()
                .Property(e => e.machineID)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer_eqpmt>()
                .Property(e => e.eqpmtType)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer_eqpmt>()
                .Property(e => e.model)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer_eqpmt>()
                .Property(e => e.jobNum)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer_event>()
                .Property(e => e.customerCode)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer_event>()
                .Property(e => e.eventType)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_customer_event>()
                .Property(e => e.eventID)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_eqpmt_kits_avlbl>()
                .Property(e => e.eqpmtType)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_eqpmt_kits_avlbl>()
                .Property(e => e.model)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_eqpmt_kits_avlbl>()
                .Property(e => e.kitID)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_eqpmt_kits_current>()
                .Property(e => e.machineID)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_eqpmt_kits_current>()
                .Property(e => e.kitID)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_eqpmt_type>()
                .Property(e => e.eqpmtType)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_eqpmt_type>()
                .Property(e => e.model)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_eqpmt_type>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_eqpmt_type_mgmt>()
                .Property(e => e.eqpmtType)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_eqpmt_type_mgmt>()
                .HasMany(e => e.tbl_customer_eqpmt)
                .WithRequired(e => e.tbl_eqpmt_type_mgmt)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_eqpmt_type_mgmt>()
                .HasMany(e => e.tbl_eqpmt_type)
                .WithRequired(e => e.tbl_eqpmt_type_mgmt)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_event_type>()
                .Property(e => e.eventID)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_event_type>()
                .Property(e => e.eventType)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.kitID)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.filePath)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
               .Property(e => e.descKitItem_1)
               .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.partNoKitItem_1)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.descKitItem_2)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.partNoKitItem_2)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.descKitItem_3)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.partNoKitItem_3)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.descKitItem_4)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.partNoKitItem_4)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.descKitItem_5)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.partNoKitItem_5)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.descKitItem_6)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.partNoKitItem_6)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.descKitItem_7)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.partNoKitItem_7)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.descKitItem_8)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.partNoKitItem_8)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.descKitItem_9)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.partNoKitItem_9)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.descKitItem_10)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.partNoKitItem_10)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .HasMany(e => e.tbl_eqpmt_kits_avlbl)
                .WithRequired(e => e.tbl_kit)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_kit>()
               .Property(e => e.infoPackage)
               .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.mechDrawing)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_kit>()
                .Property(e => e.priceSheet)
                .IsUnicode(false);
        }
    }
}

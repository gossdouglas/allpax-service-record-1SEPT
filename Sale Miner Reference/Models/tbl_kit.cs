namespace allpax_sale_miner.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cmps411.tbl_kit")]
    public partial class tbl_kit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_kit()
        {
            tbl_eqpmt_kits_avlbl = new HashSet<tbl_eqpmt_kits_avlbl>();
        }

        [Key]
        [StringLength(50)]
        public string kitID { get; set; }

        [Required]
        [StringLength(50)]
        public string description { get; set; }

        [Required]
        [StringLength(500)]
        public string filePath { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
      
        [StringLength(50)]
        public string descKitItem_1 { get; set; }
        [StringLength(50)]
        public string partNoKitItem_1 { get; set; }
        [StringLength(50)]
        public string descKitItem_2 { get; set; }
        [StringLength(50)]
        public string partNoKitItem_2 { get; set; }
        [StringLength(50)]
        public string descKitItem_3 { get; set; }
        [StringLength(50)]
        public string partNoKitItem_3 { get; set; }
        [StringLength(50)]
        public string descKitItem_4 { get; set; }
        [StringLength(50)]
        public string partNoKitItem_4 { get; set; }
        [StringLength(50)]
        public string descKitItem_5 { get; set; }
        [StringLength(50)]
        public string partNoKitItem_5 { get; set; }
        [StringLength(50)]
        public string descKitItem_6 { get; set; }
        [StringLength(50)]
        public string partNoKitItem_6 { get; set; }
        [StringLength(50)]
        public string descKitItem_7 { get; set; }
        [StringLength(50)]
        public string partNoKitItem_7 { get; set; }
        [StringLength(50)]
        public string descKitItem_8 { get; set; }
        [StringLength(50)]
        public string partNoKitItem_8 { get; set; }
        [StringLength(50)]
        public string descKitItem_9 { get; set; }
        [StringLength(50)]
        public string partNoKitItem_9 { get; set; }
        [StringLength(50)]
        public string descKitItem_10 { get; set; }
        [StringLength(50)]
        public string partNoKitItem_10 { get; set; }
        public string infoPackage { get; set; }

        public string mechDrawing { get; set; }

        public string priceSheet { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_eqpmt_kits_avlbl> tbl_eqpmt_kits_avlbl { get; set; }
    }
}

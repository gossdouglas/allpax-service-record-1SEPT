namespace allpax_sale_miner.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cmps411.tbl_eqpmt_type_mgmt")]
    public partial class tbl_eqpmt_type_mgmt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_eqpmt_type_mgmt()
        {
            tbl_customer_eqpmt = new HashSet<tbl_customer_eqpmt>();
            tbl_eqpmt_type = new HashSet<tbl_eqpmt_type>();
        }

        [Key]
        [StringLength(50)]
        public string eqpmtType { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_customer_eqpmt> tbl_customer_eqpmt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_eqpmt_type> tbl_eqpmt_type { get; set; }
    }
}

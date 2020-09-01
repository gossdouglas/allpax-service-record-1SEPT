namespace allpax_sale_miner.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cmps411.tbl_customer_eqpmt")]
    public partial class tbl_customer_eqpmt
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string customerCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string machineID { get; set; }

        [Required]
        [StringLength(50)]
        public string eqpmtType { get; set; }

        [Required]
        [StringLength(50)]
        public string model { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [StringLength(50)]
        public string jobNum { get; set; }

        public virtual tbl_customer tbl_customer { get; set; }

        public virtual tbl_eqpmt_type_mgmt tbl_eqpmt_type_mgmt { get; set; }
    }
}

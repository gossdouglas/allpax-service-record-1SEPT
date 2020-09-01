namespace allpax_sale_miner.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cmps411.tbl_sales_opportunities")]
    public partial class tbl_sales_opportunities
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string customerCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string machineID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string kitID { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string description { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(10)]
        public string supportFiles { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "date")]
        public DateTime dateEntered { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
    }
}

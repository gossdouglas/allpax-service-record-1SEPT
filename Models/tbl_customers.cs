namespace allpax_service_record.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_customers
    {
        [Key]
        [StringLength(3)]
        public string customerCode { get; set; }

        [Required]
        [StringLength(50)]
        public string customerName { get; set; }

        [Required]
        [StringLength(50)]
        public string address { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
    }
}

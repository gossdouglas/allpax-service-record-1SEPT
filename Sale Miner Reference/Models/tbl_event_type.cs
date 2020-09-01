namespace allpax_sale_miner.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cmps411.tbl_event_type")]
    public partial class tbl_event_type
    {
        [StringLength(50)]
        public string eventID { get; set; }

        [Required]
        [StringLength(50)]
        public string eventType { get; set; }

        public int id { get; set; }
    }
}

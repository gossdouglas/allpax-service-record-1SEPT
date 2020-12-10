namespace allpax_service_record.Models.MODEL_TESTING
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_jobResourceTypes
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string jobID { get; set; }

        [Key]
        [Column(Order = 1)]
        public byte resourceTypeID { get; set; }

        public decimal rate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public virtual tbl_Jobs tbl_Jobs { get; set; }
    }
}

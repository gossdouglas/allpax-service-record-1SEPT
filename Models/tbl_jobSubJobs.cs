namespace allpax_service_record.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_jobSubJobs
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string jobID { get; set; }

        [Key]
        [Column(Order = 1)]
        public byte subJobID { get; set; }

        public virtual tbl_Jobs tbl_Jobs { get; set; }
    }
}

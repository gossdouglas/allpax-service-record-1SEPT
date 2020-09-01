namespace allpax_service_record.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_dailyReportUsers
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int dailyReportID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(16)]
        public string userName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public virtual tbl_dailyReport tbl_dailyReport { get; set; }
    }
}

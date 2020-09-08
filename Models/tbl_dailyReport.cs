namespace allpax_service_record.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_dailyReport
    {
        [Required]
        [StringLength(8)]
        public string jobID { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        public int subJobID { get; set; }

        public TimeSpan startTime { get; set; }

        public TimeSpan endTime { get; set; }

        public int lunchHours { get; set; }

        public string equipment { get; set; }

        [Key]
        public int dailyReportID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_dailyReportUsers> tbl_dailyReportUsers { get; set; }
        [StringLength(16)]
        public string dailyReportAuthor { get; set; }
    }
}

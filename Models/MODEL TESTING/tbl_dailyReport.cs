namespace allpax_service_record.Models.MODEL_TESTING
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

        [Required]
        public string equipment { get; set; }

        [Required]
        [StringLength(16)]
        public string dailyReportAuthor { get; set; }

        [Key]
        public int dailyReportID { get; set; }

        public int? submissionStatus { get; set; }
    }
}

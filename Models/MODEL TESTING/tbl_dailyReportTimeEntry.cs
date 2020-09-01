namespace allpax_service_record.Models.MODEL_TESTING
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_dailyReportTimeEntry
    {
        public int dailyReportID { get; set; }

        [Required]
        [StringLength(1024)]
        public string workDescription { get; set; }

        public int workDescriptionCategory { get; set; }

        public int? hours { get; set; }

        [Key]
        public int timeEntryID { get; set; }
    }
}

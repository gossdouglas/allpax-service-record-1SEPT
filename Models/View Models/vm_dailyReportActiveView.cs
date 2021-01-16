namespace allpax_service_record.Models.View_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vm_dailyReportViewActive
    {
        public string date { get; set; }
        //[Column(TypeName = "date")]
        //public DateTime date { get; set; }
        public string jobID { get; set; }
        public string subJobDescription { get; set; }
        public List<string> teamUserNames { get; set; }
        public List<string> teamNames { get; set; }
        public List<string> teamShortNames { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public int lunchHours { get; set; }
        public decimal totalHours { get; set; }
        public decimal delayHours { get; set; }
        public decimal wntyDelayHours { get; set; }
        public Boolean active { get; set; }
        [Key]
        public int dailyReportID { get; set; }

    }
}

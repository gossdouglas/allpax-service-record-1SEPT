namespace allpax_service_record.Models.View_Models

{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vm_wntyDelays
    {
        [Key]
        public int timeEntryID { get; set; }
        //[Key]
        //public System.Int32 timeEntryID { get; set; }

        public int dailyReportID { get; set; }

        public string workDescription { get; set; }
        public string userName { get; set; }
        public int workDescriptionCategory { get; set; }
        public int hours { get; set; }
        public List<string> userNames { get; set; }
        public List<string> userShortNames { get; set; }

    }
}

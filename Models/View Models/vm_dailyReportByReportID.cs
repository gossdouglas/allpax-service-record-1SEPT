namespace allpax_service_record.Models.View_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vm_dailyReportByReportID
    {
        public int dailyReportID { get; set; }

        public Boolean active { get; set; }

        //public DateTime date { get; set; }
        public string date { get; set; }

        public string startTime { get; set; }
        public string endTime { get; set; }

        public int lunchHours { get; set; }

        public string customerCode { get; set; }

        public string jobID { get; set; }

        //public string subJobDescription { get; set; }

        public string description { get; set; }
        public string equipment { get; set; }

        public string customerName { get; set; }

        public string customerContact { get; set; }

        public string address { get; set; }

        public string subJobID { get; set; }

        public string team { get; set; }
        public List<string> names { get; set; }
        public List<string> shortNames { get; set; }
        public List<string> jobCorrespondentEmail { get; set; }
        public List<string> jobCorrespondentName { get; set; }

        public List<string> timeEntryDeleteArr { get; set; }//list of time entries to be deleted

        public string workDescription { get; set; }
        public string dailyReportAuthor { get; set; }
        public int submissionStatus { get; set; }

        public Models.tbl_dailyReportTimeEntry tbl_dailyReportTimeEntry;

        public List<vm_workDesc> workDescArr { get; set; }
        public List<vm_delays> delaysArr { get; set; }
        public List<vm_wntyDelays> wntyDelaysArr { get; set; }

    }
}

namespace allpax_service_record.Models.View_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using allpax_service_record.Models;

    public partial class vm_Jobs
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public vm_Jobs()
        //{
        //    tbl_jobCorrespondents = new HashSet<tbl_jobCorrespondents>();
        //}

        [Key]
        [StringLength(8)]
        public string jobID { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        [StringLength(3)]
        public string customerCode { get; set; }
        [StringLength(50)]
        public string customerContact { get; set; }
        public bool active { get; set; }
        [StringLength(255)]
        public string location { get; set; }
        public string nrmlHoursStart { get; set; }
        public string nrmlHoursEnd { get; set; }
        public string nrmlHoursDaily { get; set; }
        public Boolean dblTimeHours { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<tbl_jobCorrespondents> tbl_jobCorrespondents { get; set; }

        public List<tbl_Jobs> jobs { get; set; }
        public List<tbl_subJobTypes> subJobTypes { get; set; }
        public List<tbl_resourceTypes> resourceTypes { get; set; }
        public List<vm_jobCrspdtInfo> jobCrspdtInfo { get; set; }

        //an array that holds which sub-job types are added on a job creation
        public string[] subJobTypes_Add { get; set; }

    }
}

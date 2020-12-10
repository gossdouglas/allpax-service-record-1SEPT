namespace allpax_service_record.Models.MODEL_TESTING
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Jobs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Jobs()
        {
            tbl_jobCorrespondents = new HashSet<tbl_jobCorrespondents>();
            tbl_jobResourceTypes = new HashSet<tbl_jobResourceTypes>();
            tbl_jobSubJobs = new HashSet<tbl_jobSubJobs>();
        }

        [Key]
        [StringLength(8)]
        public string jobID { get; set; }

        [Required]
        [StringLength(255)]
        public string description { get; set; }

        [Required]
        [StringLength(3)]
        public string customerCode { get; set; }

        [Required]
        [StringLength(50)]
        public string customerContact { get; set; }

        public bool active { get; set; }

        [Required]
        [StringLength(255)]
        public string location { get; set; }

        public TimeSpan? nrmlHoursStart { get; set; }

        public TimeSpan? nrmlHoursEnd { get; set; }

        public bool? dblTimeHours { get; set; }

        public int? nrmlHoursDaily { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_jobCorrespondents> tbl_jobCorrespondents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_jobResourceTypes> tbl_jobResourceTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_jobSubJobs> tbl_jobSubJobs { get; set; }
    }
}

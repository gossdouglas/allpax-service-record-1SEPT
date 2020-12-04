namespace allpax_service_record.Models
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
        }

        [Key]
        [StringLength(8)]
        public string jobID { get; set; }

        [StringLength(255)]
        public string description { get; set; }

        [StringLength(3)]
        public string customerCode { get; set; }

        [StringLength(50)]
        public string customerContact { get; set; }

        public bool? active { get; set; }

        [StringLength(255)]
        public string location { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_jobCorrespondents> tbl_jobCorrespondents { get; set; }
    }
}

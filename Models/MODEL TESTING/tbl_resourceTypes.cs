namespace allpax_service_record.Models.MODEL_TESTING
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_resourceTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte resourceTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string resourceType { get; set; }

        [Required]
        [StringLength(50)]
        public string description { get; set; }

        public decimal rate { get; set; }
    }
}

namespace allpax_service_record.Models.View_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vm_resourceTypes
    {
        public string resourceTypeID { get; set; }
        public string resourceType { get; set; }
        public string description { get; set; }
        public decimal rate { get; set; }
    }
}

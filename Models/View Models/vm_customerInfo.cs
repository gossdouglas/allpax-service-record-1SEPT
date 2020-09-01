namespace allpax_service_record.Models.View_Models

{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vm_customerInfo
    {
        public string customerCode { get; set; }

        public string customerName { get; set; }

        public string address { get; set; }

        public string customerContact { get; set; }

    }
}

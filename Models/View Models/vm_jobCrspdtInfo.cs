namespace allpax_service_record.Models.View_Models

{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vm_jobCrspdtInfo
    {      
        public string jobCrspdtName { get; set; }
        public string jobCrspdtEmail { get; set; }
    }
}

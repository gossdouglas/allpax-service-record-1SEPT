namespace allpax_service_record.Models.View_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vm_userAcctInfo
    {
        public string UserName { get; set; }
        public string name { get; set; }
        public string ShortName { get; set; }
        public bool admin { get; set; }
        public bool active { get; set; }
        public string Email { get; set; }
    }
}

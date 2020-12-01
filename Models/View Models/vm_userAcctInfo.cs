namespace allpax_service_record.Models.View_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vm_userAcctInfo
    {
        public string aspNetId { get; set; }

        //public string UserName { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        //public string name { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string name { get; set; }

        //public string ShortName { get; set; }
        [Required]
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        //public bool admin { get; set; }
        [Required]
        [Display(Name = "Admin Status")]
        public bool admin { get; set; }

        //public bool active { get; set; }
        [Required]
        [Display(Name = "Active")]
        public bool active { get; set; }

        //public string Email { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

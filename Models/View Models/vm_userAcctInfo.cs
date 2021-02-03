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

        [Required]
        [Display(Name = "Full Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Admin Status")]
        public bool admin { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool active { get; set; }

        public List<string> ModelErrors { get; set; }
    }
}

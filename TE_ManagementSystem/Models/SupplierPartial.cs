using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    [MetadataType(typeof(SupplierMetadata))]
    public partial class Supplier
    {
        public class SupplierMetadata
        {
            public int ID { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "供應商名稱")]
            public string Name { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "Phone")]
            public string Phone { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "Address")]
            public string Address { get; set; }
            [Display(Name = "更新人員")]
            public string UpdateEmployee { get; set; }
        }
    }
}
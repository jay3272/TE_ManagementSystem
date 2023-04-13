using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {
        public class CustomerMetadata
        {
            public int ID { get; set; }
            [Required(ErrorMessage = "Name is required.")]
            [Display(Name = "客戶姓名")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Customer Code is required.")]
            [Display(Name = "客戶代碼")]
            public string CustCode { get; set; }
        }
    }
}
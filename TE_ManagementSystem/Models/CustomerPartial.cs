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
            [Required(ErrorMessage = "required.")]
            [Display(Name = "客戶姓名")]
            public string Name { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "客戶代碼")]
            public string CustCode { get; set; }
            public string Spare1 { get; set; }
            public string Spare2 { get; set; }
            public string Spare3 { get; set; }
            public string Spare4 { get; set; }
            public string Spare5 { get; set; }
            [Display(Name = "更新人員")]
            public string UpdateEmployee { get; set; }

        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    [MetadataType(typeof(KindMetadata))]
    public partial class Kind
    {
        public class KindMetadata
        {
            public int ID { get; set; }
            [Required(ErrorMessage = "Name is required.")]
            [Display(Name = "種類名稱")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Number is required.")]
            [Display(Name = "種類代號")]
            public string Number { get; set; }
            [Display(Name = "更新人員")]
            public string UpdateEmployee { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    [MetadataType(typeof(KindProcessMetadata))]
    public partial class KindProcess
    {
        public class KindProcessMetadata
        {
            public int ID { get; set; }
            [Required(ErrorMessage = "Name is required.")]
            [Display(Name = "製程名稱")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Number is required.")]
            [Display(Name = "製程代號")]
            public string Number { get; set; }
            [Display(Name = "更新人員")]
            public string UpdateEmployee { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    [MetadataType(typeof(KPNMetadata))]
    public partial class KPN
    {
        public class KPNMetadata
        {
            public int ID { get; set; }
            [Required(ErrorMessage = "Name is required.")]
            [Display(Name = "料號")]
            public string Name { get; set; }
            [Display(Name = "更新人員")]
            public string UpdateEmployee { get; set; }
        }
    }
}
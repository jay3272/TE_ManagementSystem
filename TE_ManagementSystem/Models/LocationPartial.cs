using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    [MetadataType(typeof(LocationMetadata))]
    public partial class Location
    {
        public class LocationMetadata
        {
            public int ID { get; set; }
            [Required(ErrorMessage = "Name is required.")]
            [Display(Name = "治具室名稱")]
            public string Name { get; set; }
            [Required(ErrorMessage = "RackPosition is required.")]
            [Display(Name = "儲位名稱")]
            public string RackPosition { get; set; }
            public bool Status { get; set; }
            [Display(Name = "更新人員")]
            public string UpdateEmployee { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    [MetadataType(typeof(MeProductMetadata))]
    public partial class MeProduct
    {
        public class MeProductMetadata
        {
            public int ID { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "治具名稱")]
            public string ProdName { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "治具種類")]
            public int KindID { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "製程種類")]
            public int KindProcessID { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "客戶")]
            public int CustomerID { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "供應商")]
            public int SupplierID { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "負責人")]
            public string Opid { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "數量")]
            public int Quantity { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "ShiftTime")]
            public int ShiftTime { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "有鉛")]
            public bool Pb { get; set; }
            [Display(Name = "圖片")]
            public string Image { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "料號")]
            public string ComList { get; set; }
            [Display(Name = "備註")]
            public string Spare1 { get; set; }
        }
    }
}
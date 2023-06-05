using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TE_ManagementSystem.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        public class ProductMetadata
        {
            public string NumberID { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "RFID")]
            public string RFID { get; set; }
            public string Status { get; set; }
            public int LocationID { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "治具名稱")]
            public int EngID { get; set; }
            public Nullable<System.DateTime> StockDate { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "年限")]
            public string Life { get; set; }
            public Nullable<System.DateTime> LastBorrowDate { get; set; }
            public Nullable<System.DateTime> LastReturnDate { get; set; }
            public Nullable<System.DateTime> UseLastDate { get; set; }
            [Required(ErrorMessage = "required.")]
            [Display(Name = "可借用")]
            public bool Usable { get; set; }
            public bool Overdue { get; set; }
            [Display(Name = "更新人員")]
            public string UpdateEmployee { get; set; }
            [Display(Name = "數量")]
            public Nullable<int> OldQuantity { get; set; }
            [Display(Name = "供應商")]
            public string OldSupplier { get; set; }
            [Display(Name = "報廢狀態")]
            public string OldState { get; set; }
            [Display(Name = "料號")]
            public string OldKpn { get; set; }
            [Display(Name = "附件")]
            public string OldAppendix { get; set; }
        }
    }
}